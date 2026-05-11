using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFrameX.Xcode.Editor
{
    /// <summary>
    /// Xcode 配置
    /// </summary>
    [CreateAssetMenu(fileName = "XcodeConfig", menuName = "XcodeConfig/Create Settings")]
    public sealed class XcodeConfig : ScriptableObject
    {
        /// <summary>
        /// 项目环境变量
        /// </summary>
        [Header("项目环境变量")] [SerializeField] public List<XcodeConfigMap> environmentVariables = new List<XcodeConfigMap>();

        /// <summary>
        /// 项目启动参数
        /// </summary>
        [Header("项目启动参数")] [SerializeField] public List<string> launcherArgs = new List<string>();

        /// <summary>
        /// Plist 列表
        /// </summary>
        [Header("Plist列表")] [SerializeField] public List<XcodeConfigPlist> plist = new List<XcodeConfigPlist>();

        /// <summary>
        /// Capabilities 配置
        /// </summary>
        [Header("Capabilities配置")] [SerializeField] public XcodeConfigCapabilities capabilities = new XcodeConfigCapabilities();

        /// <summary>
        /// 主项目配置
        /// </summary>
        [Header("主项目")] [SerializeField] public XcodeConfigData unityMain = new XcodeConfigData();

        /// <summary>
        /// UnityFrameWork 配置
        /// </summary>
        [Header("UnityFrameWork")] [SerializeField]
        public XcodeConfigData unityFrameWork = new XcodeConfigData();

        /// <summary>
        /// 本地化 列表
        /// </summary>
        [Header("本地化列表")] [SerializeField] public List<XcodeConfigLocalization> localizations = new List<XcodeConfigLocalization>();
    }

    /// <summary>
    /// Xcode 配置数据
    /// </summary>
    [Serializable]
    public sealed class XcodeConfigData
    {
        /// <summary>
        /// 库列表
        /// </summary>
        [Header("库列表")] [SerializeField] public XcodeConfigChange library = new XcodeConfigChange();

        /// <summary>
        /// 框架列表
        /// </summary>
        [Header("框架列表")] [SerializeField] public XcodeConfigChange frameworks = new XcodeConfigChange();

        /// <summary>
        /// 属性列表
        /// </summary>
        [Header("属性列表")] [SerializeField] public List<XcodeConfigMap> properties = new List<XcodeConfigMap>();

        /// <summary>
        /// 文件复制列表
        /// </summary>
        [Header("文件复制列表")] [SerializeField] public List<XcodeConfigMap> files = new List<XcodeConfigMap>();

        /// <summary>
        /// 文件夹复制列表
        /// </summary>
        [Header("文件夹复制列表")] [SerializeField] public List<XcodeConfigMap> folders = new List<XcodeConfigMap>();

        /// <summary>
        /// 文件编译设置列表
        /// </summary>
        [Header("文件编译设置列表")] [SerializeField] public List<XcodeConfigMap> filesCompileFlag = new List<XcodeConfigMap>();

        /// <summary>
        /// 其他链接列表
        /// </summary>
        [Header("其他链接列表")] [SerializeField] public List<XcodeConfigMap> otherLinkerFlag = new List<XcodeConfigMap>();
    }

    /// <summary>
    /// Plist 值类型枚举
    /// </summary>
    public enum PlistType
    {
        /// <summary>
        /// 字符串
        /// </summary>
        String,

        /// <summary>
        /// 布尔值
        /// </summary>
        Boolean,

        /// <summary>
        /// 数组
        /// </summary>
        Array,

        /// <summary>
        /// 字典
        /// </summary>
        Dictionary,
    }

    /// <summary>
    /// Xcode Config Plist 配置
    /// </summary>
    [Serializable]
    public sealed class XcodeConfigPlist
    {
        /// <summary>
        /// 键
        /// </summary>
        [Header("键")] [SerializeField] public string key;

        /// <summary>
        /// 值类型
        /// </summary>
        [Header("值类型")] [SerializeField] public PlistType type;

        /// <summary>
        /// 字符串值
        /// </summary>
        [Header("字符串值")] [SerializeField] public string stringValue;

        /// <summary>
        /// 布尔值
        /// </summary>
        [Header("布尔值")] [SerializeField] public bool boolValue;

        /// <summary>
        /// 字典对象
        /// </summary>
        [Header("字典对象")] [SerializeField] public List<XcodeConfigPlist> mapValue = new List<XcodeConfigPlist>();

        /// <summary>
        /// 数组对象
        /// </summary>
        [Header("数组对象")] [SerializeField] public List<XcodeConfigMap> arrayValue = new List<XcodeConfigMap>();
    }

    /// <summary>
    /// Xcode Config 映射配置
    /// </summary>
    [Serializable]
    public sealed class XcodeConfigMap
    {
        /// <summary>
        /// 键
        /// </summary>
        [Header("键")] [SerializeField] public string key;

        /// <summary>
        /// 值
        /// </summary>
        [Header("值")] [SerializeField] public string value;
    }

    /// <summary>
    /// Xcode Config 变更配置
    /// </summary>
    [Serializable]
    public sealed class XcodeConfigChange
    {
        /// <summary>
        /// 增加项
        /// </summary>
        [Header("增加")] [SerializeField] public string[] add;

        /// <summary>
        /// 删除项
        /// </summary>
        [Header("删除")] [SerializeField] public string[] remove;
    }

    /// <summary>
    /// Xcode Capabilities 配置
    /// </summary>
    [Serializable]
    public sealed class XcodeConfigCapabilities
    {
        /// <summary>
        /// 应用内购买
        /// </summary>
        [Header("In-App Purchase")] [SerializeField] public bool inAppPurchase = false;

        /// <summary>
        /// 游戏中心
        /// </summary>
        [Header("Game Center")] [SerializeField] public bool gameCenter = false;

        /// <summary>
        /// 推送通知
        /// </summary>
        [Header("Push Notifications")] [SerializeField] public bool pushNotifications = false;

        /// <summary>
        /// 通过 Apple 登录
        /// </summary>
        [Header("Sign In with Apple")] [SerializeField] public bool signInWithApple = false;

        /// <summary>
        /// 后台模式
        /// </summary>
        [Header("Background Modes")] [SerializeField] public string[] backgroundModes = new string[0];

        /// <summary>
        /// iCloud 键值存储
        /// </summary>
        [Header("iCloud Key-Value Storage")] [SerializeField] public bool iCloudKeyValueStorage = false;

        /// <summary>
        /// iCloud 文档
        /// </summary>
        [Header("iCloud Documents")] [SerializeField] public bool iCloudDocuments = false;

        /// <summary>
        /// iCloud 自定义容器
        /// </summary>
        [Header("iCloud Custom Containers")] [SerializeField] public string[] iCloudCustomContainers = new string[0];

        /// <summary>
        /// App Groups
        /// </summary>
        [Header("App Groups")] [SerializeField] public string[] appGroups = new string[0];

        /// <summary>
        /// 关联域名
        /// </summary>
        [Header("Associated Domains")] [SerializeField] public string[] associatedDomains = new string[0];

        /// <summary>
        /// Keychain Sharing
        /// </summary>
        [Header("Keychain Sharing")] [SerializeField] public bool keychainSharing = false;

        /// <summary>
        /// Keychain Sharing 访问组
        /// </summary>
        [Header("Keychain Sharing Access Groups")] [SerializeField] public string[] keychainSharingAccessGroups = new string[0];

        /// <summary>
        /// HealthKit
        /// </summary>
        [Header("HealthKit")] [SerializeField] public bool healthKit = false;

        /// <summary>
        /// Siri
        /// </summary>
        [Header("Siri")] [SerializeField] public bool siri = false;

        /// <summary>
        /// Personal VPN
        /// </summary>
        [Header("Personal VPN")] [SerializeField] public bool personalVPN = false;

        /// <summary>
        /// Data Protection
        /// </summary>
        [Header("Data Protection")] [SerializeField] public bool dataProtection = false;
    }


    /// <summary>
    /// Xcode Localization 本地化配置
    /// </summary>
    [Serializable]
    public sealed class XcodeConfigLocalization
    {
        /// <summary>
        /// 语言代码. 例如: en, zh-Hans, zh-Hant, ja
        /// </summary>
        [Header("语言代码")] [SerializeField] public string languageCode;

        /// <summary>
        /// 本地化内容
        /// </summary>
        [Header("本地化内容")] [SerializeField] public List<XcodeConfigMap> validMap = new List<XcodeConfigMap>();
    }
}