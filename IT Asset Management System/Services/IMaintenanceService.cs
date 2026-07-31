using IT_Asset_Management_System.Models;

namespace IT_Asset_Management_System.Services
{
    public interface IMaintenanceService
    {
        Task<List<MaintenanceRecord>> GetAllAsync();
        Task<List<MaintenanceRecord>> GetByAssetIdAsync(Guid assetId);
        Task<MaintenanceRecord> AddAsync(MaintenanceRecord record);
        Task<MaintenanceRecord> UpdateAsync(MaintenanceRecord record);
        Task DeleteAsync(Guid id);
    }
}
