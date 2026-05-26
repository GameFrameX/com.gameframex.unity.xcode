<div align="center">

![GameFrameX Logo](https://download.alianblank.com/gameframex/gameframex_logo_320.png)

# GameFrameX Xcode Config

[![Version](https://img.shields.io/github/v/release/gameframex/com.gameframex.unity.xcode?label=version&color=green)](https://github.com/gameframex/com.gameframex.unity.xcode/releases)
[![License](https://img.shields.io/badge/license-Apache%202.0-blue.svg)](LICENSE.md)
[![Documentation](https://img.shields.io/badge/docs-gameframex-brightgreen.svg)](https://gameframex.doc.alianblank.com)

**인디 게임 개발자를 위한 올인원 솔루션 · 인디 개발자의 꿈을 실현**

[📖 문서](https://gameframex.doc.alianblank.com) • [🚀 빠른 시작](#빠른-시작)

---

🌐 **언어**: [English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | **한국어**

---

</div>

Unity iOS 빌드 후 Xcode 프로젝트를 자동으로 구성하는 에디터 도구입니다. JSON 설정 파일을 통해 Info.plist, 프레임워크, 라이브러리, 빌드 속성, Capabilities, CocoaPods 소스, 현지화 등 모든 Xcode 설정을 선언적으로 관리하며, 수동 Xcode 조작이 필요 없습니다.

## 기능

- **Info.plist** — 문자열, 불리언, 정수, 배열, 딕셔너리 등의 타입을 지원하며 재귀적으로 작성
- **시스템 프레임워크/라이브러리** — `.framework` / `.tbd` 자동 추가 및 제거
- **빌드 속성** — Build Settings 설정, 추가, 제거 (예: `ENABLE_BITCODE`, `GCC_ENABLE_OBJC_EXCEPTIONS`)
- **Capabilities** — 인앱 결제, Game Center, 푸시 알림, Sign In with Apple, 백그라운드 모드, iCloud, App Groups, Associated Domains, Keychain Sharing, HealthKit, Siri, Personal VPN, Data Protection
- **현지화** — `.lproj/InfoPlist.strings` 자동 생성, 앱 이름 다국어 지원
- **CocoaPods** — Podfile 기본 소스 교체, 설정을 통한 pod 의존성 주입
- **XcScheme** — 환경 변수 및 실행 인수 주입
- **파일/폴더** — Xcode 프로젝트에 자동 복사 및 컴파일에 추가, `.framework`/`.bundle` 자동 인식
- **컴파일 플래그** — 특정 소스 파일에 컴파일 옵션 설정
- **링커 플래그** — `OTHER_LDFLAGS` 등 구성
- **Run Path Search Paths** — 런타임 검색 경로 구성
- **코드 서명** — Team ID, 번들 ID(Bundle Identifier), 서명 아이덴티티, 프로비저닝 프로필 설정
- **Swift 브릿징** — Swift 브릿징 헤더 자동 생성, Objective-C/Swift 상호 운용 지원, CI 환경에서 팝업 없음
- **다중 설정 병합** — 여러 `XCodeConfig.json` 파일의 깊은 재귀 병합 지원, 다중 모듈 협업에 적합

## 설치

다음 방법 중 하나를 선택하세요:

**방법 1: manifest.json 수정**

`Packages/manifest.json`의 `dependencies`에 추가:

```json
"com.gameframex.unity.xcode": "https://github.com/gameframex/com.gameframex.unity.xcode.git"
```

**방법 2: Package Manager Git URL**

Unity 에디터 → Window → Package Manager → Add package from git URL에 다음을 입력:

```
https://github.com/gameframex/com.gameframex.unity.xcode.git
```

**방법 3: 수동 다운로드**

이 저장소를 클론하거나 다운로드하여 Unity 프로젝트의 `Packages` 디렉토리에 넣으면 자동으로 인식됩니다.

## 빠른 시작

1. 패키지 내의 `Editor/XCodeConfigDemo.json`을 프로젝트 내 임의의 디렉토리에 복사
2. `XCodeConfig.json`으로 이름 변경
3. 필요에 따라 설정 항목 수정 (아래 설정 참조)
4. iOS 프로젝트를 빌드하면 도구가 모든 설정을 자동으로 적용

## 설정 파일 구조

설정 파일은 `XCodeConfig.json`이라는 이름이어야 합니다. 프로젝트 내 어디에나 배치할 수 있으며, 여러 파일 동시 배치를 지원합니다 (자동 병합됨).

### 최상위 구조

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

| 필드 | 타입 | 설명 |
| :--- | :--- | :--- |
| `swiftBridging` | bool | Swift 브릿징 헤더 자동 생성 활성화 (기본값: `true`) |
| `signing` | object | 코드 서명 설정 (아래 참조) |
| `plist` | object | Info.plist 키-값 쌍, 값은 모든 타입 지원 |
| `environmentVariables` | object | XcScheme 환경 변수, 키와 값 모두 문자열 |
| `launcherArgs` | string[] | XcScheme 실행 인수 목록 |
| `podSource` | string[] | CocoaPods 소스 URL 목록, Podfile 기본 소스 교체 |
| `localizations` | array | 현지화 설정 (아래 참조) |
| `capabilities` | object | iOS 앱 기능 설정 (아래 참조) |
| `unityFramework` | object | UnityFramework 타겟 설정 |
| `unityMain` | object | Unity-iPhone 타겟 설정 |

### unityFramework / unityMain

두 항목은 동일한 구조를 가지며, 각각 UnityFramework과 Unity-iPhone 타겟에 해당합니다:

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

#### libs — 시스템 라이브러리

```json
{
  "libs": {
    "+": ["libz.tbd", "libicucore.tbd"],
    "-": ["libstdc++.tbd"]
  }
}
```

- `+` 추가할 라이브러리 이름 목록
- `-` 제거할 라이브러리 이름 목록

#### frameworks — 시스템 프레임워크

```json
{
  "frameworks": {
    "+": ["WebKit.framework", "UserNotifications.framework"],
    "-": []
  }
}
```

- `+` 추가할 프레임워크 이름 목록
- `-` 제거할 프레임워크 이름 목록

#### properties — 빌드 속성

```json
{
  "properties": {
    "=": { "ENABLE_BITCODE": "NO" },
    "+": { "OTHER_CFLAGS": ["-flag1", "-flag2"] },
    "-": { "UNUSED_FLAG": [""] }
  }
}
```

- `=` 속성 설정 (키-값 쌍, 기존 값 덮어쓰기)
- `+` 속성 추가 (배열 값은 기존 목록에 추가)
- `-` 속성 제거

#### files — 파일 복사

```json
{
  "files": {
    "ios_libs.txt": "Classes/ios_libs.txt"
  }
}
```

- Key: Unity 프로젝트 내 파일 경로 (`Assets`와 동일 레벨)
- Value: Xcode 프로젝트에 복사할 상대 경로
- 대상이 이미 존재하면 삭제 후 복사

#### folders — 폴더 복사

```json
{
  "folders": {
    "XC": "Classes/XC"
  }
}
```

- Key: Unity 프로젝트 내 폴더 경로
- Value: Xcode 프로젝트에 복사할 상대 경로
- `.framework` 및 `.bundle` 자동 인식
- 대상이 이미 존재하면 오류 발생

#### filesCompileFlag — 파일 컴파일 플래그

```json
{
  "filesCompileFlag": {
    "Classes/PluginBase/UnityViewControllerListener.mm": "-fobjc-arc"
  }
}
```

- Key: Xcode 프로젝트 내 파일 경로
- Value: 설정할 컴파일 플래그

#### otherLinkerFlag — 링커 플래그

```json
{
  "otherLinkerFlag": {
    "OTHER_LDFLAGS": ["-ObjC"]
  }
}
```

- 문자열과 배열 형식 모두 지원합니다. 다중 설정 병합 시 올바른 중복 제거를 위해 배열 형식을 권장합니다

#### runPathSearchPaths — 런타임 검색 경로

```json
{
  "runPathSearchPaths": {
    "LD_RUNPATH_SEARCH_PATHS": ["@executable_path/Frameworks"]
  }
}
```

- 문자열과 배열 형식 모두 지원합니다. 다중 설정 병합 시 올바른 중복 제거를 위해 배열 형식을 권장합니다

### signing — 코드 서명

Unity-iPhone(메인) 타겟에만 적용됩니다. 모든 필드는 선택 사항입니다.

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

| 필드 | 타입 | 설명 |
| :--- | :--- | :--- |
| `teamId` | string | Apple Developer Team ID (`DEVELOPMENT_TEAM`) |
| `bundleId` | string | 앱 번들 ID (`PRODUCT_BUNDLE_IDENTIFIER`) |
| `codeSignIdentity` | string | 코드 서명 아이덴티티, 옵션: `Apple Development`, `Apple Distribution`, `iPhone Developer`, `iPhone Distribution` |
| `codeSignStyle` | string | 서명 방식: `Automatic` 또는 `Manual` |
| `provisioningProfileSpecifier` | string | 프로비저닝 프로필 이름 (Manual 모드에서 필요) |

### swiftBridging — Swift 브릿징

Unity-iPhone(메인) 타겟에만 적용됩니다. 활성화 시(기본값) Swift 파일과 브릿징 헤더를 자동 생성하여 Objective-C/Swift 상호 운용을 지원합니다. Xcode 팝업이 없어 CI 환경에 적합합니다.

```json
{
  "swiftBridging": true
}
```

- 기본값은 `true`, `false`로 비활성화 가능
- `gameframex_swift_bridging.swift`와 `Unity-iPhone-Bridging-Header.h` 자동 생성
- `SWIFT_VERSION`을 `5.0`으로 설정하고 `SWIFT_OBJC_BRIDGING_HEADER` 구성

### capabilities — 앱 기능

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

| 필드 | 타입 | 설명 |
| :--- | :--- | :--- |
| `inAppPurchase` | bool | 인앱 결제 |
| `gameCenter` | bool | Game Center |
| `pushNotifications` | bool | 푸시 알림 |
| `signInWithApple` | bool | Sign In with Apple |
| `backgroundModes` | string[] | 백그라운드 모드, 옵션: `audio`, `location`, `voip`, `newsstand`, `external`, `bluetooth`, `bluetooth-peripheral`, `fetch`, `remote-notification` |
| `iCloud.keyValueStorage` | bool | iCloud 키-값 스토리지 |
| `iCloud.iCloudDocument` | bool | iCloud 문서 스토리지 |
| `iCloud.customContainers` | string[] | iCloud 커스텀 컨테이너 |
| `appGroups` | string[] | App Group 식별자 |
| `associatedDomains` | string[] | 연관 도메인 (Universal Links) |
| `keychainSharing` | bool 또는 object | Keychain Sharing. `false` 비활성화, `true` 기본 그룹 사용, 또는 `{"accessGroups": ["group1"]}` 커스텀 그룹 지정 |
| `healthKit` | bool | HealthKit 건강 데이터 접근 |
| `siri` | bool | Siri(SiriKit 통합) |
| `personalVPN` | bool | Personal VPN(앱별 VPN) |
| `dataProtection` | bool | Data Protection(파일 수준 암호화) |

### localizations — 현지화

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

- `languageCode` — ISO 639-1 언어 코드 (중국어는 `zh-Hans` 간체 / `zh-Hant` 번체)
- `validMap` — 키-값 쌍 목록, 각 항목은 `key`와 `value` 포함
- `.lproj/InfoPlist.strings` 파일이 자동 생성되어 프로젝트에 추가

### plist — Info.plist 설정

임의의 중첩 수준을 지원합니다. 일반적인 설정:

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
    "NSCameraUsageDescription": "QR 스캔을 위해 카메라 접근이 필요합니다",
    "ITSAppUsesNonExemptEncryption": false
  }
}
```

### pods — CocoaPods 의존 라이브러리 (unityMain / unityFramework 내 설정)

`pods`는 `unityMain` 및/또는 `unityFramework` 내부에 설정합니다. 각 타겟의 pods는 해당 Podfile 타겟 블록(`target 'Unity-iPhone' do` 또는 `target 'UnityFramework' do`)에 주입됩니다. 중복되는 pod 이름은 자동으로 건너뜁니다.

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

- Key = pod 이름, Value = 버전 제약
- 값이 비어있으면 → `pod 'Name'`, 값이 있으면 → `pod 'Name', 'Value'`
- `pod install`은 자동 실행되지 않으며, 수동 또는 CI에서 실행해야 합니다

## 다중 설정 병합

프로젝트에 여러 `XCodeConfig.json` 파일을 배치할 수 있습니다 (각 모듈이 자체 파일을 관리). 빌드 시 자동으로 감지되어 깊은 병합이 수행됩니다:

- **객체**: 재귀적으로 병합 (하위 키를 레이어별로 병합)
- **배열**: 유니온 병합 (중복 제거)
- **스칼라**: 후자가 전자를 덮어씀

여러 SDK / 모듈의 Xcode 설정을 독립적으로 관리하고 충돌을 방지할 수 있습니다.

## 전체 예시

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
    "NSCameraUsageDescription": "카메라 접근 권한이 필요합니다",
    "NSMicrophoneUsageDescription": "마이크 접근 권한이 필요합니다",
    "NSPhotoLibraryUsageDescription": "사진 라이브러리 접근 권한이 필요합니다",
    "ITSAppUsesNonExemptEncryption": false,
    "NSUserTrackingUsageDescription": "이 식별자는 맞춤형 광고 게재에 사용됩니다"
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

## 주의사항

- 설정 파일은 `XCodeConfig.json`이라는 이름이어야 하며, 다른 이름으로는 인식되지 않습니다
- 모든 코드는 `#if UNITY_IOS` 조건부 컴파일 하에 있어 다른 플랫폼에 영향을 주지 않습니다
- 도구는 `[PostProcessBuild(888)]` 우선순위로 실행되어 대부분의 다른 후처리가 완료된 후 실행됩니다
- 폴더 복사 시 대상이 이미 존재하면 오류가 발생합니다. 파일 복사 시 대상이 이미 존재하면 삭제 후 복사합니다

## 요구 사항

- Unity 2017.1 이상
- iOS 빌드 타겟
- Xcode (Unity가 iOS 프로젝트를 내보낼 때 자동으로 필요)

## 라이선스

[Apache License 2.0](LICENSE.md)
