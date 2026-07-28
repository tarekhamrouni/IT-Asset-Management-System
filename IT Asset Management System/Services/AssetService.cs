using IT_Asset_Management_System.Models;
using Supabase.Postgrest;

namespace IT_Asset_Management_System.Services
{
    public class AssetService : IAssetService
    {
        private readonly ISupabaseConnectionService _connectionService;

        public AssetService(ISupabaseConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        private Supabase.Client Client => _connectionService.Client
            ?? throw new InvalidOperationException("Supabase connection has not been established.");

        public async Task<List<Asset>> GetAllAsync()
        {
            var response = await Client.From<Asset>().Get();
            return response.Models;
        }

        public async Task<Asset?> GetByIdAsync(Guid id)
        {
            return await Client.From<Asset>()
                .Filter("id", Constants.Operator.Equals, id.ToString())
                .Single();
        }

        public async Task<Asset> AddAsync(Asset asset)
        {
            var response = await Client.From<Asset>().Insert(asset);
            return response.Models.First();
        }

        public async Task<Asset> UpdateAsync(Asset asset)
        {
            var response = await Client.From<Asset>().Update(asset);
            return response.Models.First();
        }

        public async Task DeleteAsync(Guid id)
        {
            await Client.From<Asset>()
                .Filter("id", Constants.Operator.Equals, id.ToString())
                .Delete();
        }
    }
}
