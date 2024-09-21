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
                Run(project, project.GetUnityMainTargetGuid(), table.SGet<Hashtable>("unityMain"), path);
                // Unity项目
                Run(project, project.GetUnityFrameworkTargetGuid(), table.SGet<Hashtable>("unityFramework"), path);
                // 保存文件
                File.WriteAllText(projectPath, project.WriteToString());

                RunUnityMain(projectPath);
                RunUnityFramework(projectPath);
                RunPlist(project, path, table.SGet<Hashtable>("plist"));
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
        }
    }
}
#endif