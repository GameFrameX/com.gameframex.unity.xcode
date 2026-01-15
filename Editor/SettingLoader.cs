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
        /// 加载所有匹配的配置文件
        /// </summary>
        /// <param name="fileName">文件名(可包含扩展名，若无则默认.json)</param>
        /// <returns>文件路径列表</returns>
        public static List<string> LoadSettingsData(string fileName)
        {
            var guids = AssetDatabase.FindAssets($"t:textasset");
            var results = new List<string>(16);

            string extension = Path.GetExtension(fileName);
            string namePart = Path.GetFileNameWithoutExtension(fileName);

            if (string.IsNullOrEmpty(extension))
            {
                extension = ".json";
            }

            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var newFileName = Path.GetFileName(path);

                // 检查扩展名是否匹配
                if (!newFileName.EndsWith(extension, System.StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                // 检查文件名是否包含指定的部分
                if (newFileName.IndexOf(namePart, System.StringComparison.OrdinalIgnoreCase) >= 0)
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
