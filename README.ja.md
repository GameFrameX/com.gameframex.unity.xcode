<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# GameFrameX Xcode Config

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.xcode)](https://github.com/GameFrameX/com.gameframex.unity.xcode/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.xcode)](https://github.com/GameFrameX/com.gameframex.unity.xcode/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

インディゲーム開発者向けオールインワンソリューション · インディ開発者の夢を支援

<br />

[ドキュメント](https://gameframex.doc.alianblank.com) · [クイックスタート](#quick-start) · QQグループ: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | **日本語** | [한국어](README.ko.md)

</div>

## 機能概要

- **Info.plist** — 文字列、ブール値、整数、配列、辞書などの型をサポート、再帰的に書き込み
- **システムフレームワーク/ライブラリ** — `.framework` / `.tbd` の自動追加・削除
- **ビルドプロパティ** — Build Settings の設定・追加・削除（`ENABLE_BITCODE`、`GCC_ENABLE_OBJC_EXCEPTIONS` など）
- **Capabilities** — アプリ内課金、Game Center、プッシュ通知、Sign In with Apple、バックグラウンドモード、iCloud、App Groups、Associated Domains、Keychain Sharing、HealthKit、Siri、Personal VPN、Data Protection
- **ローカライズ** — `.lproj/InfoPlist.strings` の自動生成、アプリ名の多言語対応
- **CocoaPods** — Podfile デフォルトソースの置き換え、設定による pod 依存関係の注入、`pod install` の自動実行
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

### インストール

Unity プロジェクトの `Packages/manifest.json` を編集し、`scopedRegistries` セクションを追加してください：

```json
{
  "scopedRegistries": [
    {
      "name": "GameFrameX",
      "url": "https://gameframex.upm.alianblank.uk",
      "scopes": [
        "com.gameframex"
      ]
    }
  ]
}
```

`scopes` は、どのパッケージをこのレジストリから解決するかを制御します。`com.gameframex` で始まるパッケージのみがこのレジストリから取得されます。

Then add the package to `dependencies`:

```json
{
  "dependencies": {
    "com.gameframex.unity.xcode": "1.11.1"
  }
}
```

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
  "podfile": "",
  "podInstall": true,
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
| `podfile` | string | カスタム Podfile のパス、ビルド出力にコピー（`pods` より優先） |
| `podInstall` | bool | Podfile 処理後に `pod install` を自動実行（デフォルト：`true`） |
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
  "runPathSearchPaths": {},
  "pods": {}
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

#### languageCode に指定できる値

参考：[Apple Developer - Language and Locale IDs](https://developer.apple.com/library/archive/documentation/MacOSX/Conceptual/BPInternational/LanguageandLocaleIDs/LanguageandLocaleIDs.html)

よく使用されるコードは**太字**で表示しています。

| コード | 言語 | コード | 言語 | コード | 言語 | コード | 言語 |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| **en** | **英語** | **zh** | **中国語** | **ja** | **日本語** | **ko** | **韓国語** |
| **es** | **スペイン語** | **fr** | **フランス語** | **de** | **ドイツ語** | **it** | **イタリア語** |
| **pt** | **ポルトガル語** | **ru** | **ロシア語** | **ar** | **アラビア語** | **hi** | **ヒンディー語** |
| **tr** | **トルコ語** | **vi** | **ベトナム語** | **th** | **タイ語** | **id** | **インドネシア語** |
| aa | アファル語 | ab | アブハズ語 | ae | アヴェスタ語 | af | アフリカーンス語 |
| ak | アカン語 | am | アムハラ語 | an | アラゴン語 | as | アッサム語 |
| av | アヴァル語 | ay | アイマラ語 | az | アゼルバイジャン語 | ba | バシキール語 |
| be | ベラルーシ語 | bg | ブルガリア語 | bh | ビハール語 | bi | ビスラマ語 |
| bm | バンバラ語 | bn | ベンガル語 | bo | チベット語 | br | ブルトン語 |
| bs | ボスニア語 | ca | カタロニア語 | ce | チェチェン語 | ch | チャモロ語 |
| co | コルシカ語 | cr | クリー語 | cs | チェコ語 | cu | 教会スラブ語 |
| cv | チュヴァシ語 | cy | ウェールズ語 | da | デンマーク語 | dv | ディベヒ語 |
| dz | ゾンカ語 | ee | エウェ語 | el | ギリシャ語 | eo | エスペラント |
| et | エストニア語 | eu | バスク語 | fa | ペルシャ語 | ff | フラ語 |
| fi | フィンランド語 | fj | フィジー語 | fo | フェロー語 | fy | 西フリジア語 |
| ga | アイルランド語 | gd | スコットランド・ゲール語 | gl | ガリシア語 | gn | グアラニー語 |
| gu | グジャラート語 | gv | マンクス語 | ha | ハウサ語 | he | ヘブライ語 |
| ho | ヒリモツ語 | hr | クロアチア語 | ht | ハイチ・クレオール語 | hu | ハンガリー語 |
| hy | アルメニア語 | hz | ヘレロ語 | ia | インテリングア | ie | インテルリングエ |
| ig | イボ語 | ii | 四川イ語 | ik | イヌピアック語 | io | イド語 |
| is | アイスランド語 | iu | イヌクティトット語 | jv | ジャワ語 | ka | グルジア語 |
| kg | コンゴ語 | ki | キクユ語 | kj | クアニャマ語 | kk | カザフ語 |
| kl | カラーリスット語 | km | クメール語 | kn | カンナダ語 | kr | カヌリ語 |
| ks | カシミール語 | ku | クルド語 | kv | コミ語 | kw | コーンウォール語 |
| ky | キルギス語 | la | ラテン語 | lb | ルクセンブルク語 | lg | ガンダ語 |
| li | リンブルフ語 | ln | リンガラ語 | lo | ラオ語 | lt | リトアニア語 |
| lu | ルバ・カタンガ語 | lv | ラトビア語 | mg | マダガスカル語 | mh | マーシャル語 |
| mi | マオリ語 | mk | マケドニア語 | ml | マラヤーラム語 | mn | モンゴル語 |
| mr | マラーティー語 | ms | マレー語 | mt | マルタ語 | my | ビルマ語 |
| na | ナウル語 | nb | ノルウェー・ブークモール | nd | 北ンデベレ語 | ne | ネパール語 |
| ng | ンドンガ語 | nl | オランダ語 | nn | ノルウェー・ニーノシュク | no | ノルウェー語 |
| nr | 南ンデベレ語 | nv | ナバホ語 | ny | チチェワ語 | oc | オック語 |
| oj | オジブワ語 | om | オロモ語 | or | オリヤー語 | os | オセット語 |
| pa | パンジャブ語 | pi | パーリ語 | pl | ポーランド語 | ps | パシュトー語 |
| qu | ケチュア語 | rm | ロマンシュ語 | rn | ルンディ語 | ro | ルーマニア語 |
| rw | キンヤルワンダ語 | sa | サンスクリット語 | sc | サルデーニャ語 | sd | シンド語 |
| se | 北サーミ語 | sg | サンゴ語 | si | シンハラ語 | sk | スロバキア語 |
| sl | スロベニア語 | sm | サモア語 | sn | ショナ語 | so | ソマリ語 |
| sq | アルバニア語 | sr | セルビア語 | ss | スワティ語 | st | 南ソト語 |
| su | スンダ語 | sv | スウェーデン語 | sw | スワヒリ語 | ta | タミル語 |
| te | テルグ語 | tg | タジク語 | ti | ティグリニャ語 | tk | トルクメン語 |
| tl | タガログ語 | tn | ツワナ語 | to | トンガ語 | ts | ツォンガ語 |
| tt | タタール語 | tw | トウィ語 | ty | タヒチ語 | ug | ウイグル語 |
| uk | ウクライナ語 | ur | ウルドゥー語 | uz | ウズベク語 | ve | ヴェンダ語 |
| vo | ヴォラピュク語 | wa | ワロン語 | wo | ウォロフ語 | xh | コサ語 |
| yi | イディッシュ語 | yo | ヨルバ語 | za | チュワン語 | zu | ズールー語 |

> **補足**：
> - **中国語**：通常は `zh-Hans`（簡体字）と `zh-Hant`（繁体字）を使用します。
> - **ポルトガル語**：よく使われる変種は `pt-BR`（ブラジル）と `pt-PT`（ポルトガル）です。
> - その他の変種は `コード-地域` の形式で組み合わせ可能です（例：`en-GB`、`fr-CA`）。

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

### pods — CocoaPods 依存ライブラリ（unityMain / unityFramework 内に設定）

`pods` は `unityMain` や `unityFramework` の内部に設定します。各ターゲットの pods は対応する Podfile ターゲットブロック（`target 'Unity-iPhone' do` または `target 'UnityFramework' do`）に注入されます。重複する pod 名は自動的にスキップされます。

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

- Key = pod 名、Value = バージョン制約
- 値が空 → `pod 'Name'`、値が非空 → `pod 'Name', 'Value'`

### podfile — カスタム Podfile

`pods` で個別に依存関係を注入する代わりに、完全な Podfile を指定することもできます。設定時は `pods` より優先され、ファイルがビルド出力に直接コピーされた後、`podSource` のソース URL が適用されます。

```json
{
  "podfile": "XcodePodfile/Podfile"
}
```

- 相対パス（Unity プロジェクトルート、`Assets/` と同階層からの相対）と絶対パスをサポート
- ファイルが存在しない場合は警告を出力し、`pods` 設定パスにフォールバック

### podInstall — pod install の自動実行

Podfile 処理後に `pod install` を自動的に実行するかどうかを制御します。システムの `PATH` に `pod` CLI が必要です。

```json
{
  "podInstall": true
}
```

- デフォルトは `true`、ビルド出力に Podfile が存在する場合に自動実行
- `false` に設定するとスキップ（CI で個別に `pod install` を実行する場合など）
- 標準出力は info レベルでログ出力、終了コードが非ゼロの場合の stderr は error レベルで記録

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

## 注意事項

- 設定ファイルは `XCodeConfig.json` という名前にする必要があります。それ以外の名前では認識されません
- すべてのコードは `#if UNITY_IOS` 条件コンパイル下にあり、他のプラットフォームには影響しません
- ツールは `[PostProcessBuild(888)]` 優先度で実行され、ほとんどの他の後処理が完了した後に実行されます
- フォルダのコピー先が既に存在する場合はエラーになります。ファイルのコピー先が既に存在する場合は削除後にコピーされます

## 動作環境

- Unity 2017.1 以上
- iOS ビルドターゲット
- Xcode（Unity が iOS プロジェクトをエクスポートする際に自動的に必要）

## ライセンス

詳しくは [LICENSE.md](LICENSE.md) をご参照ください。
