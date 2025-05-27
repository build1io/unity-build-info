#if UNITY_EDITOR

using UnityEditor;

namespace Build1.UnityBuildInfo.Editor
{
    internal static class BuildInfoMenu
    {
        [MenuItem("Tools/Build1/Build Info/Increment Build Number", false, 1070)]
        public static void Increment()
        {
            BuildInfoProcessor.Increment();
        }
        
        [MenuItem("Tools/Build1/Build Info/Decrement Build Number", false, 1071)]
        public static void Decrement()
        {
            BuildInfoProcessor.Decrement();
        }

        [MenuItem("Tools/Build1/Build Info/Reset Build Number", false, 1120)]
        public static void Reset()
        {
            var currentBuildNumber = BuildInfo.Get().BuildNumber;
            var result = EditorUtility.DisplayDialog("Reset Build Number?",
                                        $"Are you sure you want to reset build number?\n\nCurrent build number: {currentBuildNumber}",
                                        "Reset",
                                        "Cancel");
            if (result)
                BuildInfoProcessor.Reset();
        }
        
        [MenuItem("Tools/Build1/Build Info/Tool Window...", false, 1150)]
        public static void ToolsWindow()
        {
            BuildInfoWindow.Open();
        }
    }
}

#endif