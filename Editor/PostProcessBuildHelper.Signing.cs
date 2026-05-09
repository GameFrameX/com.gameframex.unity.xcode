#if UNITY_IOS
using System.Collections;
using UnityEditor.iOS.Xcode;

namespace GameFrameX.Xcode.Editor
{
    internal partial class PostProcessBuildHelper
    {
        /// <summary>
        /// 设置签名配置（Team ID、包名、签名身份等）
        /// </summary>
        /// <param name="proj">PBX项目</param>
        /// <param name="targetGuid">目标GUID</param>
        /// <param name="signing">签名配置数据</param>
        private static void SetSigning(PBXProject proj, string targetGuid, Hashtable signing)
        {
            if (signing == null) return;

            var teamId = signing.Get<string>("teamId");
            if (!string.IsNullOrEmpty(teamId))
            {
                proj.SetBuildProperty(targetGuid, "DEVELOPMENT_TEAM", teamId);
            }

            var bundleId = signing.Get<string>("bundleId");
            if (!string.IsNullOrEmpty(bundleId))
            {
                proj.SetBuildProperty(targetGuid, "PRODUCT_BUNDLE_IDENTIFIER", bundleId);
            }

            var codeSignIdentity = signing.Get<string>("codeSignIdentity");
            if (!string.IsNullOrEmpty(codeSignIdentity))
            {
                proj.SetBuildProperty(targetGuid, "CODE_SIGN_IDENTITY", codeSignIdentity);
            }

            var codeSignStyle = signing.Get<string>("codeSignStyle");
            if (!string.IsNullOrEmpty(codeSignStyle))
            {
                proj.SetBuildProperty(targetGuid, "CODE_SIGN_STYLE", codeSignStyle);
            }

            var provisioningProfileSpecifier = signing.Get<string>("provisioningProfileSpecifier");
            if (!string.IsNullOrEmpty(provisioningProfileSpecifier))
            {
                proj.SetBuildProperty(targetGuid, "PROVISIONING_PROFILE_SPECIFIER", provisioningProfileSpecifier);
            }
        }
    }
}
#endif
