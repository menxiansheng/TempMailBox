# TempMailBox 临时邮箱助手 📬

**TempMailBox** 是一个基于 **.NET 6 (WPF)** 与 **MVVM 架构** 开发的现代化 Windows 桌面端临时邮箱客户端。通过整合 [Mail.tm](https://mail.tm/) 的免费 API 服务，让您无需注册即可快速生成一次性临时电子邮件账号，避免个人主邮箱收到垃圾邮件或被追踪。

---

## ✨ 主要功能

- ⚡ **一键生成临时邮箱**：自动获取可用域名并生成随机安全的临时电子邮件账号。
- 📋 **快速复制地址**：提供一键复制按钮，方便快速粘贴至需要验证的网站或服务。
- 🔄 **自动与手动刷新**：
  - **自动轮询**：后台每 10 秒自动检测并接收新邮件。
  - **手动刷新**：随时点击刷新按钮实时同步最新邮件。
- 📖 **邮件详情查看**：
  - 完整显示发件人、收件人、主题与发送时间。
  - 支持查看正文详情，自动标记已读状态。
- 🗑️ **邮件管理**：可单独删除不需要的邮件。
- 📜 **生成历史纪录**：自动保存本次运行中生成的临时邮箱历史，方便回溯。
- 🎨 **现代化 GUI 界面**：清晰直观的深/浅色视觉体验，搭配状态提示栏与加载动画。

---

## 🚀 使用指南 (Usage Guide)

### 1. 生成临时邮箱
1. 启动应用程序后，点击顶部的 **“生成邮箱”**（或“新邮箱”）按钮。
2. 程序将自动向 Mail.tm 请求可用域名，并随机创建一个临时账号。
3. 生成成功后，顶部文本框会显示您的临时邮箱地址（如 `user123456a@domain.com`）。

### 2. 复制与使用邮箱
1. 点击邮箱地址旁边的 **“复制”** 按钮。
2. 将复制的邮箱地址粘贴到您需要进行注册或验证的网站。

### 3. 接收与阅读邮件
1. 当目标网站发送验证码或邮件后，TempMailBox 每 10 秒会自动检测新邮件。
2. 收到邮件时，左侧邮件列表会实时更新并显示邮件数量提示。
3. 点击列表中的任意邮件，右侧预览区域将加载并显示邮件的完整内容（发件人、主题、时间、正文）。

### 4. 刷新与删除
- **刷新列表**：若想立刻检查邮件，可点击 **“刷新”** 按钮。
- **删除邮件**：选中邮件后，点击 **“删除”** 按钮即可将该封邮件从服务器与列表中移除。

---

## 🛠️ 开发环境与构建 (Development & Build)

### 系统需求
- **操作系统**：Windows 10 / Windows 11
- **开发环境**：Visual Studio 2022 (包含 .NET 桌面开发工作负载) 或 Visual Studio Code
- **运行时**：[.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) 或更高版本

### 克隆项目与运行

1. **Clone 项目**
   ```bash
   git clone https://github.com/menxiansheng/TempMailBox.git
   cd TempMailBox
   ```

2. **还原包与编译**
   ```bash
   dotnet restore
   dotnet build
   ```

3. **运行应用程序**
   ```bash
   dotnet run
   ```

4. **发布独立可执行文件 (Publish)**
   ```bash
   dotnet publish -c Release -r win-x64 --self-contained false
   ```

---

## 📂 项目结构 (Project Structure)

```text
TempMailBox/
├── Models/                # 数据模型 (Account, Domain, Message, TokenResponse)
├── Services/              # API 服务层 (MailTmService.cs)
├── ViewModels/            # MVVM ViewModel 逻辑层 (MainViewModel.cs)
├── Converters/            # WPF UI 转换器 (BooleanConverters.cs)
├── App.xaml               # 应用程序资源与样式
├── MainWindow.xaml        # 主窗口 UI 界面
├── MainWindow.xaml.cs     # 主窗口 Code-behind
├── TempMailBox.csproj     # .NET 项目文件
├── README.md              # 项目说明文件
└── LICENSE                # MIT 开源授权条款
```

---

## 🌐 API 服务说明

本项目使用 [Mail.tm](https://mail.tm/) 提供的免费 RESTful API 服务。主要端点包括：
- `GET /domains`：获取可用临时邮箱域名
- `POST /accounts`：创建临时账号
- `POST /token`：取得 JWT 身份验证 Token
- `GET /messages`：获取邮件清单
- `GET /messages/{id}`：获取指定邮件详情
- `DELETE /messages/{id}`：删除指定邮件

---

## 📄 授权条款 (License)

本项目采用 **[MIT License](LICENSE)** 授权。您可以自由修改、分发与个人或商业使用。
