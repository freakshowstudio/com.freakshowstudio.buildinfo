
using System.IO;
using UnityEngine;


namespace FreakshowStudio.BuildInfo.Runtime
{
    internal sealed class BuildInformation : ScriptableObject
    {
        [field: SerializeField]
        public string AppVersion { get; set; } = string.Empty;

        [field: SerializeField]
        public int BuildNumber { get; set; }

        [field: SerializeField]
        public string BuildTime { get; set; } = string.Empty;

        [field: SerializeField]
        public string BuildRevision { get; set; } = string.Empty;

        private const string ResourceFolder = "Assets/Resources";

        private static readonly string AbsoluteResourceFolder =
            Path.Combine(
                Application.dataPath,
                "..",
                ResourceFolder);

        private const string ResourceName = "BuildInformation";
        private const string ResourceFilename = ResourceName + ".asset";

        private static readonly string RelativeResourcePath =
            Path.Combine(ResourceFolder, ResourceFilename);

        public static BuildInformation? GetOrCreate()
        {
            var buildInfo = Resources.Load<BuildInformation>(ResourceName);
            if (buildInfo != null) return buildInfo;

#if !UNITY_EDITOR
            return null;
#else
            buildInfo = CreateInstance<BuildInformation>();
            Directory.CreateDirectory(AbsoluteResourceFolder);
            UnityEditor.AssetDatabase.CreateAsset(
                buildInfo,
                RelativeResourcePath);
            return buildInfo;
#endif
        }

        public void Save()
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssetIfDirty(this);
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
    }
}
