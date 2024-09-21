using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFrameX.Xcode.Editor
{
    [CreateAssetMenu(fileName = "XcodeConfig", menuName = "XcodeConfig/Create Settings")]
    public sealed class XcodeConfig : ScriptableObject
    {
        [Header("项目环境变量")] [SerializeField] public List<XcodeConfigMap> environmentVariables = new List<XcodeConfigMap>();
        [Header("项目启动参数")] [SerializeField] public List<string> launcherArgs = new List<string>();

        [Header("Plist列表")] [SerializeField] public List<XcodeConfigPlist> plist = new List<XcodeConfigPlist>();

        [Header("主项目")] [SerializeField] public XcodeConfigData unityMain = new XcodeConfigData();

        [Header("UnityFrameWork")] [SerializeField]
        public XcodeConfigData unityFrameWork = new XcodeConfigData();
    }

    [Serializable]
    public sealed class XcodeConfigData
    {
        [Header("库列表")] [SerializeField] public XcodeConfigChange library = new XcodeConfigChange();
        [Header("框架列表")] [SerializeField] public XcodeConfigChange frameworks = new XcodeConfigChange();
        [Header("属性列表")] [SerializeField] public List<XcodeConfigMap> properties = new List<XcodeConfigMap>();
        [Header("文件复制列表")] [SerializeField] public List<XcodeConfigMap> files = new List<XcodeConfigMap>();
        [Header("文件夹复制列表")] [SerializeField] public List<XcodeConfigMap> folders = new List<XcodeConfigMap>();
        [Header("文件编译设置列表")] [SerializeField] public List<XcodeConfigMap> filesCompileFlag = new List<XcodeConfigMap>();
        [Header("其他链接列表")] [SerializeField] public List<XcodeConfigMap> otherLinkerFlag = new List<XcodeConfigMap>();
    }

    public enum PlistType
    {
        String,
        Boolean,
        Array,
        Dictionary,
    }

    [Serializable]
    public sealed class XcodeConfigPlist
    {
        [Header("键")] [SerializeField] public string key;
        [Header("值类型")] [SerializeField] public PlistType type;
        [Header("字符串值")] [SerializeField] public string stringValue;
        [Header("布尔值")] [SerializeField] public bool boolValue;
        [Header("字典对象")] [SerializeField] public List<XcodeConfigPlist> mapValue = new List<XcodeConfigPlist>();
        [Header("数组对象")] [SerializeField] public List<XcodeConfigMap> arrayValue = new List<XcodeConfigMap>();
    }

    [Serializable]
    public sealed class XcodeConfigMap
    {
        [Header("键")] [SerializeField] public string key;
        [Header("值")] [SerializeField] public string value;
    }

    [Serializable]
    public sealed class XcodeConfigChange
    {
        [Header("增加")] [SerializeField] public string[] add;
        [Header("删除")] [SerializeField] public string[] remove;
    }
}