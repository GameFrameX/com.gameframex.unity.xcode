#if UNITY_IOS
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor.iOS.Xcode;

namespace GameFrameX.Xcode.Editor
{
    internal partial class PostProcessBuildHelper
    {
        /// <summary>
        /// 运行本地化配置
        /// </summary>
        /// <param name="project">PBXProject 实例</param>
        /// <param name="projectGuid">项目 GUID</param>
        /// <param name="path">Xcode 项目路径</param>
        /// <param name="localizations">本地化配置列表</param>
        private static void RunLocalization(PBXProject project, string projectGuid, string path, ArrayList localizations)
        {
            if (localizations == null || localizations.Count == 0)
            {
                return;
            }

            LogHelper.Log("Setting project [Localization] started");

            string pbxprojPath = Path.Combine(path, "Unity-iPhone.xcodeproj/project.pbxproj");

            // 标记是否包含 CFBundleDisplayName 本地化
            bool hasLocalizedDisplayName = false;

            // 收集所有需要本地化的键，用于更新 Info.plist
            // HashSet<string> localizationKeys = new HashSet<string>();

            foreach (Hashtable loc in localizations)
            {
                if (!loc.ContainsKey("languageCode"))
                {
                    continue;
                }

                string langCode = loc["languageCode"].ToString();
                if (string.IsNullOrEmpty(langCode))
                {
                    continue;
                }

                ArrayList validMap = loc["validMap"] as ArrayList;
                if (validMap == null || validMap.Count == 0)
                {
                    continue;
                }

                // Create .lproj directory
                string lprojName = langCode + ".lproj";
                string lprojPath = Path.Combine(path, lprojName);
                if (!Directory.Exists(lprojPath))
                {
                    Directory.CreateDirectory(lprojPath);
                }

                // Prepare InfoPlist.strings content
                StringBuilder sb = new StringBuilder();
                foreach (Hashtable map in validMap)
                {
                    if (map.ContainsKey("key") && map.ContainsKey("value"))
                    {
                        string key = map["key"].ToString();
                        string value = map["value"].ToString();
                        sb.Append($"\"{key}\" = \"{value}\";\n");

                        // 检查是否包含 CFBundleDisplayName
                        if (key == "CFBundleDisplayName")
                        {
                            hasLocalizedDisplayName = true;
                        }

                        // 记录需要本地化的键
                        // localizationKeys.Add(key);
                    }
                }

                if (sb.Length > 0)
                {
                    string fileName = "InfoPlist.strings";
                    string filePath = Path.Combine(lprojPath, fileName);
                    File.WriteAllText(filePath, sb.ToString());

                    LogHelper.Log($"Created Localization file: {filePath}");
                }
            }

            // 更新 Info.plist，将本地化键的值设置为 ${KEY} 格式
            // UpdateInfoPlistForLocalization(path, localizationKeys);

            // 如果存在显示名称本地化，设置 LSHasLocalizedDisplayName 为 true
            if (hasLocalizedDisplayName)
            {
                string plistPath = Path.Combine(path, "Info.plist");
                if (File.Exists(plistPath))
                {
                    PlistDocument plist = new PlistDocument();
                    plist.ReadFromString(File.ReadAllText(plistPath));
                    plist.root.SetBoolean("LSHasLocalizedDisplayName", true);
                    plist.WriteToFile(plistPath);
                    LogHelper.Log("Updated Info.plist: Set LSHasLocalizedDisplayName to true");
                }
            }

            // 使用 PBXProject API 添加本地化文件到项目
            AddLocalizationToProject(project, path, localizations);

            LogHelper.Log("Setting project [Localization] finished");
        }

        /*
        /// <summary>
        /// 更新 Info.plist，将本地化键的值设置为 ${KEY} 格式
        /// 这样 iOS 系统会自动从 InfoPlist.strings 中读取对应语言的值
        /// </summary>
        /// <param name="projectPath">Xcode 项目路径</param>
        /// <param name="localizationKeys">需要本地化的键集合</param>
        private static void UpdateInfoPlistForLocalization(string projectPath, HashSet<string> localizationKeys)
        {
            if (localizationKeys == null || localizationKeys.Count == 0)
            {
                return;
            }

            string plistPath = Path.Combine(projectPath, "Info.plist");
            if (!File.Exists(plistPath))
            {
                LogHelper.Log($"Info.plist not found at: {plistPath}");
                return;
            }

            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));
            PlistElementDict root = plist.root;

            foreach (string key in localizationKeys)
            {
                // 构造本地化引用格式 ${KEY}
                string localizedValue = $"${{{key}}}";

                // 使用 PlistDocument API 设置值
                root.SetString(key, localizedValue);
                LogHelper.Log($"Updated Info.plist key '{key}' to localized format: {localizedValue}");
            }

            plist.WriteToFile(plistPath);
            LogHelper.Log("Info.plist updated with localization references");
        }
        */

        /// <summary>
        /// 添加本地化资源到 Xcode 项目
        /// 使用 PBXProject API 安全地添加本地化文件引用
        /// </summary>
        /// <param name="project">PBXProject 实例</param>
        /// <param name="projectPath">Xcode 项目路径</param>
        /// <param name="localizations">本地化配置列表</param>
        private static void AddLocalizationToProject(PBXProject project, string projectPath, ArrayList localizations)
        {
            string pbxprojPath = Path.Combine(projectPath, "Unity-iPhone.xcodeproj/project.pbxproj");

            // 获取主 target 的 GUID
            string mainTargetGuid = project.GetUnityMainTargetGuid();

            foreach (Hashtable loc in localizations)
            {
                if (!loc.ContainsKey("languageCode"))
                {
                    continue;
                }

                string langCode = loc["languageCode"].ToString();
                if (string.IsNullOrEmpty(langCode))
                {
                    continue;
                }

                string lprojName = langCode + ".lproj";
                string infoPlistStringsPath = Path.Combine(lprojName, "InfoPlist.strings");
                string fullPath = Path.Combine(projectPath, infoPlistStringsPath);

                // 检查文件是否存在
                if (!File.Exists(fullPath))
                {
                    LogHelper.Log($"InfoPlist.strings not found at: {fullPath}");
                    continue;
                }

                // 使用 PBXProject API 添加文件到项目
                string fileGuid = project.AddFile(infoPlistStringsPath, infoPlistStringsPath, PBXSourceTree.Source);

                // 将文件添加到主 target 的 Resources 构建阶段
                project.AddFileToBuild(mainTargetGuid, fileGuid);

                LogHelper.Log($"Added localization file to project: {infoPlistStringsPath}");
            }

            // 保存项目文件
            project.WriteToFile(pbxprojPath);
            LogHelper.Log("Localization files added to Xcode project");
        }
    }
}
#endif
