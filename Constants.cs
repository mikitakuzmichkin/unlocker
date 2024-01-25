namespace PuzzleUnlocker.Editor
{
    public static class Constants
    {
        public const string BUNDLE_ID =
#if UNITY_ANDROID
            "com.dainty.tictactoe";
#elif UNITY_IOS
            //IOS BUNDLE ID
#endif

        public const string RATE_US_URL =
#if UNITY_ANDROID
            "http://play.google.com/store/apps/details?id=" + BUNDLE_ID;
#elif UNITY_IOS
            //IOS LINK
#endif

        public const string URL_MORE_GAMES =
#if UNITY_ANDROID
            "https://play.google.com/store/apps/developer?id=DAINTY";
#elif UNITY_IOS
            //IOS LINK
#endif

        public const string URL_PRIVACY_POLICY = "https://daintygames.github.io/privacy";
        public const string URL_EULA = "https://daintygames.github.io/terms";
    }
}