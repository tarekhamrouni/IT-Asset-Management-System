using IT_Asset_Management_System.Config;

namespace IT_Asset_Management_System.Services
{
    public interface IAppConfigService
    {
        bool IsConfigured { get; }
        AppConfiguration? Load();
        void Save(AppConfiguration configuration);
    }
}
