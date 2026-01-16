## HOMEPAGE

GameFrameX 的 Xcode 导出项目配置

# 使用文档(文档编写于GPT4)

1. 请将包内的 `XCodeConfigDemo.json` 复制到项目中修改为 `XCodeConfig.json`
2. 按需修改自己的参数配置

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
- localizations 本地化配置参数
    + 包含多个语言配置对象，每个对象包含 `languageCode` (语言代码) 和 `validMap` (键值对列表)
    + `validMap` 中每个项包含 `key` (如 `CFBundleDisplayName`) 和 `value` (本地化内容)
    *   **完整 ISO 639-1 语言代码列表**:
        > 官方参考: [Apple Developer - Language and Locale IDs](https://developer.apple.com/library/archive/documentation/MacOSX/Conceptual/BPInternational/LanguageandLocaleIDs/LanguageandLocaleIDs.html)
        > 以下列出了所有 ISO 639-1 两字母语言代码。常用代码已加粗。

        | 代码 | 语言 | 代码 | 语言 | 代码 | 语言 | 代码 | 语言 |
        | :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |
        | **en** | **英语** | **zh** | **中文** | **ja** | **日语** | **ko** | **韩语** |
        | **es** | **西班牙语** | **fr** | **法语** | **de** | **德语** | **it** | **意大利语** |
        | **pt** | **葡萄牙语** | **ru** | **俄语** | **ar** | **阿拉伯语** | **hi** | **印地语** |
        | **tr** | **土耳其语** | **vi** | **越南语** | **th** | **泰语** | **id** | **印尼语** |
        | aa | 阿法尔语 | ab | 阿布哈兹语 | ae | 阿维斯陀语 | af | 南非荷兰语 |
        | ak | 阿坎语 | am | 阿姆哈拉语 | an | 阿拉贡语 | as | 阿萨姆语 |
        | av | 阿瓦尔语 | ay | 艾马拉语 | az | 阿塞拜疆语 | ba | 巴什基尔语 |
        | be | 白俄罗斯语 | bg | 保加利亚语 | bh | 比哈尔语 | bi | 比斯拉马语 |
        | bm | 班巴拉语 | bn | 孟加拉语 | bo | 藏语 | br | 布列塔尼语 |
        | bs | 波斯尼亚语 | ca | 加泰罗尼亚语 | ce | 车臣语 | ch | 查莫罗语 |
        | co | 科西嘉语 | cr | 克里语 | cs | 捷克语 | cu | 教会斯拉夫语 |
        | cv | 楚瓦什语 | cy | 威尔士语 | da | 丹麦语 | dv | 迪维希语 |
        | dz | 宗卡语 | ee | 埃维语 | el | 希腊语 | eo | 世界语 |
        | et | 爱沙尼亚语 | eu | 巴斯克语 | fa | 波斯语 | ff | 富拉语 |
        | fi | 芬兰语 | fj | 斐济语 | fo | 法罗语 | fy | 西弗里西亚语 |
        | ga | 爱尔兰语 | gd | 苏格兰盖尔语 | gl | 加利西亚语 | gn | 瓜拉尼语 |
        | gu | 古吉拉特语 | gv | 曼岛语 | ha | 豪萨语 | he | 希伯来语 |
        | ho | 希里莫图语 | hr | 克罗地亚语 | ht | 海地克里奥尔语 | hu | 匈牙利语 |
        | hy | 亚美尼亚语 | hz | 赫雷罗语 | ia | 国际语A | ie | 国际语E |
        | ig | 伊博语 | ii | 四川彝语 | ik | 依努庇克语 | io | 伊多语 |
        | is | 冰岛语 | iu | 因纽特语 | jv | 爪哇语 | ka | 格鲁吉亚语 |
        | kg | 刚果语 | ki | 吉库尤语 | kj | 宽亚玛语 | kk | 哈萨克语 |
        | kl | 格陵兰语 | km | 高棉语 | kn | 卡纳达语 | kr | 卡努里语 |
        | ks | 克什米尔语 | ku | 库尔德语 | kv | 科米语 | kw | 康沃尔语 |
        | ky | 吉尔吉斯语 | la | 拉丁语 | lb | 卢森堡语 | lg | 干达语 |
        | li | 林堡语 | ln | 林加拉语 | lo | 老挝语 | lt | 立陶宛语 |
        | lu | 鲁巴-加丹加语 | lv | 拉脱维亚语 | mg | 马尔加什语 | mh | 马绍尔语 |
        | mi | 毛利语 | mk | 马其顿语 | ml | 马拉雅拉姆语 | mn | 蒙古语 |
        | mr | 马拉地语 | ms | 马来语 | mt | 马耳他语 | my | 缅甸语 |
        | na | 瑙鲁语 | nb | 挪威博克马尔语 | nd | 北恩德贝勒语 | ne | 尼泊尔语 |
        | ng | 恩东加语 | nl | 荷兰语 | nn | 挪威尼诺斯克语 | no | 挪威语 |
        | nr | 南恩德贝勒语 | nv | 纳瓦霍语 | ny | 切瓦语 | oc | 奥克语 |
        | oj | 奥吉布瓦语 | om | 奥罗莫语 | or | 奥里亚语 | os | 奥塞梯语 |
        | pa | 旁遮普语 | pi | 巴利语 | pl | 波兰语 | ps | 普什图语 |
        | qu | 克丘亚语 | rm | 罗曼什语 | rn | 隆迪语 | ro | 罗马尼亚语 |
        | rw | 卢旺达语 | sa | 梵语 | sc | 萨丁尼亚语 | sd | 信德语 |
        | se | 北萨米语 | sg | 桑戈语 | si | 僧伽罗语 | sk | 斯洛伐克语 |
        | sl | 斯洛文尼亚语 | sm | 萨摩亚语 | sn | 绍纳语 | so | 索马里语 |
        | sq | 阿尔巴尼亚语 | sr | 塞尔维亚语 | ss | 斯瓦蒂语 | st | 南索托语 |
        | su | 巽他语 | sv | 瑞典语 | sw | 斯瓦希里语 | ta | 泰米尔语 |
        | te | 泰卢固语 | tg | 塔吉克语 | ti | 提格雷尼亚语 | tk | 土库曼语 |
        | tl | 他加禄语 | tn | 茨瓦纳语 | to | 汤加语 | ts | 聪加语 |
        | tt | 鞑靼语 | tw | 特威语 | ty | 塔希提语 | ug | 维吾尔语 |
        | uk | 乌克兰语 | ur | 乌尔都语 | uz | 乌兹别克语 | ve | 文达语 |
        | vo | 沃拉普克语 | wa | 瓦隆语 | wo | 沃洛夫语 | xh | 科萨语 |
        | yi | 意第绪语 | yo | 约鲁巴语 | za | 壮语 | zu | 祖鲁语 |

        > **特殊说明**:
        > *   **中文**: 通常使用 `zh-Hans` (简体) 和 `zh-Hant` (繁体)。
        > *   **葡萄牙语**: 常用 `pt-BR` (巴西) 和 `pt-PT` (葡萄牙)。
        > *   **其他变体**: 可以通过 `代码-地区` 的方式组合，例如 `en-GB` (英国英语), `fr-CA` (加拿大法语)。
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
  "localizations": [
    {
      "languageCode": "en",
      "validMap": [
        {
          "key": "CFBundleDisplayName",
          "value": "My Game"
        }
      ]
    },
    {
      "languageCode": "zh-Hans",
      "validMap": [
        {
          "key": "CFBundleDisplayName",
          "value": "我的游戏"
        }
      ]
    }
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