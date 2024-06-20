#if UNITY_EDITOR && UNITY_WEBGL

using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Build1.UnityBuildInfo.Editor.WebGL
{
    public sealed class BuildNumberBuildProcessorWebGL : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            var path = Path.Join(report.summary.outputPath, "index.html");
            if (File.Exists(path))
                File.Delete(path);
        }

        [PostProcessBuild(1)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target != BuildTarget.WebGL)
                return;

            var path = Path.Join(pathToBuiltProject, "index.html");
            var buildNumber = BuildInfo.Get(true).BuildNumber;
            var contents = File.ReadAllText(path).Replace("@BUILD_NUMBER@", buildNumber.ToString());

            File.WriteAllText(path, contents);

            Debug.Log($"Build number updated in the index.html. Build number set: {buildNumber}");
        }
    }
}

#endif