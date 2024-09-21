#if UNITY_IOS
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GameFrameX.Xcode.Editor
{
    internal partial class PostProcessBuildHelper
    {
        /// <summary>
        /// 修改PodFile 文件的源
        /// </summary>
        /// <param name="path"></param>
        private static async void RunPodfile(string path)
        {
            await Task.Delay(TimeSpan.FromSeconds(3));
            string podfilePath = path + "/Podfile";

            if (!File.Exists(podfilePath))
            {
                return;
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("source 'https://mirrors.tuna.tsinghua.edu.cn/git/CocoaPods/Specs.git'");
            var readAllLines = File.ReadAllLines(podfilePath);
            foreach (var line in readAllLines)
            {
                if (!line.Trim().StartsWith("source"))
                {
                    stringBuilder.AppendLine(line);
                }
            }

            File.WriteAllText(podfilePath, stringBuilder.ToString());
        }
    }
}
#endif