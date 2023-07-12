using UnityEngine;

namespace Build1.UnityBuildInfo
{
    [System.Serializable]
    public static class BuildInfo
    {
        internal const string FileName = "build-info";

        /*
         * Static.
         */

        private static IBuildInfo _info;

        public static IBuildInfo Get()
        {
            _info ??= Load();
            return _info;
        }

        private static IBuildInfo Load()
        {
            var json = Resources.Load<TextAsset>(FileName);
            return json != null ? JsonUtility.FromJson<BuildInfoDto>(json.text) : new BuildInfoDto();
        }
    }
}