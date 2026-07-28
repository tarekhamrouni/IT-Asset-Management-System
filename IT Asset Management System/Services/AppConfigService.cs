using System.IO;
using System.Text.Json;
using IT_Asset_Management_System.Config;

namespace IT_Asset_Management_System.Services
{
    public class AppConfigService : IAppConfigService
    {
        private readonly string _configFilePath;

        public AppConfigService()
        {
            var appDataFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "IT Asset Management System");

            Directory.CreateDirectory(appDataFolder);
            _configFilePath = Path.Combine(appDataFolder, "config.json");
        }

        public bool IsConfigured => File.Exists(_configFilePath);

        public AppConfiguration? Load()
        {
            if (!File.Exists(_configFilePath))
                return null;

            var json = File.ReadAllText(_configFilePath);
            return JsonSerializer.Deserialize<AppConfiguration>(json);
        }

        public void Save(AppConfiguration configuration)
        {
            var json = JsonSerializer.Serialize(configuration, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_configFilePath, json);
        }
    }
}
