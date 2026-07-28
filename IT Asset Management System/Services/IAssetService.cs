using IT_Asset_Management_System.Models;

namespace IT_Asset_Management_System.Services
{
    public interface IAssetService
    {
        Task<List<Asset>> GetAllAsync();
        Task<Asset?> GetByIdAsync(Guid id);
        Task<Asset> AddAsync(Asset asset);
        Task<Asset> UpdateAsync(Asset asset);
        Task DeleteAsync(Guid id);
    }
}
