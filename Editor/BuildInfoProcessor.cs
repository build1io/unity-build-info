#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

namespace Build1.UnityBuildInfo.Editor
{
    [InitializeOnLoad]
    internal abstract class BuildInfoProcessor
    {
        private const string BuildNumberFileFullName = Build1.UnityBuildInfo.BuildInfo.FileName + ".json";
        private const string BuildNumberFolderPath   = "/Resources";
        private const string BuildNumberFilePath     = BuildNumberFolderPath + "/" + BuildNumberFileFullName;

        public static BuildInfoDto BuildInfo { get; private set; }

        static BuildInfoProcessor()
        {
            BuildInfo = ReadBuildInfoFile();

            if (BuildInfo != null) 
                return;
            
            BuildInfo = new BuildInfoDto
            {
                buildNumber = TryGetBuildNumberFromPlayerSettings(out var buildNumber) ? buildNumber : 1,
                isSandboxBuild = false
            };

            UpdateBuildInfoFile();
        }

        /*
         * Build Number.
         */

        public static void Increment()
        {
            Set(Mathf.Max(BuildInfo.BuildNumber + 1, 1));
        }

        public static void Decrement()
        {
            Set(Mathf.Max(BuildInfo.BuildNumber - 1, 1));
        }

        public static void Set(int buildNumber)
        {
            BuildInfo.buildNumber = buildNumber;
            
            UpdateBuildNumberInPlayerSettings();
            UpdateBuildInfoFile();

            Debug.Log($"Build number set to: {buildNumber}");
        }

        public static void Reset()
        {
            Set(1);
        }
        
        /*
         * Is Sandbox Build.
         */

        public static void SetIsSandboxBuild(bool isSandboxBuild)
        {
            BuildInfo.isSandboxBuild = isSandboxBuild;
            
            UpdateBuildInfoFile();
            
            Debug.Log($"IsSandboxBuild set to {isSandboxBuild}");
        }

        /*
         * Private.
         */

        private static bool TryGetBuildNumberFromPlayerSettings(out int buildNumber)
        {
            var target = EditorUserBuildSettings.activeBuildTarget;
            switch (target)
            {
                case BuildTarget.iOS:
                    return int.TryParse(PlayerSettings.iOS.buildNumber, out buildNumber);

                case BuildTarget.Android:
                    buildNumber = PlayerSettings.Android.bundleVersionCode;
                    return true;

                // WebGL doesn't have build number in PlayerSettings.
                case BuildTarget.WebGL:
                    buildNumber = -1;
                    return false;

                // Exception for future platforms.
                default:
                    Debug.LogError($"BuildInfo: Not implemented for build target [{target}]");
                    buildNumber = -1;
                    return false;
            }
        }

        private static void UpdateBuildNumberInPlayerSettings()
        {
            PlayerSettings.iOS.buildNumber = BuildInfo.buildNumber.ToString();
            PlayerSettings.Android.bundleVersionCode = BuildInfo.buildNumber;

            AssetDatabase.SaveAssets();
        }

        /*
         * File.
         */

        private static BuildInfoDto ReadBuildInfoFile()
        {
            var path = Application.dataPath + BuildNumberFilePath;
            if (!File.Exists(path)) 
                return null;
            
            var content = System.IO.File.ReadAllText(path);
            var info = JsonUtility.FromJson<BuildInfoDto>(content);
            return info;
        }

        private static void UpdateBuildInfoFile()
        {
            var folderPath = Application.dataPath + BuildNumberFolderPath;
            if (!System.IO.Directory.Exists(folderPath))
                System.IO.Directory.CreateDirectory(folderPath);
            
            var filePath = Application.dataPath + BuildNumberFilePath;
            var json = JsonUtility.ToJson(BuildInfo);
            System.IO.File.WriteAllText(filePath, json);
        }
    }
}

#endif