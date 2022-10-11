namespace JRunner
{
    public static class StaticVersion
    {
        public const string Ver = variables.staticversion;

        public static string[] versions = Ver.Split('.');
        public static int Betaversion = int.Parse(versions[0]);
        public static int version = int.Parse(versions[1]);
        public static int Build = int.Parse(versions[2]);
        public static int Dev = int.Parse(versions[3]);
    }
}
