# TempMailBox 臨時郵箱助手 📬

**TempMailBox** 是一個基於 **.NET 6 (WPF)** 與 **MVVM 架構** 開發的現代化 Windows 桌面版臨時郵箱客戶端。透過整合 [Mail.tm](https://mail.tm/) 的免費 API 服務，讓您無需註冊即可快速生成一次性臨時電郵，避免個人主郵箱收到垃圾郵件或被追蹤。

---

## ✨ 主要功能

- ⚡ **一鍵生成臨時郵箱**：自動獲取可用域名並生成隨機安全的臨時電子郵件帳號。
- 📋 **快速複製地址**：提供一鍵複製按鈕，方便快速貼上至需要驗證的網站或服務。
- 🔄 **自動與手動刷新**：
  - **自動輪詢**：背景每 10 秒自動檢測並接收新郵件。
  - **手動刷新**：隨時點擊刷新按鈕即時同步最新郵件。
- 📖 **郵件詳情查看**：
  - 完整顯示寄件者、收件者、主旨與發送時間。
  - 支援查看內文詳情，自動標記已讀狀態。
- 🗑️ **郵件管理**：可單獨刪除不需要的郵件。
- 📜 **生成歷史紀錄**：自動保存本次運行中生成的臨時郵箱歷史，方便回溯。
- 🎨 **現代化 GUI 介面**：清晰直覺的深/淺色視覺體驗，搭配狀態提示列與載入動畫。

---

## 🚀 使用指南 (Usage Guide)

### 1. 生成臨時郵箱
1. 啟動應用程式後，點擊頂部的 **「生成郵箱」**（或「新郵箱」）按鈕。
2. 程式將自動向 Mail.tm 請求可用域名，並隨機創建一個臨時帳號。
3. 生成成功後，頂部文字框會顯示您的臨時郵箱地址（如 `user123456a@domain.com`）。

### 2. 複製與使用郵箱
1. 點擊郵箱地址旁邊的 **「複製」** 按鈕。
2. 將複製的郵箱地址貼到您需要進行註冊或驗證的網站。

### 3. 接收與閱讀郵件
1. 當目標網站發送驗證碼或郵件後，TempMailBox 每 10 秒會自動檢測新郵件。
2. 收到郵件時，左側郵件列表會即時更新並顯示郵件數量提示。
3. 點擊列表中的任意郵件，右側預覽區域將載入並顯示郵件的完整內容（寄件者、主題、時間、內文）。

### 4. 刷新與刪除
- **刷新列表**：若想立刻檢查郵件，可點擊 **「刷新」** 按鈕。
- **刪除郵件**：選中郵件後，點擊 **「刪除」** 按鈕即可將該封郵件從伺服器與列表中移除。

---

## 🛠️ 開發環境與建置 (Development & Build)

### 系統需求
- **作業系統**：Windows 10 / Windows 11
- **開發環境**：Visual Studio 2022 (包含 .NET 桌面開發工作負載) 或 Visual Studio Code
- **運行時**：[.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) 或更高版本

### 複製專案與運行

1. **Clone 專案**
   ```bash
   git clone https://github.com/menxiansheng/TempMailBox.git
   cd TempMailBox
   ```

2. **還原套件與編譯**
   ```bash
   dotnet restore
   dotnet build
   ```

3. **運行應用程式**
   ```bash
   dotnet run
   ```

4. **發布獨立執行檔 (Publish)**
   ```bash
   dotnet publish -c Release -r win-x64 --self-contained false
   ```

---

## 📂 專案架構 (Project Structure)

```text
TempMailBox/
├── Models/                # 資料模型 (Account, Domain, Message, TokenResponse)
├── Services/              # API 服務層 (MailTmService.cs)
├── ViewModels/            # MVVM ViewModel 邏輯層 (MainViewModel.cs)
├── Converters/            # WPF UI 轉換器 (BooleanConverters.cs)
├── App.xaml               # 應用程式資源與樣式
├── MainWindow.xaml        # 主視窗 UI 介面
├── MainWindow.xaml.cs     # 主視窗 Code-behind
├── TempMailBox.csproj     # .NET 專案檔
├── README.md              # 專案說明文件
└── LICENSE                # MIT 開源授權條款
```

---

## 🌐 API 服務說明

本專案使用 [Mail.tm](https://mail.tm/) 提供的免費 RESTful API 服務。主要端點包括：
- `GET /domains`：獲取可用臨時郵箱域名
- `POST /accounts`：創建臨時帳號
- `POST /token`：取得 JWT 身份驗證 Token
- `GET /messages`：獲取郵件清單
- `GET /messages/{id}`：獲取指定郵件詳情
- `DELETE /messages/{id}`：刪除指定郵件

---

## 📄 授權條款 (License)

本專案採用 **[MIT License](LICENSE)** 授權。您可以自由修改、散布與個人或商業使用。
