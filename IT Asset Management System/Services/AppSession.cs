namespace IT_Asset_Management_System.Services
{
    // Holds the services created once SetupWindow establishes a Supabase connection,
    // so other windows/pages (created without a DI container) can reach them.
    public static class AppSession
    {
        public static IAssetService? AssetService { get; set; }
    }
}
