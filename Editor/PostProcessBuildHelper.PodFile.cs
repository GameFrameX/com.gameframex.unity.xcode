#if UNITY_IOS
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEditor;
using UnityEditor.iOS.Xcode;

namespace GameFrameX.Xcode.Editor
{
    internal partial class PostProcessBuildHelper
    {
        /// <summary>
        /// 修改PodFile 文件的源
        /// </summary>
        /// <param name="path">项目路径</param>
        /// <param name="arrayList">源列表</param>
        private static void RunPodfile(string path, ArrayList arrayList)
        {
            LogHelper.Log("修改PodFile 文件的源,  开始");

            if (arrayList == null || arrayList.Count <= 0)
            {
                LogHelper.Log("[PodFile] 参数为空或数据为空，跳过设置");
                return;
            }

            string podfilePath = path + "/Podfile";
            if (!File.Exists(podfilePath))
            {
                LogHelper.Warning("当前不是Pod 项目结构,跳过设置");
                return;
            }

            Thread.Sleep(3000);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var source in arrayList)
            {
                stringBuilder.AppendLine($"source '{source}'");
            }

            var readAllLines = File.ReadAllLines(podfilePath);
            foreach (var line in readAllLines)
            {
                if (!line.Trim().StartsWith("source"))
                {
                    stringBuilder.AppendLine(line);
                }
            }

            File.WriteAllText(podfilePath, stringBuilder.ToString());
            LogHelper.Log("修改PodFile 文件的源,  结束");
        }

        private static void EnsurePodfileExists(string path, ArrayList podSource)
        {
            var podfilePath = path + "/Podfile";
            if (File.Exists(podfilePath)) return;

            var iosVersion = PlayerSettings.iOS.targetOSVersionString;
            if (string.IsNullOrEmpty(iosVersion))
            {
                iosVersion = "12.0";
            }

            var sourceBuilder = new StringBuilder();
            if (podSource != null && podSource.Count > 0)
            {
                foreach (var source in podSource)
                {
                    sourceBuilder.AppendLine($"source '{source}'");
                }
            }
            else
            {
                sourceBuilder.AppendLine("source 'https://github.com/CocoaPods/Specs.git'");
            }

            File.WriteAllText(podfilePath,
                sourceBuilder.ToString() +
                $"platform :ios, '{iosVersion}'\n" +
                "\n" +
                "target 'Unity-iPhone' do\n" +
                "end\n" +
                "\n" +
                "target 'UnityFramework' do\n" +
                "end\n");
            LogHelper.Log($"[Pods] 自动创建 Podfile (iOS {iosVersion})");
        }

        private static void AddPodsForTarget(string path, string targetName, Hashtable pods)
        {
            if (pods == null || pods.Count <= 0) return;

            var podfilePath = path + "/Podfile";
            if (!File.Exists(podfilePath)) return;

            var lines = new List<string>(File.ReadAllLines(podfilePath));

            // 收集已有 pod 名称用于去重
            var existingPods = new System.Collections.Generic.HashSet<string>();
            foreach (var line in lines)
            {
                var trimmed = line.Trim();
                if (trimmed.StartsWith("pod "))
                {
                    var podContent = trimmed.Substring(4).Trim();
                    if (podContent.StartsWith("'") || podContent.StartsWith("\""))
                    {
                        var quoteEnd = podContent.IndexOf(podContent[0], 1);
                        if (quoteEnd > 0)
                        {
                            existingPods.Add(podContent.Substring(1, quoteEnd - 1));
                        }
                    }
                }
            }

            // 查找 target 块
            int targetIndex = -1;
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Trim().StartsWith("target") && lines[i].Contains($"'{targetName}'") && lines[i].TrimEnd().EndsWith("do"))
                {
                    targetIndex = i;
                    break;
                }
            }

            if (targetIndex < 0)
            {
                LogHelper.Warning($"[Pods] 未找到 target '{targetName}' do, 跳过设置");
                return;
            }

            // 构建 pod 行
            var podLines = new System.Collections.Generic.List<string>();
            foreach (DictionaryEntry kv in pods)
            {
                var podName = kv.Key.ToString().Trim();
                if (existingPods.Contains(podName)) continue;

                var version = kv.Value?.ToString()?.Trim();
                if (string.IsNullOrEmpty(version))
                {
                    podLines.Add($"  pod '{podName}'");
                }
                else if (version.Contains("=>") || version.StartsWith(":"))
                {
                    podLines.Add($"  pod '{podName}', {version}");
                }
                else
                {
                    podLines.Add($"  pod '{podName}', '{version}'");
                }
            }

            if (podLines.Count == 0) return;

            lines.InsertRange(targetIndex + 1, podLines);
            File.WriteAllLines(podfilePath, lines.ToArray());
            LogHelper.Log($"[Pods] 已为 {targetName} 添加 {podLines.Count} 个 pod 依赖");
        }
    }
}
#endif