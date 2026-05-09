#if UNITY_IOS
using System;
using System.Collections;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace GameFrameX.Xcode.Editor
{
    internal partial class PostProcessBuildHelper
    {
        /// <summary>
        /// 设置Capabilities
        /// </summary>
        /// <param name="pbxProject">PBX项目</param>
        /// <param name="targetGuid">目标GUID</param>
        /// <param name="path">项目路径</param>
        /// <param name="hashtable">配置数据</param>
        static void SetCapabilities(PBXProject pbxProject, string targetGuid, string path, Hashtable hashtable)
        {
            try
            {
                if (hashtable == null)
                {
                    return;
                }

                Debug.Log("开始设置Capabilities");

                var projectCapabilityManager = new ProjectCapabilityManager(path + "/Unity-iPhone.xcodeproj/project.pbxproj", "gameframex.entitlements", null, targetGuid);

                // In-App Purchase
                if (hashtable.ContainsKey("inAppPurchase") && hashtable["inAppPurchase"] is bool inAppPurchase && inAppPurchase)
                {
                    projectCapabilityManager.AddInAppPurchase();
                    Debug.Log("已添加 In-App Purchase Capability");
                }

                // Game Center
                if (hashtable.ContainsKey("gameCenter") && hashtable["gameCenter"] is bool gameCenter && gameCenter)
                {
                    projectCapabilityManager.AddGameCenter();
                    Debug.Log("已添加 Game Center Capability");
                }

                // Push Notifications
                if (hashtable.ContainsKey("pushNotifications") && hashtable["pushNotifications"] is bool pushNotifications && pushNotifications)
                {
                    projectCapabilityManager.AddPushNotifications(false);
                    Debug.Log("已添加 Push Notifications Capability");
                }

                // Sign In with Apple
                if (hashtable.ContainsKey("signInWithApple") && hashtable["signInWithApple"] is bool signInWithApple && signInWithApple)
                {
                    projectCapabilityManager.AddSignInWithApple();
                    Debug.Log("已添加 Sign In with Apple Capability");
                }

                // Background Modes
                if (hashtable.ContainsKey("backgroundModes"))
                {
                    var backgroundModes = hashtable["backgroundModes"] as ArrayList;
                    if (backgroundModes != null && backgroundModes.Count > 0)
                    {
                        var modes = new BackgroundModesOptions();
                        foreach (var modeObj in backgroundModes)
                        {
                            string mode = modeObj?.ToString()?.ToLower();
                            if (string.IsNullOrEmpty(mode))
                            {
                                continue;
                            }

                            switch (mode)
                            {
                                case "audio":
                                    modes |= BackgroundModesOptions.AudioAirplayPiP;
                                    break;
                                case "location":
                                    modes |= BackgroundModesOptions.LocationUpdates;
                                    break;
                                case "voip":
                                    modes |= BackgroundModesOptions.VoiceOverIP;
                                    break;
                                case "newsstand":
                                    modes |= BackgroundModesOptions.NewsstandDownloads;
                                    break;
                                case "external":
                                    modes |= BackgroundModesOptions.ExternalAccessoryCommunication;
                                    break;
                                case "bluetooth":
                                    modes |= BackgroundModesOptions.UsesBluetoothLEAccessory;
                                    break;
                                case "bluetooth-peripheral":
                                    modes |= BackgroundModesOptions.ActsAsABluetoothLEAccessory;
                                    break;
                                case "fetch":
                                    modes |= BackgroundModesOptions.BackgroundFetch;
                                    break;
                                case "remote-notification":
                                    modes |= BackgroundModesOptions.RemoteNotifications;
                                    break;
                            }
                        }

                        projectCapabilityManager.AddBackgroundModes(modes);
                        Debug.Log($"已添加 Background Modes Capability: {string.Join(", ", backgroundModes.ToArray())}");
                    }
                }

                // iCloud
                if (hashtable.ContainsKey("iCloud"))
                {
                    var iCloudConfig = hashtable["iCloud"] as Hashtable;
                    if (iCloudConfig != null)
                    {
                        bool enableKeyValueStorage = iCloudConfig.ContainsKey("keyValueStorage") && iCloudConfig["keyValueStorage"] is bool kvStorage && kvStorage;
                        bool enableiCloudDocument = iCloudConfig.ContainsKey("iCloudDocument") && iCloudConfig["iCloudDocument"] is bool iCloudDoc && iCloudDoc;

                        var customContainers = iCloudConfig["customContainers"] as ArrayList;
                        string[] containers = null;
                        if (customContainers != null && customContainers.Count > 0)
                        {
                            containers = new string[customContainers.Count];
                            for (int i = 0; i < customContainers.Count; i++)
                            {
                                containers[i] = customContainers[i].ToString();
                            }
                        }

                        projectCapabilityManager.AddiCloud(enableKeyValueStorage, enableiCloudDocument, containers);
                        Debug.Log("已添加 iCloud Capability");
                    }
                }

                // App Groups
                if (hashtable.ContainsKey("appGroups"))
                {
                    var appGroups = hashtable["appGroups"] as ArrayList;
                    if (appGroups != null && appGroups.Count > 0)
                    {
                        string[] groups = new string[appGroups.Count];
                        for (int i = 0; i < appGroups.Count; i++)
                        {
                            groups[i] = appGroups[i].ToString();
                        }

                        projectCapabilityManager.AddAppGroups(groups);
                        Debug.Log($"已添加 App Groups Capability: {string.Join(", ", groups)}");
                    }
                }

                // Associated Domains
                if (hashtable.ContainsKey("associatedDomains"))
                {
                    var domains = hashtable["associatedDomains"] as ArrayList;
                    if (domains != null && domains.Count > 0)
                    {
                        string[] domainArray = new string[domains.Count];
                        for (int i = 0; i < domains.Count; i++)
                        {
                            domainArray[i] = domains[i].ToString();
                        }

                        projectCapabilityManager.AddAssociatedDomains(domainArray);
                        Debug.Log($"已添加 Associated Domains Capability: {string.Join(", ", domainArray)}");
                    }
                }

                projectCapabilityManager.WriteToFile();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}
#endif