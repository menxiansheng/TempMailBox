using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TempMailBox.Models;

namespace TempMailBox.Services
{
    public class MailTmService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://api.mail.tm";

        public MailTmService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private async Task EnsureSuccessAsync(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"HTTP {(int)response.StatusCode}: {errorBody}");
            }
        }

        public async Task<List<Domain>> GetDomainsAsync()
        {
            var response = await _httpClient.GetAsync("/domains");
            await EnsureSuccessAsync(response);
            var json = await response.Content.ReadAsStringAsync();

            // API may return plain array or hydra:member wrapper
            try
            {
                var list = JsonSerializer.Deserialize<List<Domain>>(json);
                if (list != null) return list;
            }
            catch { }

            var result = JsonSerializer.Deserialize<DomainListResponse>(json);
            return result?.Members ?? new List<Domain>();
        }

        public async Task<AccountResponse?> CreateAccountAsync(string address, string password)
        {
            var request = new AccountRequest { Address = address, Password = password };
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/accounts", content);
            await EnsureSuccessAsync(response);
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AccountResponse>(responseJson);
        }

        public async Task<string?> GetTokenAsync(string address, string password)
        {
            var request = new TokenRequest { Address = address, Password = password };
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/token", content);
            await EnsureSuccessAsync(response);
            var responseJson = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseJson);
            return tokenResponse?.Token;
        }

        public async Task<List<Message>> GetMessagesAsync(string token)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "/messages");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.SendAsync(request);
            await EnsureSuccessAsync(response);
            var json = await response.Content.ReadAsStringAsync();

            // API may return plain array or hydra:member wrapper
            try
            {
                var list = JsonSerializer.Deserialize<List<Message>>(json);
                if (list != null) return list;
            }
            catch { }

            var result = JsonSerializer.Deserialize<MessageListResponse>(json);
            return result?.Members ?? new List<Message>();
        }

        public async Task<Message?> GetMessageAsync(string token, string messageId)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"/messages/{messageId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.SendAsync(request);
            await EnsureSuccessAsync(response);
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Message>(json);
        }

        public async Task DeleteMessageAsync(string token, string messageId)
        {
            using var request = new HttpRequestMessage(HttpMethod.Delete, $"/messages/{messageId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.SendAsync(request);
            await EnsureSuccessAsync(response);
        }

        public async Task DeleteAccountAsync(string token, string accountId)
        {
            using var request = new HttpRequestMessage(HttpMethod.Delete, $"/accounts/{accountId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.SendAsync(request);
            // Ignore errors on account deletion
        }
    }
}
