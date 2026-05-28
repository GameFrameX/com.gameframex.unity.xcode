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

        private static bool CopyPodfile(string buildPath, string podfileConfigPath)
        {
            if (string.IsNullOrEmpty(podfileConfigPath))
            {
                return false;
            }

            var trimmedPath = podfileConfigPath.Trim();
            string src = Path.IsPathRooted(trimmedPath)
                ? trimmedPath
                : Path.Combine(Directory.GetParent(UnityEngine.Application.dataPath).FullName, trimmedPath);

            if (!File.Exists(src))
            {
                LogHelper.Warning($"[Pods] 配置指定的 Podfile 不存在: {src}");
                return false;
            }

            string dest = buildPath + "/Podfile";
            File.Copy(src, dest, true);
            LogHelper.Log($"[Pods] 已从 {podfileConfigPath} 复制 Podfile 到构建输出");
            return true;
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

        private static void RunPodInstall(string path)
        {
            var podfilePath = path + "/Podfile";
            if (!File.Exists(podfilePath))
            {
                LogHelper.Log("[PodInstall] Podfile 不存在，跳过 pod install");
                return;
            }

            LogHelper.Log("[PodInstall] 开始执行 pod install...");

            // Unity 子进程不继承 shell PATH，需要解析 pod 的绝对路径
            string podPath = GetPodFullPath();
            if (string.IsNullOrEmpty(podPath))
            {
                LogHelper.Error("[PodInstall] 未找到 pod 命令，请确认已安装 CocoaPods (sudo gem install cocoapods)");
                return;
            }

            LogHelper.Log($"[PodInstall] 使用 pod 路径: {podPath}");

            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = podPath;
            process.StartInfo.Arguments = "install";
            process.StartInfo.WorkingDirectory = path;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            // CocoaPods 要求 UTF-8 编码，Unity 子进程默认不带这些环境变量
            process.StartInfo.EnvironmentVariables["LANG"] = "en_US.UTF-8";
            process.StartInfo.EnvironmentVariables["LC_ALL"] = "en_US.UTF-8";

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (!string.IsNullOrEmpty(output))
            {
                LogHelper.Log($"[PodInstall] {output}");
            }

            if (process.ExitCode != 0)
            {
                LogHelper.Error($"[PodInstall] pod install 失败 (退出码: {process.ExitCode}): {error}");
                return;
            }

            if (!string.IsNullOrEmpty(error))
            {
                LogHelper.Warning($"[PodInstall] {error}");
            }

            LogHelper.Log("[PodInstall] pod install 完成");
        }

        private static string GetPodFullPath()
        {
            // 常见的 pod 安装路径，按优先级尝试
            string[] candidates = new string[]
            {
                "/opt/homebrew/bin/pod",
                "/usr/local/bin/pod",
                System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("HOME") ?? "", ".gem/ruby/bin/pod"),
                "/usr/bin/pod",
            };

            foreach (var candidate in candidates)
            {
                if (System.IO.File.Exists(candidate))
                {
                    return candidate;
                }
            }

            // 尝试通过 shell 解析
            try
            {
                var which = new System.Diagnostics.Process();
                which.StartInfo.FileName = "/bin/zsh";
                which.StartInfo.Arguments = "-lc \"which pod\"";
                which.StartInfo.UseShellExecute = false;
                which.StartInfo.RedirectStandardOutput = true;
                which.StartInfo.CreateNoWindow = true;
                which.Start();
                string result = which.StandardOutput.ReadToEnd().Trim();
                which.WaitForExit();

                if (!string.IsNullOrEmpty(result) && System.IO.File.Exists(result))
                {
                    return result;
                }
            }
            catch
            {
                // ignored
            }

            return null;
        }
    }
}
#endif