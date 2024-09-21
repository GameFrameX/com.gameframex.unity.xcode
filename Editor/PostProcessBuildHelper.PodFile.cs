#if UNITY_IOS
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameFrameX.Xcode.Editor
{
    internal partial class PostProcessBuildHelper
    {
        /// <summary>
        /// 修改PodFile 文件的源
        /// </summary>
        /// <param name="path"></param>
        /// <param name="arrayList"></param>
        private static async void RunPodfile(string path, ArrayList arrayList)
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

            await Task.Delay(TimeSpan.FromSeconds(3));

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
    }
}
#endif