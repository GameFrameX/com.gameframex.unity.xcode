<div align="center">

![GameFrameX Logo](https://download.alianblank.com/gameframex/gameframex_logo_320.png)

# GameFrameX Xcode Config

[![Version](https://img.shields.io/github/v/release/gameframex/com.gameframex.unity.xcode?label=version&color=green)](https://github.com/gameframex/com.gameframex.unity.xcode/releases)
[![License](https://img.shields.io/badge/license-Apache%202.0-blue.svg)](LICENSE.md)
[![Documentation](https://img.shields.io/badge/docs-gameframex-brightgreen.svg)](https://gameframex.doc.alianblank.com)

**All-in-One Solution for Indie Game Development · Empowering Indie Developers' Dreams**

[📖 Documentation](https://gameframex.doc.alianblank.com) • [🚀 Quick Start](#quick-start)

---

🌐 **Language**: **English** | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | [한국어](README.ko.md)

---

</div>

An editor tool that automatically configures Xcode projects after Unity iOS builds. Declaratively manage all Xcode settings — Info.plist, frameworks, libraries, build properties, Capabilities, CocoaPods sources, localizations, and more — through a JSON config file, with no manual Xcode operations needed.

## Features

- **Info.plist** — Supports string, boolean, integer, array, dictionary types with recursive writing
- **System Frameworks/Libraries** — Automatically add or remove `.framework` / `.tbd`
- **Build Properties** — Set, append, or remove Build Settings (e.g. `ENABLE_BITCODE`, `GCC_ENABLE_OBJC_EXCEPTIONS`)
- **Capabilities** — In-App Purchase, Game Center, Push Notifications, Sign In with Apple, Background Modes, iCloud, App Groups, Associated Domains, Keychain Sharing, HealthKit, Siri, Personal VPN, Data Protection
- **Localization** — Auto-generate `.lproj/InfoPlist.strings` with multi-language app name support
- **CocoaPods** — Replace Podfile default source, inject pod dependencies via config
- **XcScheme** — Inject environment variables and launch arguments
- **Files/Folders** — Auto-copy to Xcode project and add to compilation, recognize `.framework`/`.bundle`
- **Compile Flags** — Set compile options for specific source files
- **Linker Flags** — Configure `OTHER_LDFLAGS` etc.
- **Run Path Search Paths** — Configure runtime search paths
- **Code Signing** — Configure Team ID, bundle identifier, code sign identity, and provisioning profile
- **Swift Bridging** — Auto-create Swift bridging header for Objective-C/Swift interop, CI-friendly with no Xcode prompts
- **Multi-Config Merge** — Support deep recursive merge of multiple `XCodeConfig.json` files for multi-module collaboration

## Installation

Choose one of the following methods:

**Method 1: Edit manifest.json**

Add to `Packages/manifest.json` under `dependencies`:

```json
"com.gameframex.unity.xcode": "https://github.com/gameframex/com.gameframex.unity.xcode.git"
```

**Method 2: Package Manager Git URL**

Unity Editor → Window → Package Manager → Add package from git URL, enter:

```
https://github.com/gameframex/com.gameframex.unity.xcode.git
```

**Method 3: Manual Download**

Clone or download this repository and place it in your Unity project's `Packages` directory. It will be recognized automatically.

## Quick Start

1. Copy `Editor/XCodeConfigDemo.json` from the package to any directory in your project
2. Rename it to `XCodeConfig.json`
3. Modify the configuration as needed (see Configuration Reference below)
4. Build the iOS project — the tool will automatically apply all settings

## Configuration Reference

The configuration file must be named `XCodeConfig.json`. It can be placed anywhere in the project, and multiple files are supported (they will be merged automatically).

### Top-Level Structure

```json
{
  "swiftBridging": true,
  "signing": {},
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

| Field | Type | Description |
| :--- | :--- | :--- |
| `swiftBridging` | bool | Enable Swift bridging header auto-generation (default: `true`) |
| `signing` | object | Code signing configuration (see below) |
| `plist` | object | Info.plist key-value pairs, values support any type |
| `environmentVariables` | object | XcScheme environment variables, both keys and values are strings |
| `launcherArgs` | string[] | XcScheme launch arguments list |
| `podSource` | string[] | CocoaPods source URLs, replaces Podfile default source |
| `localizations` | array | Localization configuration (see below) |
| `capabilities` | object | iOS app capability configuration (see below) |
| `unityFramework` | object | UnityFramework target configuration |
| `unityMain` | object | Unity-iPhone target configuration |

### unityFramework / unityMain

Both share the same structure, targeting UnityFramework and Unity-iPhone respectively:

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

#### libs — System Libraries

```json
{
  "libs": {
    "+": ["libz.tbd", "libicucore.tbd"],
    "-": ["libstdc++.tbd"]
  }
}
```

- `+` Library names to add
- `-` Library names to remove

#### frameworks — System Frameworks

```json
{
  "frameworks": {
    "+": ["WebKit.framework", "UserNotifications.framework"],
    "-": []
  }
}
```

- `+` Framework names to add
- `-` Framework names to remove

#### properties — Build Properties

```json
{
  "properties": {
    "=": { "ENABLE_BITCODE": "NO" },
    "+": { "OTHER_CFLAGS": ["-flag1", "-flag2"] },
    "-": { "UNUSED_FLAG": [""] }
  }
}
```

- `=` Set properties (key-value pairs, overwrites existing values)
- `+` Append properties (array values are appended to existing list)
- `-` Remove properties

#### files — File Copy

```json
{
  "files": {
    "ios_libs.txt": "Classes/ios_libs.txt"
  }
}
```

- Key: File path in the Unity project (same level as `Assets`)
- Value: Relative path to copy to in the Xcode project
- If the destination exists, it will be deleted before copying

#### folders — Folder Copy

```json
{
  "folders": {
    "XC": "Classes/XC"
  }
}
```

- Key: Folder path in the Unity project
- Value: Relative path to copy to in the Xcode project
- Automatically recognizes `.framework` and `.bundle`
- An error is raised if the destination already exists

#### filesCompileFlag — File Compile Flags

```json
{
  "filesCompileFlag": {
    "Classes/PluginBase/UnityViewControllerListener.mm": "-fobjc-arc"
  }
}
```

- Key: File path in the Xcode project
- Value: Compile flags to set

#### otherLinkerFlag — Linker Flags

```json
{
  "otherLinkerFlag": {
    "OTHER_LDFLAGS": ["-ObjC"]
  }
}
```

- Values support both string and array format; array format is recommended for proper multi-config merging

#### runPathSearchPaths — Runtime Search Paths

```json
{
  "runPathSearchPaths": {
    "LD_RUNPATH_SEARCH_PATHS": ["@executable_path/Frameworks"]
  }
}
```

- Values support both string and array format; array format is recommended for proper multi-config merging

### signing — Code Signing

Applied to the Unity-iPhone (main) target only. All fields are optional.

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

| Field | Type | Description |
| :--- | :--- | :--- |
| `teamId` | string | Apple Developer Team ID (`DEVELOPMENT_TEAM`) |
| `bundleId` | string | App bundle identifier (`PRODUCT_BUNDLE_IDENTIFIER`) |
| `codeSignIdentity` | string | Code signing identity, options: `Apple Development`, `Apple Distribution`, `iPhone Developer`, `iPhone Distribution` |
| `codeSignStyle` | string | Signing style: `Automatic` or `Manual` |
| `provisioningProfileSpecifier` | string | Provisioning profile name (required for Manual mode) |

### swiftBridging — Swift Bridging Header

Applied to the Unity-iPhone (main) target only. When enabled (default), automatically creates a Swift file and bridging header for Objective-C/Swift interoperability. No Xcode prompts — suitable for CI environments.

```json
{
  "swiftBridging": true
}
```

- Default is `true`; set to `false` to disable
- Creates `gameframex_swift_bridging.swift` and `Unity-iPhone-Bridging-Header.h` in the Xcode project
- Sets `SWIFT_VERSION` to `5.0` and configures `SWIFT_OBJC_BRIDGING_HEADER`

### capabilities — App Capabilities

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

| Field | Type | Description |
| :--- | :--- | :--- |
| `inAppPurchase` | bool | In-App Purchase |
| `gameCenter` | bool | Game Center |
| `pushNotifications` | bool | Push Notifications |
| `signInWithApple` | bool | Sign In with Apple |
| `backgroundModes` | string[] | Background modes, options: `audio`, `location`, `voip`, `newsstand`, `external`, `bluetooth`, `bluetooth-peripheral`, `fetch`, `remote-notification` |
| `iCloud.keyValueStorage` | bool | iCloud Key-Value Storage |
| `iCloud.iCloudDocument` | bool | iCloud Document Storage |
| `iCloud.customContainers` | string[] | iCloud custom containers |
| `appGroups` | string[] | App Group identifiers |
| `associatedDomains` | string[] | Associated domains (Universal Links) |
| `keychainSharing` | bool or object | Keychain Sharing. `false` to disable, `true` for default group, or `{"accessGroups": ["group1"]}` for custom groups |
| `healthKit` | bool | HealthKit |
| `siri` | bool | Siri (SiriKit integration) |
| `personalVPN` | bool | Personal VPN (per-app VPN) |
| `dataProtection` | bool | Data Protection (file-level encryption) |

### localizations — Localization

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

- `languageCode` — ISO 639-1 language code (use `zh-Hans` for Simplified Chinese / `zh-Hant` for Traditional Chinese)
- `validMap` — Key-value pairs, each with `key` and `value`
- Automatically generates `.lproj/InfoPlist.strings` and adds them to the project

### plist — Info.plist Configuration

Supports arbitrary nested levels. Common configuration:

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
    "NSCameraUsageDescription": "Camera access is required for QR scanning",
    "ITSAppUsesNonExemptEncryption": false
  }
}
```

### pods — CocoaPods Dependencies (inside unityMain / unityFramework)

`pods` is configured inside `unityMain` and/or `unityFramework` sections. Each target's pods are injected into the corresponding Podfile target block (`target 'Unity-iPhone' do` or `target 'UnityFramework' do`). Duplicate pod names are skipped automatically.

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

- Key = pod name, Value = version constraint
- Empty value → `pod 'Name'`, non-empty → `pod 'Name', 'Value'`
- `pod install` is **not** executed automatically; run it manually or in CI

## Multi-Config Merging

You can place multiple `XCodeConfig.json` files in the project (e.g., each module maintains its own). During build, they are automatically discovered and deep-merged:

- **Objects**: Recursively merged (child keys merged layer by layer)
- **Arrays**: Union-merged (deduplicated)
- **Scalars**: Latter overwrites former

This allows Xcode configurations from multiple SDKs / modules to be managed independently without conflicts.

## Full Example

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
    "NSCameraUsageDescription": "Camera access is required",
    "NSMicrophoneUsageDescription": "Microphone access is required",
    "NSPhotoLibraryUsageDescription": "Photo library access is required",
    "ITSAppUsesNonExemptEncryption": false,
    "NSUserTrackingUsageDescription": "This identifier will be used to deliver personalized ads"
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

## Notes

- The configuration file must be named `XCodeConfig.json`, otherwise it will not be recognized
- All code is under `#if UNITY_IOS` conditional compilation, no impact on other platforms
- The tool runs at `[PostProcessBuild(888)]` priority, executing after most other post-processing
- Folder copy raises an error if the destination exists; file copy deletes the existing destination first

## Requirements

- Unity 2017.1 or later
- iOS build target
- Xcode (automatically required when Unity exports iOS project)

## License

[Apache License 2.0](LICENSE.md)
