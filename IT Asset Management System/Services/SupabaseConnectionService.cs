using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using IT_Asset_Management_System.Config;

namespace IT_Asset_Management_System.Services
{
    public class SupabaseConnectionService : ISupabaseConnectionService
    {
        private static readonly HttpClient VerificationHttpClient = new();

        public Supabase.Client? Client { get; private set; }

        public async Task<bool> ConnectAsync(AppConfiguration configuration)
        {
            if (!await VerifyCredentialsAsync(configuration))
            {
                Client = null;
                return false;
            }

            try
            {
                var options = new Supabase.SupabaseOptions { AutoConnectRealtime = false };
                var client = new Supabase.Client(configuration.SupabaseUrl, configuration.SupabasePublishableKey, options);
                await client.InitializeAsync();

                Client = client;
                return true;
            }
            catch
            {
                Client = null;
                return false;
            }
        }

        private static async Task<bool> VerifyCredentialsAsync(AppConfiguration configuration)
        {
            try
            {
                // PostgREST's bare /rest/v1/ root isn't anon-accessible by default (returns 401
                // even with valid credentials); querying a known table is a reliable check instead.
                var requestUrl = $"{configuration.SupabaseUrl.TrimEnd('/')}/rest/v1/assets?select=id&limit=1";

                using var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                request.Headers.Add("apikey", configuration.SupabasePublishableKey);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", configuration.SupabasePublishableKey);

                using var response = await VerificationHttpClient.SendAsync(request);
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }
    }
}
