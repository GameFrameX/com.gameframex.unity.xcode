<div align="center">

![GameFrameX Logo](https://download.alianblank.com/gameframex/gameframex_logo_320.png)

# GameFrameX Xcode 配置

[![Version](https://img.shields.io/github/v/release/gameframex/com.gameframex.unity.xcode?label=version&color=green)](https://github.com/gameframex/com.gameframex.unity.xcode/releases)
[![License](https://img.shields.io/badge/license-Apache%202.0-blue.svg)](LICENSE.md)
[![Documentation](https://img.shields.io/badge/docs-gameframex-brightgreen.svg)](https://gameframex.doc.alianblank.com)

**獨立遊戲前後端一體化解決方案 · 獨立遊戲開發者的圓夢大使**

[📖 文檔](https://gameframex.doc.alianblank.com) • [🚀 快速開始](#快速開始)

---

🌐 **語言**: [English](README.md) | [简体中文](README.zh-CN.md) | **繁體中文** | [日本語](README.ja.md) | [한국어](README.ko.md)

---

</div>

Unity iOS 建構後自動配置 Xcode 專案的編輯器工具。透過 JSON 配置檔案宣告式管理 Info.plist、框架、函式庫、建構屬性、Capabilities、CocoaPods 來源、本地化等所有 Xcode 設定，無需手動操作 Xcode。

## 功能特性

- **Info.plist** — 支援字串、布林、整數、陣列、字典等型別，遞迴寫入
- **系統框架/函式庫** — 自動新增或移除 `.framework` / `.tbd`
- **建構屬性** — 設定、追加、移除 Build Settings（如 `ENABLE_BITCODE`、`GCC_ENABLE_OBJC_EXCEPTIONS`）
- **Capabilities** — 內購、Game Center、推播、Sign In with Apple、背景模式、iCloud、App Groups、Associated Domains、Keychain Sharing、HealthKit、Siri、Personal VPN、Data Protection
- **本地化** — 自動產生 `.lproj/InfoPlist.strings`，支援應用名稱多語言
- **CocoaPods** — 替換 Podfile 預設來源，透過配置注入 pod 依賴
- **XcScheme** — 注入環境變數和啟動參數
- **檔案/資料夾** — 自動複製到 Xcode 工程並加入編譯，識別 `.framework`/`.bundle`
- **編譯標誌** — 對指定原始碼檔案設定編譯選項
- **連結器標誌** — 配置 `OTHER_LDFLAGS` 等
- **Run Path Search Paths** — 配置執行時搜尋路徑
- **程式碼簽名** — 配置 Team ID、包名（Bundle Identifier）、簽名身份和描述檔
- **Swift 橋接** — 自動建立 Swift 橋接標頭檔案，支援 Objective-C/Swift 混編，CI 環境無彈窗
- **多配置合併** — 支援多個 `XCodeConfig.json` 深度遞迴合併，適合多模組協作

## 安裝

任選以下方式之一：

**方式一：修改 manifest.json**

在 `Packages/manifest.json` 的 `dependencies` 中新增：

```json
"com.gameframex.unity.xcode": "https://github.com/gameframex/com.gameframex.unity.xcode.git"
```

**方式二：Package Manager Git URL**

Unity 編輯器 → Window → Package Manager → Add package from git URL，輸入：

```
https://github.com/gameframex/com.gameframex.unity.xcode.git
```

**方式三：手動下載**

克隆或下載本倉庫，放入 Unity 專案的 `Packages` 目錄即可自動識別。

## 快速開始

1. 將包內 `Editor/XCodeConfigDemo.json` 複製到專案任意目錄
2. 重新命名為 `XCodeConfig.json`
3. 按需修改配置項（參見下方配置說明）
4. 建構 iOS 專案，工具將自動套用所有配置

## 配置檔案結構

配置檔案必須命名為 `XCodeConfig.json`，支援放在專案任意位置，支援多個檔案並存（會自動合併）。

### 頂層結構

```json
{
  "swiftBridging": true,
  "signing": {},
  "plist": {},
  "environmentVariables": {},
  "launcherArgs": [],
  "podSource": [],
  "pods": {},
  "localizations": [],
  "capabilities": {},
  "unityFramework": {},
  "unityMain": {}
}
```

| 欄位 | 型別 | 說明 |
| :--- | :--- | :--- |
| `swiftBridging` | bool | 啟用 Swift 橋接標頭檔案自動產生（預設：`true`） |
| `signing` | object | 程式碼簽名配置（詳見下方） |
| `plist` | object | Info.plist 鍵值對，值支援任意型別 |
| `environmentVariables` | object | XcScheme 環境變數，鍵值均為字串 |
| `launcherArgs` | string[] | XcScheme 啟動參數列表 |
| `podSource` | string[] | CocoaPods 來源位址列表，替換 Podfile 預設來源 |
| `pods` | object | CocoaPods 依賴庫，key 為 pod 名稱，value 為版本約束（詳見下方） |
| `localizations` | array | 本地化配置（詳見下方） |
| `capabilities` | object | iOS 應用能力配置（詳見下方） |
| `unityFramework` | object | UnityFramework target 配置 |
| `unityMain` | object | Unity-iPhone target 配置 |

### unityFramework / unityMain

兩者結構相同，分別對應 Xcode 專案中的 UnityFramework 和 Unity-iPhone target：

```json
{
  "libs": { "+": [], "-": [] },
  "frameworks": { "+": [], "-": [] },
  "properties": { "=": {}, "+": {}, "-": {} },
  "files": {},
  "folders": {},
  "filesCompileFlag": {},
  "otherLinkerFlag": {},
  "runPathSearchPaths": {}
}
```

#### libs — 系統函式庫

```json
{
  "libs": {
    "+": ["libz.tbd", "libicucore.tbd"],
    "-": ["libstdc++.tbd"]
  }
}
```

- `+` 要新增的函式庫名稱列表
- `-` 要移除的函式庫名稱列表

#### frameworks — 系統框架

```json
{
  "frameworks": {
    "+": ["WebKit.framework", "UserNotifications.framework"],
    "-": []
  }
}
```

- `+` 要新增的框架名稱列表
- `-` 要移除的框架名稱列表

#### properties — 建構屬性

```json
{
  "properties": {
    "=": { "ENABLE_BITCODE": "NO" },
    "+": { "OTHER_CFLAGS": ["-flag1", "-flag2"] },
    "-": { "UNUSED_FLAG": [""] }
  }
}
```

- `=` 設定屬性（鍵值對，覆蓋已有值）
- `+` 追加屬性（值為陣列時追加到現有列表）
- `-` 移除屬性

#### files — 檔案複製

```json
{
  "files": {
    "ios_libs.txt": "Classes/ios_libs.txt"
  }
}
```

- Key：檔案在 Unity 工程中的路徑（與 `Assets` 同級）
- Value：複製到 Xcode 工程的相對路徑
- 如目標已存在會先刪除再複製

#### folders — 資料夾複製

```json
{
  "folders": {
    "XC": "Classes/XC"
  }
}
```

- Key：資料夾在 Unity 工程中的路徑
- Value：複製到 Xcode 工程的相對路徑
- 自動識別 `.framework` 和 `.bundle`
- 如目標已存在會報錯

#### filesCompileFlag — 檔案編譯標誌

```json
{
  "filesCompileFlag": {
    "Classes/PluginBase/UnityViewControllerListener.mm": "-fobjc-arc"
  }
}
```

- Key：檔案在 Xcode 工程中的路徑
- Value：要設定的編譯標誌

#### otherLinkerFlag — 連結器標誌

```json
{
  "otherLinkerFlag": {
    "OTHER_LDFLAGS": ["-ObjC"]
  }
}
```

- 值支援字串和陣列兩種格式，推薦使用陣列格式以確保多配置合併時正確去重合併

#### runPathSearchPaths — 執行時搜尋路徑

```json
{
  "runPathSearchPaths": {
    "LD_RUNPATH_SEARCH_PATHS": ["@executable_path/Frameworks"]
  }
}
```

- 值支援字串和陣列兩種格式，推薦使用陣列格式以確保多配置合併時正確去重合併

### signing — 程式碼簽名

僅在 Unity-iPhone（主）target 上生效，所有欄位均可選。

```json
{
  "signing": {
    "teamId": "XXXXXXXXXX",
    "bundleId": "com.company.app",
    "codeSignIdentity": "Apple Development",
    "codeSignStyle": "Automatic",
    "provisioningProfileSpecifier": ""
  }
}
```

| 欄位 | 型別 | 說明 |
| :--- | :--- | :--- |
| `teamId` | string | Apple 開發者團隊 ID（`DEVELOPMENT_TEAM`） |
| `bundleId` | string | 應用包名（`PRODUCT_BUNDLE_IDENTIFIER`） |
| `codeSignIdentity` | string | 簽名身份，可選值：`Apple Development`、`Apple Distribution`、`iPhone Developer`、`iPhone Distribution` |
| `codeSignStyle` | string | 簽名方式：`Automatic`（自動）或 `Manual`（手動） |
| `provisioningProfileSpecifier` | string | 描述檔名稱（僅 Manual 模式需要） |

### swiftBridging — Swift 橋接

僅在 Unity-iPhone（主）target 上生效。啟用後（預設啟用）自動建立 Swift 檔案和橋接標頭檔案，實現 Objective-C/Swift 混編。無 Xcode 彈窗提示，適合 CI 自動化建構。

```json
{
  "swiftBridging": true
}
```

- 預設為 `true`，設為 `false` 可關閉
- 自動建立 `gameframex_swift_bridging.swift` 和 `Unity-iPhone-Bridging-Header.h`
- 設定 `SWIFT_VERSION` 為 `5.0` 並配置 `SWIFT_OBJC_BRIDGING_HEADER`

### capabilities — 應用能力

```json
{
  "capabilities": {
    "inAppPurchase": true,
    "gameCenter": false,
    "pushNotifications": false,
    "signInWithApple": false,
    "backgroundModes": ["audio", "remote-notification"],
    "iCloud": {
      "keyValueStorage": false,
      "iCloudDocument": false,
      "customContainers": []
    },
    "appGroups": [],
    "associatedDomains": [],
    "keychainSharing": false,
    "healthKit": false,
    "siri": false,
    "personalVPN": false,
    "dataProtection": false
  }
}
```

| 欄位 | 型別 | 說明 |
| :--- | :--- | :--- |
| `inAppPurchase` | bool | 內購 |
| `gameCenter` | bool | Game Center |
| `pushNotifications` | bool | 推播通知 |
| `signInWithApple` | bool | Sign In with Apple |
| `backgroundModes` | string[] | 背景模式，可選值：`audio`、`location`、`voip`、`newsstand`、`external`、`bluetooth`、`bluetooth-peripheral`、`fetch`、`remote-notification` |
| `iCloud.keyValueStorage` | bool | iCloud 鍵值儲存 |
| `iCloud.iCloudDocument` | bool | iCloud 文件儲存 |
| `iCloud.customContainers` | string[] | iCloud 自訂容器 |
| `appGroups` | string[] | App Groups 識別符 |
| `associatedDomains` | string[] | 關聯網域（Universal Links） |
| `keychainSharing` | bool 或 object | Keychain Sharing。`false` 停用，`true` 使用預設分組，或 `{"accessGroups": ["group1"]}` 指定自訂分組 |
| `healthKit` | bool | HealthKit 健康資料存取 |
| `siri` | bool | Siri 語音助理整合 |
| `personalVPN` | bool | Personal VPN 個人 VPN |
| `dataProtection` | bool | Data Protection 檔案層級加密 |

### localizations — 本地化

```json
{
  "localizations": [
    {
      "languageCode": "en",
      "validMap": [
        { "key": "CFBundleDisplayName", "value": "My Game" }
      ]
    },
    {
      "languageCode": "zh-Hans",
      "validMap": [
        { "key": "CFBundleDisplayName", "value": "我的游戏" }
      ]
    }
  ]
}
```

- `languageCode` — ISO 639-1 語言代碼（中文使用 `zh-Hans` 簡體 / `zh-Hant` 繁體）
- `validMap` — 鍵值對列表，每個項包含 `key` 和 `value`
- 會自動產生 `.lproj/InfoPlist.strings` 檔案並新增到工程

### plist — Info.plist 配置

支援任意層級巢狀，常見配置：

```json
{
  "plist": {
    "CFBundleURLTypes": [
      {
        "CFBundleTypeRole": "Editor",
        "CFBundleURLSchemes": ["myapp"],
        "CFBundleURLName": "com.example.myapp"
      }
    ],
    "NSAppTransportSecurity": {
      "NSAllowsArbitraryLoads": true
    },
    "NSCameraUsageDescription": "需要相機權限用於掃碼",
    "ITSAppUsesNonExemptEncryption": false
  }
}
```

### pods — CocoaPods 依賴庫

向 Podfile 的 `target 'Unity-iPhone' do` 區塊注入 `pod` 宣告，自動跳過已存在的同名 pod。

```json
{
  "pods": {
    "FirebaseAnalytics": "",
    "FBSDKLoginKit": "~> 14.0"
  }
}
```

- Key = pod 名稱，Value = 版本約束
- 值為空 → `pod 'Name'`，值非空 → `pod 'Name', 'Value'`
- **不會**自動執行 `pod install`，需手動或在 CI 中執行

## 多配置合併

專案中可以放置多個 `XCodeConfig.json` 檔案（如不同模組各自維護一份），建構時會自動發現並深度合併：

- **物件**：遞迴合併（子鍵逐層合併）
- **陣列**：去重合併（union）
- **純量**：後者覆蓋前者

這使得多 SDK / 多模組的 Xcode 配置可以獨立管理、互不干擾。

## 完整範例

```json
{
  "swiftBridging": true,
  "signing": {
    "teamId": "XXXXXXXXXX",
    "bundleId": "com.company.app",
    "codeSignIdentity": "Apple Development",
    "codeSignStyle": "Automatic",
    "provisioningProfileSpecifier": ""
  },
  "plist": {
    "CFBundleURLTypes": [
      {
        "CFBundleTypeRole": "Editor",
        "CFBundleURLSchemes": ["bbqgame"],
        "CFBundleURLName": "com.smartdogx.bbq"
      },
      {
        "CFBundleTypeRole": "Editor",
        "CFBundleURLSchemes": ["wx5dfe430e96b395a6"]
      }
    ],
    "LSApplicationQueriesSchemes": [
      "weixin", "wechat", "mqqapi"
    ],
    "NSAppTransportSecurity": {
      "NSAllowsArbitraryLoads": true,
      "NSExceptionDomains": {
        "qq.com": {
          "NSIncludesSubdomains": true,
          "NSThirdPartyExceptionAllowsInsecureHTTPLoads": true,
          "NSThirdPartyExceptionRequiresForwardSecrecy": false
        }
      }
    },
    "NSCameraUsageDescription": "需要您的相機權限",
    "NSMicrophoneUsageDescription": "需要您的麥克風權限",
    "NSPhotoLibraryUsageDescription": "需要您的相簿權限",
    "ITSAppUsesNonExemptEncryption": false,
    "NSUserTrackingUsageDescription": "此識別符將用於向您推薦個人化廣告"
  },
  "environmentVariables": {
    "IDEPreferLogStreaming": "YES",
    "OS_ACTIVITY_MODE": "disable"
  },
  "launcherArgs": ["-debug"],
  "localizations": [
    {
      "languageCode": "en",
      "validMap": [
        { "key": "CFBundleDisplayName", "value": "My Game" }
      ]
    },
    {
      "languageCode": "zh-Hans",
      "validMap": [
        { "key": "CFBundleDisplayName", "value": "我的游戏" }
      ]
    }
  ],
  "podSource": [
    "https://mirrors.tuna.tsinghua.edu.cn/git/CocoaPods/Specs.git"
  ],
  "pods": {},
  "capabilities": {
    "inAppPurchase": true,
    "gameCenter": false,
    "pushNotifications": false,
    "signInWithApple": false,
    "backgroundModes": [],
    "iCloud": {
      "keyValueStorage": false,
      "iCloudDocument": false,
      "customContainers": []
    },
    "appGroups": [],
    "associatedDomains": [],
    "keychainSharing": false,
    "healthKit": false,
    "siri": false,
    "personalVPN": false,
    "dataProtection": false
  },
  "unityFramework": {
    "libs": {
      "+": ["libicucore.tbd", "libz.tbd"],
      "-": []
    },
    "frameworks": {
      "+": ["WebKit.framework", "Security.framework"],
      "-": []
    },
    "properties": {
      "=": {
        "ENABLE_BITCODE": "NO",
        "GCC_ENABLE_OBJC_EXCEPTIONS": true,
        "CLANG_ENABLE_OBJC_ARC": true
      },
      "+": {},
      "-": {}
    },
    "filesCompileFlag": {},
    "otherLinkerFlag": {
      "OTHER_LDFLAGS": ["-ObjC"]
    },
    "files": {},
    "folders": {}
  },
  "unityMain": {
    "libs": {
      "+": ["libz.tbd"],
      "-": []
    },
    "frameworks": {
      "+": ["WebKit.framework"],
      "-": []
    },
    "properties": {
      "="": { "ENABLE_BITCODE": "NO" },
      "+": {},
      "-": {}
    },
    "otherLinkerFlag": {
      "OTHER_LDFLAGS": ["-ObjC"]
    },
    "files": {},
    "folders": {}
  }
}
```

## 注意事項

- 配置檔案名稱必須為 `XCodeConfig.json`，否則不會被識別
- 所有程式碼在 `#if UNITY_IOS` 條件編譯下，不會影響其他平台
- 工具在 `[PostProcessBuild(ushort.MaxValue)]` 優先級執行，即所有其他後處理完成後執行
- 資料夾複製時如目標已存在會報錯；檔案複製時如目標已存在會先刪除再複製

## 環境要求

- Unity 2017.1 及以上
- iOS 建構目標
- Xcode（Unity 匯出 iOS 工程時自動依賴）

## 開源協議

[Apache License 2.0](LICENSE.md)
