namespace Build1.UnityBuildInfo
{
    public interface IBuildInfo
    {
        int  BuildNumber    { get; }
        bool IsSandboxBuild { get; }
    }
}