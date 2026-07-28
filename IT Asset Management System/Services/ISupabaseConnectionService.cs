using IT_Asset_Management_System.Config;

namespace IT_Asset_Management_System.Services
{
    public interface ISupabaseConnectionService
    {
        Supabase.Client? Client { get; }

        // Verifies the credentials against Supabase before initializing Client; Client stays null on failure.
        Task<bool> ConnectAsync(AppConfiguration configuration);
    }
}
