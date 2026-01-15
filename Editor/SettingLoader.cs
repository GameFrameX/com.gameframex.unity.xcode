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
    }
}
#endif
