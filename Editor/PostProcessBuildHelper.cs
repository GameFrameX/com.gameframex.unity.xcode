#if UNITY_IOS
using System;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace GameFrameX.Xcode.Editor
{
    internal partial class PostProcessBuildHelper
    {
        [PostProcessBuild(ushort.MaxValue)]
        public static void OnPostProcessBuild(BuildTarget target, string path)
        {
            if (target != BuildTarget.iOS)
            {
                return;
            }

            try
            {
                //读取配置文件
                var jsonPaths = SettingLoader.LoadSettingsData("XCodeConfig.json");

                // 如果没有找到任何匹配的文件，直接返回
                if (jsonPaths.Count == 0)
                {
                    LogHelper.Error("未找到任何 XCodeConfig 相关配置文件, 跳过设置");
                    return;
                }

                // 合并所有配置文件的内容
                Hashtable finalConfig = new Hashtable();
                foreach (var jsonPath in jsonPaths)
                {
                    LogHelper.Log($"[MergeConfig] 正在合并配置: {jsonPath}");
                    string json = File.ReadAllText(jsonPath);
                    Hashtable table = json.HashtableFromJson();
                    if (table == null)
                    {
                        LogHelper.Error($"{jsonPath} 解析失败, 跳过合并");
                        continue;
                    }

                    // 使用扩展方法合并 Hashtable
                    finalConfig.Merge(table);
                }

                if (finalConfig.Count == 0)
                {
                    LogHelper.Error("合并后的配置为空，跳过设置");
                    return;
                }

                string projectPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
                var project = new PBXProject();
                project.ReadFromString(File.ReadAllText(projectPath));

                // 第一阶段：应用所有配置到 PBXProject
                LogHelper.Log("[PBXProject] 正在应用最终合并配置...");
                // 配置主项目
                Run(project, project.GetUnityMainTargetGuid(), finalConfig.Get<Hashtable>("unityMain"), path);
                // Unity项目
                Run(project, project.GetUnityFrameworkTargetGuid(), finalConfig.Get<Hashtable>("unityFramework"), path);

                // 设置签名配置（只在主项目上设置）
                SetSigning(project, project.GetUnityMainTargetGuid(), finalConfig.Get<Hashtable>("signing"));

                // 保存 PBXProject
                File.WriteAllText(projectPath, project.WriteToString());

                // 第二阶段：应用其他配置 (Plist, Env, Args, Pod, Capabilities)
                LogHelper.Log("[OtherSettings] 正在应用最终合并配置...");

                // 设置Info.Plist
                RunPlist(project, path, finalConfig.Get<Hashtable>("plist"));
                // 启动环境变量
                RunEnvironmentVariables(path, finalConfig.Get<Hashtable>("environmentVariables"));
                // 运行启动参数
                RunArgument(path, finalConfig.Get("launcherArgs") as ArrayList);

                // 设置Capabilities (只在主项目上设置)
                SetCapabilities(project, project.GetUnityMainTargetGuid(), path, finalConfig.Get<Hashtable>("capabilities"));

                // Capabilities 通过 ProjectCapabilityManager 独立写盘，需要重新读取以保留其修改
                project = new PBXProject();
                project.ReadFromString(File.ReadAllText(projectPath));

                // Localization
                RunLocalization(project, project.ProjectGuid(), path, finalConfig.Get("localizations") as ArrayList);

                // PodFile
                RunPodfile(path, finalConfig.Get("podSource") as ArrayList);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }


        static void Run(PBXProject pbxProject, string targetGuid, Hashtable hashtable, string path)
        {
            // 设置构建属性
            SetBuildProperties(pbxProject, targetGuid, hashtable.Get<Hashtable>("properties"));
            // 设置框架
            SetFrameworks(pbxProject, targetGuid, hashtable.Get<Hashtable>("frameworks"));
            // 复制文件
            RunCopyFiles(pbxProject, targetGuid, path, hashtable.Get<Hashtable>("files"));
            // 复制文件夹
            CopyFolders(pbxProject, targetGuid, path, hashtable.Get<Hashtable>("folders"));
            // 设置文件编译标记
            SetFilesCompileFlag(pbxProject, targetGuid, hashtable.Get<Hashtable>("filesCompileFlag"));
            // Linker Flag
            AddOtherLinkFlag(pbxProject, targetGuid, hashtable.Get<Hashtable>("otherLinkerFlag"));
            // Run Path Search Paths
            AddRunPathSearchPaths(pbxProject, targetGuid, hashtable.Get<Hashtable>("runPathSearchPaths"));
        }
    }
}
#endif