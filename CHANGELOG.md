# [1.10.0](https://github.com/gameframex/com.gameframex.unity.xcode/compare/1.9.0...1.10.0) (2026-05-26)


### Features

* **xcode:** 支持自定义 Podfile 路径 ([9ec2b65](https://github.com/gameframex/com.gameframex.unity.xcode/commit/9ec2b65bda9de9f762b5a7433bdcbf0890756103))

# [1.9.0](https://github.com/gameframex/com.gameframex.unity.xcode/compare/1.8.0...1.9.0) (2026-05-26)


### Features

* **xcode:** 支持渠道专属 Xcode 配置合并 ([8cde132](https://github.com/gameframex/com.gameframex.unity.xcode/commit/8cde132c3ccffff07a4a1fe5e45755f93ea7e178))

# [1.8.0](https://github.com/gameframex/com.gameframex.unity.xcode/compare/1.7.0...1.8.0) (2026-05-13)


### Bug Fixes

* **xcode:** bundle 资源包同时添加到主项目构建引用 ([b910f82](https://github.com/gameframex/com.gameframex.unity.xcode/commit/b910f82f4276447a38730118b9fced23b0d25d6f))
* **xcode:** 使用 PlayerSettings 获取 iOS 版本 ([b9917e0](https://github.com/gameframex/com.gameframex.unity.xcode/commit/b9917e0ec67bbb03f9ae1aa2d35ac29b3d5741df))
* **xcode:** 修复 build property 数组值未逐项追加 ([846160d](https://github.com/gameframex/com.gameframex.unity.xcode/commit/846160d024652ef4fe78fe086fe7fd6fd344fccf))
* **xcode:** 修复 framework target 桥接头文件编译错误 ([18170f9](https://github.com/gameframex/com.gameframex.unity.xcode/commit/18170f988f61d7c6ec692148eda43cdb41fda358))
* **xcode:** 修复文件夹复制时 framework/bundle/静态库链接 ([3ed071a](https://github.com/gameframex/com.gameframex.unity.xcode/commit/3ed071ae2935567bdf8ed3dfce1fadd7194d3b93))
* **xcode:** 将 swift bridging 文件添加到 UnityFramework target ([de0784d](https://github.com/gameframex/com.gameframex.unity.xcode/commit/de0784db62cf2f232c0f0a399587a7033e1da8be))
* **xcode:** 移除 xcframework 的 framework 引用 ([b4602b9](https://github.com/gameframex/com.gameframex.unity.xcode/commit/b4602b9cb501b3b84cc070b1c760ef7135b7bf5f))


### Features

* **xcode:** 修复 libs 配置未生效 ([b69e4d3](https://github.com/gameframex/com.gameframex.unity.xcode/commit/b69e4d3d7c518b430d5122b3bdbe06171d25f386)), closes [#12](https://github.com/gameframex/com.gameframex.unity.xcode/issues/12)
* **xcode:** 支持 build property 数组值格式 ([10b94e9](https://github.com/gameframex/com.gameframex.unity.xcode/commit/10b94e97ea858fe125be7945b97d6700d3665672))
* **xcode:** 添加 CocoaPods 依赖库自动注入 ([92d840a](https://github.com/gameframex/com.gameframex.unity.xcode/commit/92d840aae611902f906f380a706802968a7b6efb))
* **xcode:** 添加 Swift 桥接头文件自动生成 ([460ff97](https://github.com/gameframex/com.gameframex.unity.xcode/commit/460ff97c781dd6b526b5d2e749831073e426a9ce))
* **xcode:** 添加 xcframework 支持 ([1e0f5d7](https://github.com/gameframex/com.gameframex.unity.xcode/commit/1e0f5d7919fc73631ed89241d0b02bcb99b80f99))
* **xcode:** 重构 pods 配置为按 target 独立设置 ([35726e6](https://github.com/gameframex/com.gameframex.unity.xcode/commit/35726e638dd0cccbc867925eceb4422571a9457e))

# [1.7.0](https://github.com/gameframex/com.gameframex.unity.xcode/compare/1.6.0...1.7.0) (2026-05-11)


### Features

* **capabilities:** 扩展 HealthKit/Siri/VPN/DataProtection 支持 ([7e92d74](https://github.com/gameframex/com.gameframex.unity.xcode/commit/7e92d744f9360d74189ba3974f60390ee16de583))
* **capabilities:** 添加 Keychain Sharing 支持 ([932f233](https://github.com/gameframex/com.gameframex.unity.xcode/commit/932f23303a03279e45ef7f113f45cd45fb9288d7))

# [1.6.0](https://github.com/gameframex/com.gameframex.unity.xcode/compare/1.5.1...1.6.0) (2026-05-09)


### Features

* **signing:** 添加 Xcode 签名配置支持 ([0c3d5a2](https://github.com/gameframex/com.gameframex.unity.xcode/commit/0c3d5a2a11a9dd622d64fc75eccac4599afbdd75))

## [1.5.1](https://github.com/gameframex/com.gameframex.unity.xcode/compare/1.5.0...1.5.1) (2026-05-09)


### Bug Fixes

* **Capabilities:** 使用安全的类型检查替代强制转换 ([0efc455](https://github.com/gameframex/com.gameframex.unity.xcode/commit/0efc4552ed0fea0f1c235a103dcc29ccff0f3c3e))
* **HashtableEX:** 修复键值对解析越界访问 ([330e0d5](https://github.com/gameframex/com.gameframex.unity.xcode/commit/330e0d5bba6cf0dda8b47f2f54c8e97d46b50caf))
* **MiniJSON:** 使用InvariantCulture解析浮点数 ([5e921b2](https://github.com/gameframex/com.gameframex.unity.xcode/commit/5e921b2044e571eb6d2c5344ef16da1ab1561b0a))
* **Plist:** 添加缺失的using System指令 ([9496973](https://github.com/gameframex/com.gameframex.unity.xcode/commit/9496973e281be196658ad60c52f27138d3cc0855))
* **PostProcessBuildHelper:** 重新读取PBXProject以保留Capabilities修改 ([266c0a2](https://github.com/gameframex/com.gameframex.unity.xcode/commit/266c0a21628f407bec722e746e1a2f373dab90b0))

# [1.5.0](https://github.com/gameframex/com.gameframex.unity.xcode/compare/1.4.0...1.5.0) (2026-01-17)


### Features

* **Xcode编辑器:** 添加对CFBundleDisplayName本地化的支持 ([8bdea75](https://github.com/gameframex/com.gameframex.unity.xcode/commit/8bdea75076438da74caba2bef1a802003ef20f3b))

# [1.4.0](https://github.com/gameframex/com.gameframex.unity.xcode/compare/1.3.0...1.4.0) (2026-01-17)


### Bug Fixes

* **Xcode本地化:** 修复本地化文件添加到Xcode项目的问题 ([9f5db58](https://github.com/gameframex/com.gameframex.unity.xcode/commit/9f5db5862ce18ff9ba240d0fffb6ea1249d38a0b))


### Features

* **Xcode:** 添加iOS项目本地化支持 ([ee401b8](https://github.com/gameframex/com.gameframex.unity.xcode/commit/ee401b829328918ef0cdea1728a6dfbcbacd016c))
* **XCode配置:** 添加应用本地化配置支持 ([72a7bf0](https://github.com/gameframex/com.gameframex.unity.xcode/commit/72a7bf0c2b0555a7af75d5c65e7f33bd5ad0c6fa))
* **本地化:** 添加对Info.plist的本地化键自动更新功能 ([8f47e84](https://github.com/gameframex/com.gameframex.unity.xcode/commit/8f47e84d6391c46c077eef1aab67de193c7edfc5))

# [1.3.0](https://github.com/gameframex/com.gameframex.unity.xcode/compare/1.2.0...1.3.0) (2026-01-16)


### Bug Fixes

* **Xcode:** 修复Capabilities设置中的targetGuid参数问题 ([e353bfc](https://github.com/gameframex/com.gameframex.unity.xcode/commit/e353bfcd1e0c0dc8f2ec56bf98758243e00faebd))
* 修复文件复制时覆盖已存在文件的问题 ([8700265](https://github.com/gameframex/com.gameframex.unity.xcode/commit/870026517347ef23e46567b510e510b8ee5025ce))


### Features

* **HashtableEX:** 添加深度合并Hashtable的功能 ([5b2cd1a](https://github.com/gameframex/com.gameframex.unity.xcode/commit/5b2cd1af5cf1d9a2f4f877261d73c26a966f078a))
* **Xcode编辑器:** 添加加载匹配配置文件的功能 ([e1c93e2](https://github.com/gameframex/com.gameframex.unity.xcode/commit/e1c93e2c1f9a6131208d229bf71149348c92b57b))
* **XCode配置:** 支持合并多个XCodeConfig配置文件并优化处理流程 ([0cd6f38](https://github.com/gameframex/com.gameframex.unity.xcode/commit/0cd6f3810634cc2697d4c69533ff434d90a1a62f))

# [1.2.0](https://github.com/gameframex/com.gameframex.unity.xcode/compare/1.1.1...1.2.0) (2025-12-23)


### Features

* **ci:** change ci ([19068ee](https://github.com/gameframex/com.gameframex.unity.xcode/commit/19068ee24327abbeb878575377ac9cbfa4cf0cd2))

# Changelog

## [1.1.1](https://github.com/GameFrameX/com.gameframex.unity.xcode/tree/1.1.1) (2025-06-01)

[Full Changelog](https://github.com/GameFrameX/com.gameframex.unity.xcode/compare/1.1.0...1.1.1)

## [1.1.0](https://github.com/GameFrameX/com.gameframex.unity.xcode/tree/1.1.0) (2025-05-31)

[Full Changelog](https://github.com/GameFrameX/com.gameframex.unity.xcode/compare/1.0.0...1.1.0)

## [1.0.0](https://github.com/GameFrameX/com.gameframex.unity.xcode/tree/1.0.0) (2024-09-21)

[Full Changelog](https://github.com/GameFrameX/com.gameframex.unity.xcode/compare/17ca019b89c5ec95d482c2191deee2724a9f7095...1.0.0)



\* *This Changelog was automatically generated by [github_changelog_generator](https://github.com/github-changelog-generator/github-changelog-generator)*
