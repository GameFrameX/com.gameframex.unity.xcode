<div align="center">

![GameFrameX Logo](https://download.alianblank.com/gameframex/gameframex_logo_320.png)

# GameFrameX Xcode Config

[![Version](https://img.shields.io/github/v/release/gameframex/com.gameframex.unity.xcode?label=version&color=green)](https://github.com/gameframex/com.gameframex.unity.xcode/releases)
[![License](https://img.shields.io/badge/license-Apache%202.0-blue.svg)](LICENSE.md)
[![Documentation](https://img.shields.io/badge/docs-gameframex-brightgreen.svg)](https://gameframex.doc.alianblank.com)

**インディゲーム開発者向けオールインワンソリューション · インディ開発者の夢を支援**

[📖 ドキュメント](https://gameframex.doc.alianblank.com) • [🚀 クイックスタート](#クイックスタート)

---

🌐 **言語**: [English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | **日本語** | [한국어](README.ko.md)

---

</div>

Unity iOS ビルド後に Xcode プロジェクトを自動設定するエディタツール。JSON 設定ファイルにより、Info.plist、フレームワーク、ライブラリ、ビルドプロパティ、Capabilities、CocoaPods ソース、ローカライズなどすべての Xcode 設定を宣言的に管理し、手動での Xcode 操作は不要です。

## 機能概要

- **Info.plist** — 文字列、ブール値、整数、配列、辞書などの型をサポート、再帰的に書き込み
- **システムフレームワーク/ライブラリ** — `.framework` / `.tbd` の自動追加・削除
- **ビルドプロパティ** — Build Settings の設定・追加・削除（`ENABLE_BITCODE`、`GCC_ENABLE_OBJC_EXCEPTIONS` など）
- **Capabilities** — アプリ内課金、Game Center、プッシュ通知、Sign In with Apple、バックグラウンドモード、iCloud、App Groups、Associated Domains、Keychain Sharing、HealthKit、Siri、Personal VPN、Data Protection
- **ローカライズ** — `.lproj/InfoPlist.strings` の自動生成、アプリ名の多言語対応
- **CocoaPods** — Podfile デフォルトソースの置き換え、設定による pod 依存関係の注入
- **XcScheme** — 環境変数と起動引数の注入
- **ファイル/フォルダ** — Xcode プロジェクトへの自動コピーとコンパイル追加、`.framework`/`.bundle` の自動認識
- **コンパイルフラグ** — 特定のソースファイルにコンパイルオプションを設定
- **リンカフラグ** — `OTHER_LDFLAGS` などの設定
- **Run Path Search Paths** — ランタイム検索パスの設定
- **コード署名** — Team ID、バンドル ID（Bundle Identifier）、署名アイデンティティ、プロビジョニングプロファイルの設定
- **Swift ブリッジング** — Swift ブリッジングヘッダーを自動生成、Objective-C/Swift 相互運用、CI 環境でプロンプトなし
- **マルチ設定マージ** — 複数の `XCodeConfig.json` の深層再帰マージをサポート、マルチモジュール連携に最適

## インストール

以下のいずれかの方法を選択してください：

**方法 1: manifest.json を編集**

`Packages/manifest.json` の `dependencies` に追加：

```json
"com.gameframex.unity.xcode": "https://github.com/gameframex/com.gameframex.unity.xcode.git"
```

**方法 2: Package Manager の Git URL**

Unity エディタ → Window → Package Manager → Add package from git URL に以下を入力：

```
https://github.com/gameframex/com.gameframex.unity.xcode.git
```

**方法 3: 手動ダウンロード**

このリポジトリをクローンまたはダウンロードし、Unity プロジェクトの `Packages` ディレクトリに配置すると自動的に認識されます。

## クイックスタート

1. パッケージ内の `Editor/XCodeConfigDemo.json` をプロジェクト内の任意のディレクトリにコピー
2. `XCodeConfig.json` にリネーム
3. 設定項目を必要に応じて変更（下記の設定リファレンスを参照）
4. iOS プロジェクトをビルドすると、ツールがすべての設定を自動的に適用

## 設定ファイル構造

設定ファイルは `XCodeConfig.json` という名前にする必要があります。プロジェクト内の任意の場所に配置でき、複数ファイルの同時配置もサポート（自動マージされます）。

### トップレベル構造

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

| フィールド | 型 | 説明 |
| :--- | :--- | :--- |
| `swiftBridging` | bool | Swift ブリッジングヘッダーの自動生成を有効化（デフォルト：`true`） |
| `signing` | object | コード署名設定（下記参照） |
| `plist` | object | Info.plist のキーと値のペア、値は任意の型をサポート |
| `environmentVariables` | object | XcScheme 環境変数、キーと値はともに文字列 |
| `launcherArgs` | string[] | XcScheme 起動引数リスト |
| `podSource` | string[] | CocoaPods ソース URL リスト、Podfile デフォルトソースを置き換え |
| `pods` | object | CocoaPods 依存ライブラリ、key = pod 名、value = バージョン制約（下記参照） |
| `localizations` | array | ローカライズ設定（下記参照） |
| `capabilities` | object | iOS アプリ機能設定（下記参照） |
| `unityFramework` | object | UnityFramework ターゲット設定 |
| `unityMain` | object | Unity-iPhone ターゲット設定 |

### unityFramework / unityMain

両者は同じ構造を持ち、それぞれ UnityFramework と Unity-iPhone ターゲットに対応します：

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

#### libs — システムライブラリ

```json
{
  "libs": {
    "+": ["libz.tbd", "libicucore.tbd"],
    "-": ["libstdc++.tbd"]
  }
}
```

- `+` 追加するライブラリ名のリスト
- `-` 削除するライブラリ名のリスト

#### frameworks — システムフレームワーク

```json
{
  "frameworks": {
    "+": ["WebKit.framework", "UserNotifications.framework"],
    "-": []
  }
}
```

- `+` 追加するフレームワーク名のリスト
- `-` 削除するフレームワーク名のリスト

#### properties — ビルドプロパティ

```json
{
  "properties": {
    "=": { "ENABLE_BITCODE": "NO" },
    "+": { "OTHER_CFLAGS": ["-flag1", "-flag2"] },
    "-": { "UNUSED_FLAG": [""] }
  }
}
```

- `=` プロパティの設定（キーと値のペア、既存の値を上書き）
- `+` プロパティの追加（配列値は既存リストに追加）
- `-` プロパティの削除

#### files — ファイルコピー

```json
{
  "files": {
    "ios_libs.txt": "Classes/ios_libs.txt"
  }
}
```

- Key：Unity プロジェクト内のファイルパス（`Assets` と同階層）
- Value：Xcode プロジェクトにコピーする相対パス
- コピー先が存在する場合は削除後にコピー

#### folders — フォルダコピー

```json
{
  "folders": {
    "XC": "Classes/XC"
  }
}
```

- Key：Unity プロジェクト内のフォルダパス
- Value：Xcode プロジェクトにコピーする相対パス
- `.framework` と `.bundle` を自動認識
- コピー先が既に存在する場合はエラー

#### filesCompileFlag — ファイルコンパイルフラグ

```json
{
  "filesCompileFlag": {
    "Classes/PluginBase/UnityViewControllerListener.mm": "-fobjc-arc"
  }
}
```

- Key：Xcode プロジェクト内のファイルパス
- Value：設定するコンパイルフラグ

#### otherLinkerFlag — リンカフラグ

```json
{
  "otherLinkerFlag": {
    "OTHER_LDFLAGS": ["-ObjC"]
  }
}
```

- 値は文字列と配列の両方をサポートします。複数設定のマージ時に正しく重複排除されるため、配列形式を推奨します

#### runPathSearchPaths — ランタイム検索パス

```json
{
  "runPathSearchPaths": {
    "LD_RUNPATH_SEARCH_PATHS": ["@executable_path/Frameworks"]
  }
}
```

- 値は文字列と配列の両方をサポートします。複数設定のマージ時に正しく重複排除されるため、配列形式を推奨します

### signing — コード署名

Unity-iPhone（メイン）ターゲットにのみ適用されます。すべてのフィールドは省略可能です。

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

| フィールド | 型 | 説明 |
| :--- | :--- | :--- |
| `teamId` | string | Apple Developer Team ID（`DEVELOPMENT_TEAM`） |
| `bundleId` | string | アプリのバンドル ID（`PRODUCT_BUNDLE_IDENTIFIER`） |
| `codeSignIdentity` | string | コード署名アイデンティティ、オプション：`Apple Development`、`Apple Distribution`、`iPhone Developer`、`iPhone Distribution` |
| `codeSignStyle` | string | 署名方式：`Automatic` または `Manual` |
| `provisioningProfileSpecifier` | string | プロビジョニングプロファイル名（Manual モードで必要） |

### swiftBridging — Swift ブリッジング

Unity-iPhone（メイン）ターゲットにのみ適用されます。有効時（デフォルト）Swift ファイルとブリッジングヘッダーを自動生成し、Objective-C/Swift 相互運用を実現します。Xcode のプロンプトが表示されず、CI 環境に適しています。

```json
{
  "swiftBridging": true
}
```

- デフォルトは `true`、`false` で無効化
- `gameframex_swift_bridging.swift` と `Unity-iPhone-Bridging-Header.h` を自動生成
- `SWIFT_VERSION` を `5.0` に設定し、`SWIFT_OBJC_BRIDGING_HEADER` を構成

### capabilities — アプリ機能

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

| フィールド | 型 | 説明 |
| :--- | :--- | :--- |
| `inAppPurchase` | bool | アプリ内課金 |
| `gameCenter` | bool | Game Center |
| `pushNotifications` | bool | プッシュ通知 |
| `signInWithApple` | bool | Sign In with Apple |
| `backgroundModes` | string[] | バックグラウンドモード、オプション：`audio`、`location`、`voip`、`newsstand`、`external`、`bluetooth`、`bluetooth-peripheral`、`fetch`、`remote-notification` |
| `iCloud.keyValueStorage` | bool | iCloud キー値ストレージ |
| `iCloud.iCloudDocument` | bool | iCloud ドキュメントストレージ |
| `iCloud.customContainers` | string[] | iCloud カスタムコンテナ |
| `appGroups` | string[] | App Group 識別子 |
| `associatedDomains` | string[] | 関連ドメイン（Universal Links） |
| `keychainSharing` | bool または object | Keychain Sharing。`false` で無効、`true` でデフォルトグループ、または `{"accessGroups": ["group1"]}` でカスタムグループを指定 |
| `healthKit` | bool | HealthKit ヘルスデータアクセス |
| `siri` | bool | Siri（SiriKit 統合） |
| `personalVPN` | bool | Personal VPN（アプリごとの VPN） |
| `dataProtection` | bool | Data Protection（ファイルレベルの暗号化） |

### localizations — ローカライズ

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

- `languageCode` — ISO 639-1 言語コード（中国語は `zh-Hans` 簡体 / `zh-Hant` 繁体）
- `validMap` — キーと値のペアリスト、各項目に `key` と `value` を含む
- `.lproj/InfoPlist.strings` ファイルが自動生成されプロジェクトに追加

### plist — Info.plist 設定

任意のネストレベルをサポート。一般的な設定：

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
    "NSCameraUsageDescription": "QRスキャンにカメラへのアクセスが必要です",
    "ITSAppUsesNonExemptEncryption": false
  }
}
```

### pods — CocoaPods 依存ライブラリ

Podfile の `target 'Unity-iPhone' do` ブロックに `pod` 宣言を注入します。重複する pod 名は自動的にスキップされます。

```json
{
  "pods": {
    "FirebaseAnalytics": "",
    "FBSDKLoginKit": "~> 14.0"
  }
}
```

- Key = pod 名、Value = バージョン制約
- 値が空 → `pod 'Name'`、値が非空 → `pod 'Name', 'Value'`
- `pod install` は自動実行されません。手動または CI で実行してください

## マルチ設定マージ

プロジェクト内に複数の `XCodeConfig.json` ファイルを配置できます（各モジュールが独自に管理）。ビルド時に自動的に検出され、深層マージされます：

- **オブジェクト**：再帰的にマージ（子キーをレイヤーごとにマージ）
- **配列**：ユニオンマージ（重複排除）
- **スカラー**：後の値が前の値を上書き

複数の SDK / モジュールの Xcode 設定を独立して管理し、競合を防ぐことができます。

## 完全な例

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
    "NSCameraUsageDescription": "カメラへのアクセスが必要です",
    "NSMicrophoneUsageDescription": "マイクへのアクセスが必要です",
    "NSPhotoLibraryUsageDescription": "フォトライブラリへのアクセスが必要です",
    "ITSAppUsesNonExemptEncryption": false,
    "NSUserTrackingUsageDescription": "この識別子はパーソナライズされた広告の配信に使用されます"
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

## 注意事項

- 設定ファイルは `XCodeConfig.json` という名前にする必要があります。それ以外の名前では認識されません
- すべてのコードは `#if UNITY_IOS` 条件コンパイル下にあり、他のプラットフォームには影響しません
- ツールは `[PostProcessBuild(ushort.MaxValue)]` 優先度で実行され、他のすべての後処理が完了した後に実行されます
- フォルダのコピー先が既に存在する場合はエラーになります。ファイルのコピー先が既に存在する場合は削除後にコピーされます

## 動作環境

- Unity 2017.1 以上
- iOS ビルドターゲット
- Xcode（Unity が iOS プロジェクトをエクスポートする際に自動的に必要）

## ライセンス

[Apache License 2.0](LICENSE.md)
