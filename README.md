## HOMEPAGE

GameFrameX 的 Xcode 导出项目配置

# 使用文档(文档编写于GPT4)

## 参数解析

- plist plist配置
    + Key 是属性名，Value 是属性值
- environmentVariables Xcode 中的XcScheme Run的环境变量
    + Key 是属性名，Value 是属性值
- launcherArgs Xcode 中的XcScheme Run 的启动参数
    + 值是一个字符串列表
- podSource 当使用pod的时候，该参数替换默认的源内容，可以配置多个源
    + 值是一个字符串列表
- unityFramework UnityFramework的配置参数
- unityMain Unity Iphone 的配置参数
    - libs 需要添加的库
        - `+` 需要添加的库名称
        - `-` 需要移除的库名称

        + 值是一个字符串列表
    - frameworks 需要添加的框架
        - `=` 需要设置的框架名称
        - `+` 需要添加的框架名称
        - `-` 需要移除的框架名称

        + 值是一个字符串列表
    - properties 需要添加的属性
        - `=` 需要设置的属性
        - `+` 需要添加的属性
        - `-` 需要移除的属性

        + Key 是属性名，Value 是属性值
    - filesCompileFlag 需要添加的文件编译标志
        + Key 是文件在Xcode项目中的路径，Value 是需要添加的文件编译标志
    - otherLinkerFlag 需要添加的链接器标志
        + Key 是文件在Xcode项目中的路径，Value 是需要添加的链接器标志
    - folders 需要添加的文件夹
        + Key是文件夹在工程的相对于项目的路径（和`Assets`目录同级别），Value是复制到XCode 项目的相对路径。如果已经存在则会报错
    - files 需要添加的文件
        + Key是文件在工程的相对于项目的路径（和`Assets`目录同级别），Value是复制到XCode 项目的相对路径。如果已经存在则会删除，然后复制

## 示例配置

```json
{
  "plist": {
    "CFBundleURLTypes": [
      {
        "CFBundleTypeRole": "Editor",
        "CFBundleURLSchemes": [
          "bbqgame"
        ],
        "CFBundleURLName": "com.smartdogx.bbq"
      },
      {
        "CFBundleTypeRole": "Editor",
        "CFBundleURLSchemes": [
          "wx5dfe430e96b395a6"
        ]
      },
      {
        "CFBundleTypeRole": "Editor",
        "CFBundleURLSchemes": [
          "QQ41E77C8B"
        ]
      }
    ],
    "LSApplicationQueriesSchemes": [
      "weixin",
      "wechat",
      "mqqapi",
      "mqqopensdkapiV2",
      "mqqopensdkapiV3",
      "mqqOpensdkSSoLogin",
      "mqqwpa",
      "mqq"
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
    "NSCameraUsageDescription": "需要您的相机权限,APP才能识别图书",
    "NSLocationWhenInUseUsageDescription": "需要您的位置权限,APP才能确定您的所属位置",
    "NSMicrophoneUsageDescription": "需要您的麦克风权限,APP才能正常使用部分功能",
    "NSPhotoLibraryAddUsageDescription": "需要您的相册权限,APP才能保存照片到相册",
    "NSPhotoLibraryUsageDescription": "需要您的相册权限,APP才能保存照片到相册",
    "ITSAppUsesNonExemptEncryption": false,
    "NSUserTrackingUsageDescription": "此标识符将用于向您推荐个性化广告"
  },
  "environmentVariables": {
    "IDEPreferLogStreaming": "YES",
    "OS_ACTIVITY_MODE": "disable"
  },
  "launcherArgs": [
    "xx",
    "aaaa",
    "bbbbb"
  ],
  "podSource": [
    "https://mirrors.tuna.tsinghua.edu.cn/git/CocoaPods/Specs.git"
  ],
  "unityFramework": {
    "libs": {
      "+": [
        "libicucore.tbd",
        "libz.tbd",
        "libstdc++.tbd",
        "libsqlite3.tbd"
      ],
      "-": []
    },
    "frameworks": {
      "+": [
        "JavaScriptCore.framework",
        "AdServices.framework",
        "Security.framework",
        "CoreVideo.framework",
        "SystemConfiguration.framework",
        "AdSupport.framework",
        "WebKit.framework",
        "UserNotifications.framework",
        "AppTrackingTransparency.framework",
        "AssetsLibrary.framework"
      ],
      "-": []
    },
    "properties": {
      "=": {
        "ENABLE_BITCODE": "NO",
        "GCC_ENABLE_OBJC_EXCEPTIONS": true,
        "GCC_ENABLE_CPP_EXCEPTIONS": true,
        "CLANG_ENABLE_OBJC_ARC": true
      },
      "+": {},
      "-": {}
    },
    "filesCompileFlag": {
      "Classes/PluginBase/UnityViewControllerListener.mm": "-hjdaj",
      "Classes/PluginBase/LifeCycleListener.mm": "-dsacz-dasdsa-dzxcxz-dsadsa"
    },
    "otherLinkerFlag": {
      "OTHER_LDFLAGS": "-ObjC"
    },
    "files": {
      "ios_libs.txt": "Classes/ios_libs.txt"
    },
    "folders": {
      "XC": "Classes/XC"
    }
  },
  "unityMain": {
    "libs": {
      "+": [
        "libicucore.tbd",
        "libz.tbd",
        "libstdc++.tbd",
        "libsqlite3.tbd"
      ],
      "-": []
    },
    "frameworks": {
      "+": [
        "JavaScriptCore.framework",
        "AdServices.framework",
        "Security.framework",
        "CoreVideo.framework",
        "SystemConfiguration.framework",
        "AdSupport.framework",
        "WebKit.framework",
        "UserNotifications.framework",
        "AppTrackingTransparency.framework",
        "AssetsLibrary.framework"
      ],
      "-": []
    },
    "properties": {
      "=": {
        "ENABLE_BITCODE": "NO",
        "GCC_ENABLE_OBJC_EXCEPTIONS": true
      },
      "+": {},
      "-": {}
    },
    "filesCompileFlag": {},
    "otherLinkerFlag": {
      "OTHER_LDFLAGS": "-ObjC"
    },
    "files": {
      "ios_libsM.txt": "Classes/ios_libsM.txt"
    },
    "folders": {
      "XCM": "Classes/XCM"
    }
  }
}

```

## 注意事项

- 配置文件的名称必须为 `XCodeConfig.json`

# 使用方式(任选其一)

1. 直接在 `manifest.json` 的文件中的 `dependencies` 节点下添加以下内容
   ```json
      {"com.gameframex.unity.xcode": "https://github.com/gameframex/com.gameframex.unity.xcode.git"}
    ```
2. 在Unity 的`Packages Manager` 中使用`Git URL` 的方式添加库,地址为：https://github.com/gameframex/com.gameframex.unity.xcode.git

3. 直接下载仓库放置到Unity 项目的`Packages` 目录下。会自动加载识别