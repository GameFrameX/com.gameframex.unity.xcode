#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GameFrameX.Xcode.Editor
{
    public class SettingLoader
    {
        private const string CHANNEL_ARG = "-channel";

        /// <summary>
        /// 从命令行参数中获取当前渠道标识
        /// </summary>
        public static string GetChannel()
        {
            var args = Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length - 1; i++)
            {
                if (args[i].Equals(CHANNEL_ARG, StringComparison.OrdinalIgnoreCase))
                {
                    return args[i + 1];
                }
            }

            return null;
        }

        /// <summary>
        /// 加载所有匹配的配置文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>文件路径列表</returns>
        public static List<string> LoadSettingsData(string fileName)
        {
            var guids = AssetDatabase.FindAssets($"t:textasset");
            var results = new List<string>(16);
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var newFileName = Path.GetFileName(path);

                // 检查文件名是否完全匹配
                if (newFileName.Equals(fileName, System.StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(path);
                }
            }

            // 排序，保证加载顺序确定
            results.Sort();
            return results;
        }

        /// <summary>
        /// 加载渠道专属配置文件，文件名格式为 XCodeConfig.{channel}.json
        /// </summary>
        /// <param name="baseFileName">基础文件名，如 XCodeConfig.json</param>
        /// <param name="channel">渠道标识</param>
        /// <returns>文件路径列表</returns>
        public static List<string> LoadChannelSettingsData(string baseFileName, string channel)
        {
            if (string.IsNullOrEmpty(channel))
            {
                return new List<string>(0);
            }

            string baseName = Path.GetFileNameWithoutExtension(baseFileName);
            string ext = Path.GetExtension(baseFileName);
            string channelFileName = $"{baseName}.{channel}{ext}";

            var guids = AssetDatabase.FindAssets($"t:textasset");
            var results = new List<string>(4);
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var newFileName = Path.GetFileName(path);

                if (newFileName.Equals(channelFileName, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(path);
                }
            }

            results.Sort();
            return results;
        }
    }
}
#endif
