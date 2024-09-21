#if UNITY_IOS
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
    }
}
#endif