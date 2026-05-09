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
- **Capabilities** — 内购、Game Center、推送、Sign In with Apple、后台模式、iCloud、App Groups、Associated Domains
- **本地化** — 自动生成 `.lproj/InfoPlist.strings`，支持应用名多语言
- **CocoaPods** — 替换 Podfile 默认源，支持配置多个镜像源
- **XcScheme** — 注入环境变量和启动参数
- **文件/文件夹** — 自动复制到 Xcode 工程并加入编译，识别 `.framework`/`.bundle`
- **编译标志** — 对指定源文件设置编译选项
- **链接器标志** — 配置 `OTHER_LDFLAGS` 等
- **Run Path Search Paths** — 配置运行时搜索路径
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
  "plist": {},
  "environmentVariables": {},
  "launcherArgs": [],
  "podSource": [],
  "localizations": [],
  "capabilities": {},
  "unityFramework": {},
  "unityMain": {}
}
```

| 字段 | 类型 | 说明 |
| :--- | :--- | :--- |
| `plist` | object | Info.plist 键值对，值支持任意类型 |
| `environmentVariables` | object | XcScheme 环境变量，键值均为字符串 |
| `launcherArgs` | string[] | XcScheme 启动参数列表 |
| `podSource` | string[] | CocoaPods 源地址列表，替换 Podfile 默认源 |
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
  "runPathSearchPaths": {}
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
    "OTHER_LDFLAGS": "-ObjC"
  }
}
```

#### runPathSearchPaths — 运行时搜索路径

```json
{
  "runPathSearchPaths": {
    "LD_RUNPATH_SEARCH_PATHS": "@executable_path/Frameworks"
  }
}
```

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
    "associatedDomains": []
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

## 多配置合并

项目中可以放置多个 `XCodeConfig.json` 文件（如不同模块各自维护一份），构建时会自动发现并深度合并：

- **对象**：递归合并（子键逐层合并）
- **数组**：去重合并（union）
- **标量**：后者覆盖前者

这使得多 SDK / 多模块的 Xcode 配置可以独立管理、互不干扰。

## 完整示例

```json
{
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
    "associatedDomains": []
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
      "OTHER_LDFLAGS": "-ObjC"
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
      "OTHER_LDFLAGS": "-ObjC"
    },
    "files": {},
    "folders": {}
  }
}
```

## 注意事项

- 配置文件名称必须为 `XCodeConfig.json`，否则不会被识别
- 所有代码在 `#if UNITY_IOS` 条件编译下，不会影响其他平台
- 工具在 `[PostProcessBuild(ushort.MaxValue)]` 优先级运行，即所有其他后处理完成后执行
- 文件夹复制时如目标已存在会报错；文件复制时如目标已存在会先删除再复制

## 环境要求

- Unity 2017.1 及以上
- iOS 构建目标
- Xcode（Unity 导出 iOS 工程时自动依赖）

## 开源协议

[Apache License 2.0](LICENSE.md)
