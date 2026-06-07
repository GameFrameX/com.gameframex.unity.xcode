<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# GameFrameX Xcode 配置

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.xcode)](https://github.com/GameFrameX/com.gameframex.unity.xcode/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.xcode)](https://github.com/GameFrameX/com.gameframex.unity.xcode/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

獨立遊戲前後端一體化解決方案 · 獨立遊戲開發者的圓夢大使

<br />

[文檔](https://gameframex.doc.alianblank.com) · [快速開始](#quick-start) · QQ群: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | **繁體中文** | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>

## 功能特性

- **Info.plist** — 支援字串、布林、整數、陣列、字典等型別，遞迴寫入
- **系統框架/函式庫** — 自動新增或移除 `.framework` / `.tbd`
- **建構屬性** — 設定、追加、移除 Build Settings（如 `ENABLE_BITCODE`、`GCC_ENABLE_OBJC_EXCEPTIONS`）
- **Capabilities** — 內購、Game Center、推播、Sign In with Apple、背景模式、iCloud、App Groups、Associated Domains、Keychain Sharing、HealthKit、Siri、Personal VPN、Data Protection
- **本地化** — 自動產生 `.lproj/InfoPlist.strings`，支援應用名稱多語言
- **CocoaPods** — 替換 Podfile 預設來源，透過配置注入 pod 依賴，自動執行 `pod install`
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
  "podfile": "",
  "podInstall": true,
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
| `podfile` | string | 自訂 Podfile 檔案路徑，複製到建構輸出目錄（優先級高於 `pods`） |
| `podInstall` | bool | Podfile 處理完畢後自動執行 `pod install`（預設：`true`） |
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
  "runPathSearchPaths": {},
  "pods": {}
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

#### languageCode 支援的值

參考：[Apple Developer - Language and Locale IDs](https://developer.apple.com/library/archive/documentation/MacOSX/Conceptual/BPInternational/LanguageandLocaleIDs/LanguageandLocaleIDs.html)

常用代碼已**加粗**。

| 代碼 | 語言 | 代碼 | 語言 | 代碼 | 語言 | 代碼 | 語言 |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| **en** | **英語** | **zh** | **中文** | **ja** | **日語** | **ko** | **韓語** |
| **es** | **西班牙語** | **fr** | **法語** | **de** | **德語** | **it** | **義大利語** |
| **pt** | **葡萄牙語** | **ru** | **俄語** | **ar** | **阿拉伯語** | **hi** | **印地語** |
| **tr** | **土耳其語** | **vi** | **越南語** | **th** | **泰語** | **id** | **印尼語** |
| aa | 阿法爾語 | ab | 阿布哈茲語 | ae | 阿維斯陀語 | af | 南非荷蘭語 |
| ak | 阿坎語 | am | 阿姆哈拉語 | an | 阿拉貢語 | as | 阿薩姆語 |
| av | 阿瓦爾語 | ay | 艾馬拉語 | az | 亞塞拜然語 | ba | 巴什基爾語 |
| be | 白俄羅斯語 | bg | 保加利亞語 | bh | 比哈爾語 | bi | 比斯拉馬語 |
| bm | 班巴拉語 | bn | 孟加拉語 | bo | 藏語 | br | 布列塔尼語 |
| bs | 波斯尼亞語 | ca | 加泰羅尼亞語 | ce | 車臣語 | ch | 查莫羅語 |
| co | 科西嘉語 | cr | 克里語 | cs | 捷克語 | cu | 教會斯拉夫語 |
| cv | 楚瓦什語 | cy | 威爾斯語 | da | 丹麥語 | dv | 迪維希語 |
| dz | 宗卡語 | ee | 埃維語 | el | 希臘語 | eo | 世界語 |
| et | 愛沙尼亞語 | eu | 巴斯克語 | fa | 波斯語 | ff | 富拉語 |
| fi | 芬蘭語 | fj | 斐濟語 | fo | 法羅語 | fy | 西弗里西亞語 |
| ga | 愛爾蘭語 | gd | 蘇格蘭蓋爾語 | gl | 加利西亞語 | gn | 瓜拉尼語 |
| gu | 古吉拉特語 | gv | 曼島語 | ha | 豪薩語 | he | 希伯來語 |
| ho | 希里莫圖語 | hr | 克羅埃西亞語 | ht | 海地克里奧爾語 | hu | 匈牙利語 |
| hy | 亞美尼亞語 | hz | 赫雷羅語 | ia | 國際語A | ie | 國際語E |
| ig | 伊博語 | ii | 四川彝語 | ik | 依努庇克語 | io | 伊多語 |
| is | 冰島語 | iu | 因紐特語 | jv | 爪哇語 | ka | 喬治亞語 |
| kg | 剛果語 | ki | 吉庫尤語 | kj | 寬亞瑪語 | kk | 哈薩克語 |
| kl | 格陵蘭語 | km | 高棉語 | kn | 卡納達語 | kr | 卡努里語 |
| ks | 克什米爾語 | ku | 庫德語 | kv | 科米語 | kw | 康沃爾語 |
| ky | 吉爾吉斯語 | la | 拉丁語 | lb | 盧森堡語 | lg | 干達語 |
| li | 林堡語 | ln | 林加拉語 | lo | 寮語 | lt | 立陶宛語 |
| lu | 魯巴-加丹加語 | lv | 拉脫維亞語 | mg | 馬爾加什語 | mh | 馬紹爾語 |
| mi | 毛利語 | mk | 馬其頓語 | ml | 馬拉雅拉姆語 | mn | 蒙古語 |
| mr | 馬拉地語 | ms | 馬來語 | mt | 馬爾他語 | my | 緬甸語 |
| na | 瑙魯語 | nb | 挪威博克馬爾語 | nd | 北恩德貝勒語 | ne | 尼泊爾語 |
| ng | 恩東加語 | nl | 荷蘭語 | nn | 挪威尼諾斯克語 | no | 挪威語 |
| nr | 南恩德貝勒語 | nv | 納瓦霍語 | ny | 切瓦語 | oc | 奧克語 |
| oj | 奧吉布瓦語 | om | 奧羅莫語 | or | 奧里亞語 | os | 奧塞梯語 |
| pa | 旁遮普語 | pi | 巴利語 | pl | 波蘭語 | ps | 普什圖語 |
| qu | 克丘亞語 | rm | 羅曼什語 | rn | 隆迪語 | ro | 羅馬尼亞語 |
| rw | 盧安達語 | sa | 梵語 | sc | 薩丁尼亞語 | sd | 信德語 |
| se | 北薩米語 | sg | 桑戈語 | si | 僧伽羅語 | sk | 斯洛伐克語 |
| sl | 斯洛維尼亞語 | sm | 薩摩亞語 | sn | 紹納語 | so | 索馬利亞語 |
| sq | 阿爾巴尼亞語 | sr | 塞爾維亞語 | ss | 斯瓦蒂語 | st | 南索托語 |
| su | 巽他語 | sv | 瑞典語 | sw | 斯瓦希里語 | ta | 坦米爾語 |
| te | 泰盧固語 | tg | 塔吉克語 | ti | 提格雷尼亞語 | tk | 土庫曼語 |
| tl | 他加祿語 | tn | 茨瓦納語 | to | 湯加語 | ts | 聰加語 |
| tt | 韃靼語 | tw | 特威語 | ty | 大溪地語 | ug | 維吾爾語 |
| uk | 烏克蘭語 | ur | 烏爾都語 | uz | 烏茲別克語 | ve | 文達語 |
| vo | 沃拉普克語 | wa | 瓦隆語 | wo | 沃洛夫語 | xh | 科薩語 |
| yi | 意第緒語 | yo | 約魯巴語 | za | 壯語 | zu | 祖魯語 |

> **特殊說明**：
> - **中文**：通常使用 `zh-Hans`（簡體）和 `zh-Hant`（繁體）。
> - **葡萄牙語**：常用 `pt-BR`（巴西）和 `pt-PT`（葡萄牙）。
> - 其他變體可以透過 `代碼-地區` 的方式組合，例如 `en-GB`（英國英語）、`fr-CA`（加拿大法語）。

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

### pods — CocoaPods 依賴庫（在 unityMain / unityFramework 內配置）

`pods` 配置在 `unityMain` 和/或 `unityFramework` 內部。每個 target 的 pods 會注入到對應的 Podfile target 區塊（`target 'Unity-iPhone' do` 或 `target 'UnityFramework' do`）中。自動跳過已存在的同名 pod。

```json
{
  "unityFramework": {
    "pods": {
      "FirebaseAnalytics": "",
      "FBSDKLoginKit": "~> 14.0"
    }
  },
  "unityMain": {
    "pods": {
      "SomePod": "~> 1.0"
    }
  }
}
```

- Key = pod 名稱，Value = 版本約束
- 值為空 → `pod 'Name'`，值非空 → `pod 'Name', 'Value'`

### podfile — 自訂 Podfile

除了透過 `pods` 逐個注入依賴，也可以直接提供一個完整的 Podfile 檔案。設定後優先級高於 `pods` 配置——檔案會被直接複製到建構輸出目錄，然後再套用 `podSource` 中配置的來源位址。

```json
{
  "podfile": "XcodePodfile/Podfile"
}
```

- 支援相對路徑（相對於 Unity 專案根目錄，即 `Assets/` 的上級目錄）和絕對路徑
- 如果檔案不存在，會輸出警告並回退到 `pods` 配置路徑

### podInstall — 自動執行 pod install

控制 Podfile 處理完畢後是否自動執行 `pod install`。需要系統 `PATH` 中有 `pod` 命令列工具。

```json
{
  "podInstall": true
}
```

- 預設為 `true`，當建構輸出中存在 Podfile 時自動執行
- 設為 `false` 可跳過（例如在 CI 中單獨執行 `pod install` 的場景）
- 標準輸出以 info 級別記錄日誌；非零退出碼的 stderr 以 error 級別記錄

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
  "podfile": "",
  "podInstall": true,
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
    "pods": {
      "FirebaseAnalytics": ""
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
- 工具在 `[PostProcessBuild(888)]` 優先級執行，在大多數其他後處理完成後執行
- 資料夾複製時如目標已存在會報錯；檔案複製時如目標已存在會先刪除再複製

## 環境要求

- Unity 2017.1 及以上
- iOS 建構目標
- Xcode（Unity 匯出 iOS 工程時自動依賴）

## 開源協議

[Apache License 2.0](LICENSE.md)
