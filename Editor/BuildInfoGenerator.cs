
using System;
using System.IO;
using BuildInfo.Runtime;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#if HAS_UCB_ENVIRONMENT
using UcbEnvironment.Editor;
#endif

namespace BuildInfo.Editor
{
    internal sealed class GenerateBuildInfo : IPreprocessBuildWithReport
    {
        // Make sure the build number has a chance to be set by other
        // scripts before this runs.
        public int callbackOrder => 1000;

        public void OnPreprocessBuild(BuildReport report)
        {
            var bi = BuildInformation.GetOrCreate();
            if (bi == null) throw new FileNotFoundException(
                "Could not find build info");

            bi.AppVersion = PlayerSettings.bundleVersion;
            bi.BuildNumber = GetBuildNumber();
            bi.BuildTime = DateTime.Now.ToString("O");
#if HAS_UCB_ENVIRONMENT
            if (bool.TryParse(
                    UnityCloudBuildEnvironment.IsBuilder,
                    out var isBuilder)
                && isBuilder)
            {
                bi.BuildRevision = UnityCloudBuildEnvironment.BuildRevision;
            }
#endif
        }

        private int GetBuildNumber()
        {
#if UNITY_IOS
            return int.TryParse(
                PlayerSettings.iOS.buildNumber,
                out var buildNumber)
            ? buildNumber
            : 0;
#elif UNITY_ANDROID
            return PlayerSettings.Android.bundleVersionCode;
#else
            return 0;
#endif
        }
    }
}
