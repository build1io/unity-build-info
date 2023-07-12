namespace Build1.UnityBuildInfo
{
    public static class BuildNumber
    {
        public static int Get()
        {
            return BuildInfo.Get().BuildNumber;
        }
    }
}