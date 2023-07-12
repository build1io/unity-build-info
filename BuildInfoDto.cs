using System;

namespace Build1.UnityBuildInfo
{
    [Serializable]
    internal sealed class BuildInfoDto : IBuildInfo
    {
        public int  BuildNumber    => buildNumber;
        public bool IsSandboxBuild => isSandboxBuild;

        public int  buildNumber;
        public bool isSandboxBuild;
    }
}