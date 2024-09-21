#if UNITY_IOS
using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace GameFrameX.Xcode.Editor
{
    internal partial class PostProcessBuildHelper
    {
        private static XcodeConfig _xcodeConfig;

        [PostProcessBuild(ushort.MaxValue)]
        public static void OnPostProcessBuild(BuildTarget target, string path)
        {
            if (target != BuildTarget.iOS)
            {
                return;
            }

            try
            {
                _xcodeConfig = SettingLoader.LoadSettingData<XcodeConfig>();
                if (_xcodeConfig == null)
                {
                    return;
                }

                string projectPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
                var project = new PBXProject();
                project.ReadFromString(File.ReadAllText(projectPath));
                // 配置主项目
                Run(project, project.GetUnityMainTargetGuid(), _xcodeConfig.unityMain, path);
                // Unity项目
                Run(project, project.GetUnityFrameworkTargetGuid(), _xcodeConfig.unityFrameWork, path);
                // 保存文件
                File.WriteAllText(projectPath, project.WriteToString());

                RunUnityMain(projectPath);
                RunUnityFramework(projectPath);
                RunPlist(path,_xcodeConfig.plist);
                // 启动环境变量
                RunEnvironmentVariables(path, table.SGet<Hashtable>("environmentVariables"));
                // 运行启动参数
                RunArgument(path, table.SGet("launcherArgs") as ArrayList);
                // PodFile
                RunPodfile(path,table.SGet("podSource") as ArrayList);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        static void Run(PBXProject pbxProject, string targetGuid, XcodeConfigData xcodeConfigData, string path)
        {
            SetBuildProperties(pbxProject,targetGuid, xcodeConfigData.properties);
            // 设置框架
            SetFrameworks(pbxProject,targetGuid,xcodeConfigData.frameworks);
            // 复制文件
            RunCopyFiles(pbxProject, targetGuid, path, hashtable.SGet<Hashtable>("files"));
            // 复制文件夹
            CopyFolders(pbxProject, targetGuid, path, hashtable.SGet<Hashtable>("folders"));
            // 设置文件编译标记
            SetFilesCompileFlag(pbxProject, targetGuid, xcodeConfigData.filesCompileFlag);
            // Linker Flag
            AddOtherLinkFlag(pbxProject, targetGuid, xcodeConfigData.otherLinkerFlag);
        }

        private static void RunUnityMain(string projectPath)
        {
            var project = new PBXProject();
            project.ReadFromString(File.ReadAllText(projectPath));

            string targetGuid = project.GetUnityMainTargetGuid();
            project.AddBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");
            File.WriteAllText(projectPath, project.WriteToString());
            Debug.Log("设置项目[UnityMain]结束");
        }

        private static void RunUnityFramework(string projectPath)
        {
            var project = new PBXProject();
            project.ReadFromString(File.ReadAllText(projectPath));

            string targetGuid = project.GetUnityFrameworkTargetGuid();
            // project.AddFrameworkToProject(targetGuid, "WebKit.framework", false);
            project.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-ObjC");
            project.AddBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");
            project.SetBuildProperty(targetGuid, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");
            project.SetBuildProperty(targetGuid, "GCC_ENABLE_CPP_EXCEPTIONS", "YES");
            project.SetBuildProperty(targetGuid, "CLANG_ENABLE_OBJC_ARC", "YES");
            File.WriteAllText(projectPath, project.WriteToString());
            Debug.Log("设置项目[UnityFramework]结束");
        }
    }
}
#endif