#if UNITY_IOS
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GameFrameX.Xcode.Editor
{
    public class SettingLoader
    {
        /// <summary>
        /// 加载相关的配置文件
        /// </summary>
        public static string LoadSettingData(string fileName)
        {
            var guids = AssetDatabase.FindAssets($"t:textasset");

            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);

                var newFileName = Path.GetFileName(path);
                if (fileName == newFileName)
                {
                    return path;
                }
            }

            File.WriteAllText(fileName, "");
            return null;
        }

        /// <summary>
        /// 加载所有匹配的配置文件
        /// </summary>
        /// <param name="fileNamePart">文件名包含的部分</param>
        /// <returns>文件路径列表</returns>
        public static List<string> LoadSettingDatas(string fileNamePart)
        {
            var guids = AssetDatabase.FindAssets($"t:textasset");
            var results = new List<string>(16);

            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var newFileName = Path.GetFileName(path);

                // 检查文件名是否包含 fileNamePart (忽略大小写)，且以 .json 结尾
                if (newFileName.IndexOf(fileNamePart, System.StringComparison.OrdinalIgnoreCase) >= 0 &&
                    newFileName.EndsWith(".json", System.StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(path);
                }
            }

            // 排序，保证加载顺序确定
            results.Sort();
            return results;
        }
    }
}
#endif
