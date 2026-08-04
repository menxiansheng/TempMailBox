using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TempMailBox.Models;
using TempMailBox.Services;

namespace TempMailBox.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly MailTmService _mailService;
        private readonly DispatcherTimer _pollTimer;
        private string? _currentPassword;
        private string? _currentAccountId;

        [ObservableProperty]
        private string _currentEmail = string.Empty;

        [ObservableProperty]
        private string _statusText = "点击「生成邮箱」开始使用";

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private bool _hasAccount;

        [ObservableProperty]
        private string? _currentToken;

        [ObservableProperty]
        private Message? _selectedMessage;

        [ObservableProperty]
        private Message? _messageDetail;

        [ObservableProperty]
        private bool _isLoadingDetail;

        [ObservableProperty]
        private bool _isCopied;

        [ObservableProperty]
        private int _messageCount;

        public ObservableCollection<Message> Messages { get; } = new();
        public ObservableCollection<string> EmailHistory { get; } = new();

        public MainViewModel()
        {
            _mailService = new MailTmService();
            _pollTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(10)
            };
            _pollTimer.Tick += async (s, e) => await PollMessagesAsync();
        }

        [RelayCommand]
        private async Task GenerateEmailAsync()
        {
            try
            {
                IsLoading = true;
                StatusText = "正在获取可用域名...";

                // Stop polling for old account
                _pollTimer.Stop();

                // Get available domains
                var domains = await _mailService.GetDomainsAsync();
                if (domains.Count == 0)
                {
                    StatusText = "❌ 没有可用域名，请稍后重试";
                    return;
                }

                var domain = domains.First(d => d.IsActive);

                // Generate random username
                var random = new Random();
                var username = $"user{random.Next(100000, 999999)}{(char)('a' + random.Next(26))}";
                var address = $"{username}@{domain.DomainName}";
                var password = Guid.NewGuid().ToString("N") + "Aa1!";

                StatusText = $"正在创建邮箱 {address}...";

                // Create account
                var account = await _mailService.CreateAccountAsync(address, password);
                if (account == null)
                {
                    StatusText = "❌ 创建邮箱失败，请重试";
                    return;
                }

                // Get token
                StatusText = "正在获取认证令牌...";
                var token = await _mailService.GetTokenAsync(address, password);
                if (string.IsNullOrEmpty(token))
                {
                    StatusText = "❌ 获取令牌失败，请重试";
                    return;
                }

                // Store credentials
                CurrentEmail = address;
                _currentPassword = password;
                _currentAccountId = account.Id;
                CurrentToken = token;
                HasAccount = true;

                // Add to history
                EmailHistory.Insert(0, address);

                // Clear messages
                Messages.Clear();
                SelectedMessage = null;
                MessageDetail = null;
                MessageCount = 0;

                StatusText = "✅ 邮箱已就绪，等待接收邮件...";

                // Start polling
                _pollTimer.Start();
            }
            catch (Exception ex)
            {
                StatusText = $"❌ 错误: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task CopyEmailAsync()
        {
            if (!string.IsNullOrEmpty(CurrentEmail))
            {
                try
                {
                    Clipboard.SetText(CurrentEmail);
                    IsCopied = true;
                    StatusText = "📋 邮箱地址已复制到剪贴板";

                    await Task.Delay(1500);
                    IsCopied = false;
                }
                catch
                {
                    StatusText = "❌ 复制失败";
                    IsCopied = false;
                }
            }
        }

        [RelayCommand]
        private async Task RefreshMessagesAsync()
        {
            if (string.IsNullOrEmpty(CurrentToken)) return;

            try
            {
                IsLoadingDetail = true;
                StatusText = "🔄 正在刷新邮件...";
                await PollMessagesAsync();
                StatusText = $"✅ 刷新完成，共 {MessageCount} 封邮件";
            }
            catch (Exception ex)
            {
                StatusText = $"❌ 刷新失败: {ex.Message}";
            }
            finally
            {
                IsLoadingDetail = false;
            }
        }

        [RelayCommand]
        private async Task ViewMessageAsync(Message? message)
        {
            if (message == null || string.IsNullOrEmpty(CurrentToken)) return;

            try
            {
                IsLoadingDetail = true;
                var detail = await _mailService.GetMessageAsync(CurrentToken, message.Id);
                if (detail != null)
                {
                    MessageDetail = detail;
                    message.Seen = true;

                    // Refresh the list to update seen status visually
                    var index = Messages.IndexOf(message);
                    if (index >= 0)
                    {
                        Messages.RemoveAt(index);
                        Messages.Insert(index, message);
                        SelectedMessage = message;
                    }
                }
            }
            catch (Exception ex)
            {
                StatusText = $"❌ 加载邮件失败: {ex.Message}";
            }
            finally
            {
                IsLoadingDetail = false;
            }
        }

        [RelayCommand]
        private async Task DeleteMessageAsync(Message? message)
        {
            if (message == null || string.IsNullOrEmpty(CurrentToken)) return;

            try
            {
                await _mailService.DeleteMessageAsync(CurrentToken, message.Id);

                // Find and remove by Id (message may be a different object instance)
                var toRemove = Messages.FirstOrDefault(m => m.Id == message.Id);
                if (toRemove != null)
                {
                    Messages.Remove(toRemove);
                }
                MessageCount = Messages.Count;

                if (MessageDetail?.Id == message.Id)
                {
                    MessageDetail = null;
                    SelectedMessage = null;
                }

                StatusText = $"🗑️ 邮件已删除，剩余 {MessageCount} 封";
            }
            catch (Exception ex)
            {
                StatusText = $"❌ 删除失败: {ex.Message}";
            }
        }

        private async Task PollMessagesAsync()
        {
            if (string.IsNullOrEmpty(CurrentToken)) return;

            try
            {
                var messages = await _mailService.GetMessagesAsync(CurrentToken);
                var newCount = messages.Count - Messages.Count;

                // Update message list
                Messages.Clear();
                foreach (var msg in messages.OrderByDescending(m => m.CreatedAt))
                {
                    Messages.Add(msg);
                }

                MessageCount = Messages.Count;

                if (newCount > 0)
                {
                    StatusText = $"📬 收到 {newCount} 封新邮件！共 {MessageCount} 封";
                }
            }
            catch
            {
                // Silently fail on polling errors
            }
        }

        partial void OnSelectedMessageChanged(Message? value)
        {
            if (value != null)
            {
                _ = ViewMessageAsync(value);
            }
        }

        public void Cleanup()
        {
            _pollTimer.Stop();
        }
    }
}
