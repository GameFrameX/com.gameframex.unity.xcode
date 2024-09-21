#if UNITY_IOS
using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using UnityEditor.iOS.Xcode;

namespace GameFrameX.Xcode.Editor
{
    internal partial class PostProcessBuildHelper
    {
        /// <summary>
        /// 设置项目[XcScheme-Argument]
        /// </summary>
        /// <param name="path">项目路径</param>
        /// <param name="arrayList"></param>
        private static async void RunArgument(string path, ArrayList arrayList)
        {
            LogHelper.Log("设置项目[XcScheme-Argument]开始");
            if (arrayList == null || arrayList.Count <= 0)
            {
                LogHelper.Log("[XcScheme-Argument] 参数为空或数据为空，跳过设置");
                return;
            }

            await Task.Delay(TimeSpan.FromSeconds(1));
            string projectPath = path + "/Unity-iPhone.xcodeproj/xcshareddata/xcschemes/Unity-iPhone.xcscheme";
            XcScheme xcScheme = new XcScheme();
            xcScheme.ReadFromFile(projectPath);
            foreach (var argument in arrayList)
            {
                xcScheme.AddArgumentPassedOnLaunch(argument.ToString());
            }

            xcScheme.WriteToFile(projectPath);
            LogHelper.Log("设置项目[XcScheme-Argument]结束");
        }

        /// <summary>
        /// 添加环境变量
        /// </summary>
        /// <param name="path">项目路径</param>
        /// <param name="map"></param>
        private static async void RunEnvironmentVariables(string path, Hashtable map)
        {
            LogHelper.Log("设置项目[XcScheme-EnvironmentVariables]开始");
            if (map == null || map.Count <= 0)
            {
                LogHelper.Log("[XcScheme-EnvironmentVariables] 参数为空或数据为空，跳过设置");
                return;
            }

            await Task.Delay(TimeSpan.FromSeconds(1));
            foreach (DictionaryEntry entry in map)
            {
                AddEnvironmentVariablesPassedOnLaunch(path, entry.Key.ToString(), entry.Value.ToString());
            }

            // AddEnvironmentVariablesPassedOnLaunch(path, "IDEPreferLogStreaming", "YES");
            // AddEnvironmentVariablesPassedOnLaunch(path, "OS_ACTIVITY_MODE", "disable");
            LogHelper.Log("设置项目[XcScheme-EnvironmentVariables]结束");
        }

        /// <summary>
        /// 添加环境变量
        /// </summary>
        /// <param name="path">项目路径</param>
        /// <param name="key">环境变量KEY</param>
        /// <param name="value">环境变量值</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static void AddEnvironmentVariablesPassedOnLaunch(string path, string key, string value)
        {
            string projectPath = path + "/Unity-iPhone.xcodeproj/xcshareddata/xcschemes/Unity-iPhone.xcscheme";
            string text = File.ReadAllText(projectPath);
            var mDoc = XDocument.Load(XmlReader.Create(new StringReader(text), new XmlReaderSettings()
            {
                ProhibitDtd = false,
                XmlResolver = null
            }));
            if (mDoc.Root != null)
            {
                XElement node = mDoc.Root.XPathSelectElement("./LaunchAction");
                XElement xElement;
                if (node != null)
                {
                    xElement = node.XPathSelectElement("./EnvironmentVariables");
                }
                else
                {
                    throw new Exception("The xcscheme document does not contain build configuration setting");
                }

                if (xElement == null)
                {
                    node.Add(new XElement("EnvironmentVariables"));
                    xElement = node.XPathSelectElement("./EnvironmentVariables");
                }

                xElement?.Add(new XElement("EnvironmentVariable", new object[3]
                {
                    new XAttribute("key", key),
                    new XAttribute("value", value),
                    new XAttribute("isEnabled", "YES")
                }));
            }

            mDoc.Save(projectPath);
        }
    }
}
#endif