#if UNITY_IOS
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
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

            // 使用 PBXProjectExtensions 添加本地化支持
            AddLocalizationToProject(path, localizations);

            LogHelper.Log("Setting project [Localization] finished");
        }

        /// <summary>
        /// 添加本地化资源到 Xcode 项目
        /// 这个方法直接操作 project.pbxproj 文件来添加 VariantGroup
        /// </summary>
        private static void AddLocalizationToProject(string projectPath, ArrayList localizations)
        {
            string pbxprojPath = Path.Combine(projectPath, "Unity-iPhone.xcodeproj/project.pbxproj");
            string pbxContent = File.ReadAllText(pbxprojPath);

            // 生成唯一的 GUID
            string variantGroupGuid = GenerateGuid();
            string variantGroupName = "InfoPlist.strings";

            StringBuilder childrenBuilder = new StringBuilder();
            StringBuilder fileRefsBuilder = new StringBuilder();
            StringBuilder buildFileBuilder = new StringBuilder();

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

                string fileRefGuid = GenerateGuid();
                string lprojName = langCode + ".lproj";
                string relativePath = $"{lprojName}/InfoPlist.strings";

                // 添加到 children 列表
                childrenBuilder.AppendLine($"\t\t\t\t{fileRefGuid} /* {langCode} */,");

                // 添加 PBXFileReference
                fileRefsBuilder.AppendLine($"\t\t{fileRefGuid} /* {langCode} */ = {{isa = PBXFileReference; lastKnownFileType = text.plist.strings; name = {langCode}; path = {relativePath}; sourceTree = \"<group>\"; }};");
            }

            // 创建 PBXVariantGroup
            string variantGroupSection = $@"
/* Begin PBXVariantGroup section */
		{variantGroupGuid} /* {variantGroupName} */ = {{
			isa = PBXVariantGroup;
			children = (
{childrenBuilder.ToString().TrimEnd()}
			);
			name = {variantGroupName};
			sourceTree = ""<group>"";
		}};
/* End PBXVariantGroup section */";

            // 检查是否已经有 PBXVariantGroup section
            if (pbxContent.Contains("/* Begin PBXVariantGroup section */"))
            {
                // 在现有的 section 中添加
                pbxContent = pbxContent.Replace(
                    "/* End PBXVariantGroup section */",
                    $@"		{variantGroupGuid} /* {variantGroupName} */ = {{
			isa = PBXVariantGroup;
			children = (
{childrenBuilder.ToString().TrimEnd()}
			);
			name = {variantGroupName};
			sourceTree = ""<group>"";
		}};
/* End PBXVariantGroup section */");
            }
            else
            {
                // 在 PBXSourcesBuildPhase section 之前插入新的 section
                pbxContent = pbxContent.Replace(
                    "/* Begin PBXSourcesBuildPhase section */",
                    variantGroupSection + "\n/* Begin PBXSourcesBuildPhase section */");
            }

            // 添加 PBXFileReference 条目
            string fileRefInsert = fileRefsBuilder.ToString();
            pbxContent = pbxContent.Replace(
                "/* End PBXFileReference section */",
                fileRefInsert + "/* End PBXFileReference section */");

            // 将 VariantGroup 添加到主 Group 的 children 中
            // 查找 CustomTemplate 或 Unity-iPhone 组并添加引用
            string mainGroupPattern = @"(mainGroup = )([A-F0-9]+)";
            Match mainGroupMatch = Regex.Match(pbxContent, mainGroupPattern);
            if (mainGroupMatch.Success)
            {
                string mainGroupGuid = mainGroupMatch.Groups[2].Value;
                // 找到这个 group 并添加 variant group 到 children
                string groupPattern = $@"({mainGroupGuid} \/\* .* \*\/ = \{{\s*isa = PBXGroup;\s*children = \()";
                pbxContent = Regex.Replace(pbxContent, groupPattern,
                                           $"$1\n\t\t\t\t{variantGroupGuid} /* {variantGroupName} */,");
            }

            // 添加到 Resources build phase
            string targetGuid = GetUnityMainTargetGuidFromContent(pbxContent);
            if (!string.IsNullOrEmpty(targetGuid))
            {
                string buildFileGuid = GenerateGuid();

                // 添加 PBXBuildFile
                string buildFileEntry = $"\t\t{buildFileGuid} /* {variantGroupName} in Resources */ = {{isa = PBXBuildFile; fileRef = {variantGroupGuid} /* {variantGroupName} */; }};\n";
                pbxContent = pbxContent.Replace(
                    "/* End PBXBuildFile section */",
                    buildFileEntry + "/* End PBXBuildFile section */");

                // 添加到 Resources build phase
                string resourcesPhasePattern = @"(isa = PBXResourcesBuildPhase;[^}]*files = \()";
                pbxContent = Regex.Replace(pbxContent, resourcesPhasePattern,
                                           $"$1\n\t\t\t\t{buildFileGuid} /* {variantGroupName} in Resources */,");
            }

            // 添加 knownRegions
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

                // 检查是否已有这个区域
                if (!pbxContent.Contains($"\"{langCode}\"") && !pbxContent.Contains($"{langCode},"))
                {
                    string knownRegionsPattern = @"(knownRegions = \()";
                    pbxContent = Regex.Replace(pbxContent, knownRegionsPattern,
                                               $"$1\n\t\t\t\t{langCode},");
                }
            }

            File.WriteAllText(pbxprojPath, pbxContent);
            LogHelper.Log("Added localization variant groups to project.pbxproj");
        }

        /// <summary>
        /// 生成一个 Xcode 风格的 24 位十六进制 GUID
        /// </summary>
        private static string GenerateGuid()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 24).ToUpper();
        }

        /// <summary>
        /// 从 pbxproj 内容中获取 Unity-iPhone target 的 GUID
        /// </summary>
        private static string GetUnityMainTargetGuidFromContent(string content)
        {
            // 查找 Unity-iPhone target
            string pattern = @"([A-F0-9]+) \/\* Unity-iPhone \*\/ = \{\s*isa = PBXNativeTarget";
            Match match = Regex.Match(content, pattern);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return null;
        }
    }
}
#endif