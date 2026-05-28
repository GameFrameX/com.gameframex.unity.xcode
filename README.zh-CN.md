<div align="center">

![GameFrameX Logo](https://download.alianblank.com/gameframex/gameframex_logo_320.png)

# GameFrameX Xcode 配置

[![Version](https://img.shields.io/github/v/release/gameframex/com.gameframex.unity.xcode?label=version&color=green)](https://github.com/gameframex/com.gameframex.unity.xcode/releases)
[![License](https://img.shields.io/badge/license-Apache%202.0-blue.svg)](LICENSE.md)
[![Documentation](https://img.shields.io/badge/docs-gameframex-brightgreen.svg)](https://gameframex.doc.alianblank.com)

**独立游戏前后端一体化解决方案 · 独立游戏开发者的圆梦大使**

[📖 文档](https://gameframex.doc.alianblank.com) • [🚀 快速开始](#快速开始)

---

🌐 **语言**: [English](README.md) | **简体中文** | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | [한국어](README.ko.md)

---

</div>

Unity iOS 构建后自动配置 Xcode 项目的编辑器工具。通过 JSON 配置文件声明式管理 Info.plist、框架、库、构建属性、Capabilities、CocoaPods 源、本地化等所有 Xcode 设置，无需手动操作 Xcode。

## 功能特性

- **Info.plist** — 支持字符串、布尔、整数、数组、字典等类型，递归写入
- **系统框架/库** — 自动添加或移除 `.framework` / `.tbd`
- **构建属性** — 设置、追加、移除 Build Settings（如 `ENABLE_BITCODE`、`GCC_ENABLE_OBJC_EXCEPTIONS`）
- **Capabilities** — 内购、Game Center、推送、Sign In with Apple、后台模式、iCloud、App Groups、Associated Domains、Keychain Sharing、HealthKit、Siri、Personal VPN、Data Protection
- **本地化** — 自动生成 `.lproj/InfoPlist.strings`，支持应用名多语言
- **CocoaPods** — 替换 Podfile 默认源，通过配置注入 pod 依赖，自动执行 `pod install`
- **XcScheme** — 注入环境变量和启动参数
- **文件/文件夹** — 自动复制到 Xcode 工程并加入编译，识别 `.framework`/`.bundle`
- **编译标志** — 对指定源文件设置编译选项
- **链接器标志** — 配置 `OTHER_LDFLAGS` 等
- **Run Path Search Paths** — 配置运行时搜索路径
- **代码签名** — 配置 Team ID、包名（Bundle Identifier）、签名身份和描述文件
- **Swift 桥接** — 自动创建 Swift 桥接头文件，支持 Objective-C/Swift 混编，CI 环境无弹窗
- **多配置合并** — 支持多个 `XCodeConfig.json` 深度递归合并，适合多模块协作

## 安装

任选以下方式之一：

**方式一：修改 manifest.json**

在 `Packages/manifest.json` 的 `dependencies` 中添加：

```json
"com.gameframex.unity.xcode": "https://github.com/gameframex/com.gameframex.unity.xcode.git"
```

**方式二：Package Manager Git URL**

Unity 编辑器 → Window → Package Manager → Add package from git URL，输入：

```
https://github.com/gameframex/com.gameframex.unity.xcode.git
```

**方式三：手动下载**

克隆或下载本仓库，放入 Unity 项目的 `Packages` 目录即可自动识别。

## 快速开始

1. 将包内 `Editor/XCodeConfigDemo.json` 复制到项目任意目录
2. 重命名为 `XCodeConfig.json`
3. 按需修改配置项（参见下方配置说明）
4. 构建 iOS 项目，工具将自动应用所有配置

## 配置文件结构

配置文件必须命名为 `XCodeConfig.json`，支持放在项目任意位置，支持多个文件并存（会自动合并）。

### 顶层结构

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

| 字段 | 类型 | 说明 |
| :--- | :--- | :--- |
| `swiftBridging` | bool | 启用 Swift 桥接头文件自动生成（默认：`true`） |
| `signing` | object | 代码签名配置（详见下方） |
| `plist` | object | Info.plist 键值对，值支持任意类型 |
| `environmentVariables` | object | XcScheme 环境变量，键值均为字符串 |
| `launcherArgs` | string[] | XcScheme 启动参数列表 |
| `podSource` | string[] | CocoaPods 源地址列表，替换 Podfile 默认源 |
| `podfile` | string | 自定义 Podfile 文件路径，复制到构建输出目录（优先级高于 `pods`） |
| `podInstall` | bool | Podfile 处理完毕后自动执行 `pod install`（默认：`true`） |
| `localizations` | array | 本地化配置（详见下方） |
| `capabilities` | object | iOS 应用能力配置（详见下方） |
| `unityFramework` | object | UnityFramework target 配置 |
| `unityMain` | object | Unity-iPhone target 配置 |

### unityFramework / unityMain

两者结构相同，分别对应 Xcode 项目中的 UnityFramework 和 Unity-iPhone target：

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

#### libs — 系统库

```json
{
  "libs": {
    "+": ["libz.tbd", "libicucore.tbd"],
    "-": ["libstdc++.tbd"]
  }
}
```

- `+` 要添加的库名称列表
- `-` 要移除的库名称列表

#### frameworks — 系统框架

```json
{
  "frameworks": {
    "+": ["WebKit.framework", "UserNotifications.framework"],
    "-": []
  }
}
```

- `+` 要添加的框架名称列表
- `-` 要移除的框架名称列表

#### properties — 构建属性

```json
{
  "properties": {
    "=": { "ENABLE_BITCODE": "NO" },
    "+": { "OTHER_CFLAGS": ["-flag1", "-flag2"] },
    "-": { "UNUSED_FLAG": [""] }
  }
}
```

- `=` 设置属性（键值对，覆盖已有值）
- `+` 追加属性（值为数组时追加到现有列表）
- `-` 移除属性

#### files — 文件复制

```json
{
  "files": {
    "ios_libs.txt": "Classes/ios_libs.txt"
  }
}
```

- Key：文件在 Unity 工程中的路径（与 `Assets` 同级）
- Value：复制到 Xcode 工程的相对路径
- 如目标已存在会先删除再复制

#### folders — 文件夹复制

```json
{
  "folders": {
    "XC": "Classes/XC"
  }
}
```

- Key：文件夹在 Unity 工程中的路径
- Value：复制到 Xcode 工程的相对路径
- 自动识别 `.framework` 和 `.bundle`
- 如目标已存在会报错

#### filesCompileFlag — 文件编译标志

```json
{
  "filesCompileFlag": {
    "Classes/PluginBase/UnityViewControllerListener.mm": "-fobjc-arc"
  }
}
```

- Key：文件在 Xcode 工程中的路径
- Value：要设置的编译标志

#### otherLinkerFlag — 链接器标志

```json
{
  "otherLinkerFlag": {
    "OTHER_LDFLAGS": ["-ObjC"]
  }
}
```

- 值支持字符串和数组两种格式，推荐使用数组格式以确保多配置合并时正确去重合并

#### runPathSearchPaths — 运行时搜索路径

```json
{
  "runPathSearchPaths": {
    "LD_RUNPATH_SEARCH_PATHS": ["@executable_path/Frameworks"]
  }
}
```

- 值支持字符串和数组两种格式，推荐使用数组格式以确保多配置合并时正确去重合并

### signing — 代码签名

仅在 Unity-iPhone（主）target 上生效，所有字段均可选。

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

| 字段 | 类型 | 说明 |
| :--- | :--- | :--- |
| `teamId` | string | Apple 开发者团队 ID（`DEVELOPMENT_TEAM`） |
| `bundleId` | string | 应用包名（`PRODUCT_BUNDLE_IDENTIFIER`） |
| `codeSignIdentity` | string | 签名身份，可选值：`Apple Development`、`Apple Distribution`、`iPhone Developer`、`iPhone Distribution` |
| `codeSignStyle` | string | 签名方式：`Automatic`（自动）或 `Manual`（手动） |
| `provisioningProfileSpecifier` | string | 描述文件名称（仅 Manual 模式需要） |

### swiftBridging — Swift 桥接

仅在 Unity-iPhone（主）target 上生效。开启后（默认开启）自动创建 Swift 文件和桥接头文件，实现 Objective-C/Swift 混编。无 Xcode 弹窗提示，适合 CI 自动化构建。

```json
{
  "swiftBridging": true
}
```

- 默认为 `true`，设为 `false` 可关闭
- 自动创建 `gameframex_swift_bridging.swift` 和 `Unity-iPhone-Bridging-Header.h`
- 设置 `SWIFT_VERSION` 为 `5.0` 并配置 `SWIFT_OBJC_BRIDGING_HEADER`

### capabilities — 应用能力

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

| 字段 | 类型 | 说明 |
| :--- | :--- | :--- |
| `inAppPurchase` | bool | 内购 |
| `gameCenter` | bool | Game Center |
| `pushNotifications` | bool | 推送通知 |
| `signInWithApple` | bool | Sign In with Apple |
| `backgroundModes` | string[] | 后台模式，可选值：`audio`、`location`、`voip`、`newsstand`、`external`、`bluetooth`、`bluetooth-peripheral`、`fetch`、`remote-notification` |
| `iCloud.keyValueStorage` | bool | iCloud 键值存储 |
| `iCloud.iCloudDocument` | bool | iCloud 文档存储 |
| `iCloud.customContainers` | string[] | iCloud 自定义容器 |
| `appGroups` | string[] | App Groups 标识符 |
| `associatedDomains` | string[] | 关联域名（Universal Links） |
| `keychainSharing` | bool 或 object | Keychain Sharing。`false` 禁用，`true` 使用默认分组，或 `{"accessGroups": ["group1"]}` 指定自定义分组 |
| `healthKit` | bool | HealthKit 健康数据访问 |
| `siri` | bool | Siri 语音助手集成 |
| `personalVPN` | bool | Personal VPN 个人 VPN |
| `dataProtection` | bool | Data Protection 文件级加密 |

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

- `languageCode` — ISO 639-1 语言代码（中文使用 `zh-Hans` 简体 / `zh-Hant` 繁体）
- `validMap` — 键值对列表，每个项包含 `key` 和 `value`
- 会自动生成 `.lproj/InfoPlist.strings` 文件并添加到工程

### plist — Info.plist 配置

支持任意层级嵌套，常见配置：

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
    "NSCameraUsageDescription": "需要相机权限用于扫码",
    "ITSAppUsesNonExemptEncryption": false
  }
}
```

### pods — CocoaPods 依赖库（在 unityMain / unityFramework 内配置）

`pods` 配置在 `unityMain` 和/或 `unityFramework` 内部。每个 target 的 pods 会注入到对应的 Podfile target 块（`target 'Unity-iPhone' do` 或 `target 'UnityFramework' do`）中。自动跳过已存在的同名 pod。

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

- Key = pod 名称，Value = 版本约束
- 值为空 → `pod 'Name'`，值非空 → `pod 'Name', 'Value'`

### podfile — 自定义 Podfile

除了通过 `pods` 逐个注入依赖，也可以直接提供一个完整的 Podfile 文件。设置后优先级高于 `pods` 配置——文件会被直接复制到构建输出目录，然后再应用 `podSource` 中配置的源地址。

```json
{
  "podfile": "XcodePodfile/Podfile"
}
```

- 支持相对路径（相对于 Unity 工程根目录，即 `Assets/` 的上级目录）和绝对路径
- 如果文件不存在，会输出警告并回退到 `pods` 配置路径

### podInstall — 自动执行 pod install

控制 Podfile 处理完毕后是否自动执行 `pod install`。需要系统 `PATH` 中有 `pod` 命令行工具。

```json
{
  "podInstall": true
}
```

- 默认为 `true`，当构建输出中存在 Podfile 时自动执行
- 设为 `false` 可跳过（例如在 CI 中单独执行 `pod install` 的场景）
- 标准输出以 info 级别记录日志；非零退出码的 stderr 以 error 级别记录

## 多配置合并

项目中可以放置多个 `XCodeConfig.json` 文件（如不同模块各自维护一份），构建时会自动发现并深度合并：

- **对象**：递归合并（子键逐层合并）
- **数组**：去重合并（union）
- **标量**：后者覆盖前者

这使得多 SDK / 多模块的 Xcode 配置可以独立管理、互不干扰。

## 完整示例

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
    "NSCameraUsageDescription": "需要您的相机权限",
    "NSMicrophoneUsageDescription": "需要您的麦克风权限",
    "NSPhotoLibraryUsageDescription": "需要您的相册权限",
    "ITSAppUsesNonExemptEncryption": false,
    "NSUserTrackingUsageDescription": "此标识符将用于向您推荐个性化广告"
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
      "=": { "ENABLE_BITCODE": "NO" },
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

## 注意事项

- 配置文件名称必须为 `XCodeConfig.json`，否则不会被识别
- 所有代码在 `#if UNITY_IOS` 条件编译下，不会影响其他平台
- 工具在 `[PostProcessBuild(888)]` 优先级运行，在大多数其他后处理完成后执行
- 文件夹复制时如目标已存在会报错；文件复制时如目标已存在会先删除再复制

## 环境要求

- Unity 2017.1 及以上
- iOS 构建目标
- Xcode（Unity 导出 iOS 工程时自动依赖）

## 开源协议

[Apache License 2.0](LICENSE.md)
