using IT_Asset_Management_System.Models;
using Supabase.Postgrest;

namespace IT_Asset_Management_System.Services
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly ISupabaseConnectionService _connectionService;

        public MaintenanceService(ISupabaseConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        private Supabase.Client Client => _connectionService.Client
            ?? throw new InvalidOperationException("Supabase connection has not been established.");

        public async Task<List<MaintenanceRecord>> GetAllAsync()
        {
            var response = await Client.From<MaintenanceRecord>().Get();
            return response.Models;
        }

        public async Task<List<MaintenanceRecord>> GetByAssetIdAsync(Guid assetId)
        {
            var response = await Client.From<MaintenanceRecord>()
                .Filter("asset_id", Constants.Operator.Equals, assetId.ToString())
                .Order("service_date", Constants.Ordering.Descending)
                .Get();

            return response.Models;
        }

        public async Task<MaintenanceRecord> AddAsync(MaintenanceRecord record)
        {
            var response = await Client.From<MaintenanceRecord>().Insert(record);
            return response.Models.First();
        }

        public async Task<MaintenanceRecord> UpdateAsync(MaintenanceRecord record)
        {
            var response = await Client.From<MaintenanceRecord>().Update(record);
            return response.Models.First();
        }

        public async Task DeleteAsync(Guid id)
        {
            await Client.From<MaintenanceRecord>()
                .Filter("id", Constants.Operator.Equals, id.ToString())
                .Delete();
        }
    }
}
