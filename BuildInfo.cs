using System;
using UnityEngine;

namespace Build1.UnityBuildInfo
{
    public static class BuildInfo
    {
        private const string FileName                = "build-info";
        private const string BuildNumberFileFullName = FileName + ".json";

        internal const string BuildNumberFolderPath = "/Resources";
        internal const string BuildNumberFilePath   = BuildNumberFolderPath + "/" + BuildNumberFileFullName;

        private static IBuildInfo _info;

        public static IBuildInfo Get(bool reload = false)
        {
            if (reload)
                _info = Load();
            else
                _info ??= Load();

            return _info;
        }

        public static IBuildInfo Set(int buildNumber, bool isSandbox)
        {
            if (Application.isPlaying)
                throw new NotSupportedException("Build number and mode modification not available in runtime");

            var dto = new BuildInfoDto
            {
                buildNumber = buildNumber,
                isSandboxBuild = isSandbox
            };

            var folderPath = Application.dataPath + BuildNumberFolderPath;
            if (!System.IO.Directory.Exists(folderPath))
                System.IO.Directory.CreateDirectory(folderPath);

            var filePath = Application.dataPath + BuildNumberFilePath;
            var json = JsonUtility.ToJson(dto);
            System.IO.File.WriteAllText(filePath, json);

            return dto;
        }

        private static IBuildInfo Load()
        {
            var json = Resources.Load<TextAsset>(FileName);
            return json != null ? JsonUtility.FromJson<BuildInfoDto>(json.text) : new BuildInfoDto();
        }
    }
}