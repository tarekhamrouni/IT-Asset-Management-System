using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using IT_Asset_Management_System.Models;
using IT_Asset_Management_System.Services;

namespace IT_Asset_Management_System.Views
{
    public partial class DashboardPage : Page
    {
        public DashboardPage()
        {
            InitializeComponent();
            Loaded += DashboardPage_Loaded;
        }

        private async void DashboardPage_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadDashboardAsync();
        }

        private async Task LoadDashboardAsync()
        {
            var assets = await AppSession.AssetService!.GetAllAsync();

            TotalAssetsText.Text = assets.Count.ToString();

            AvailableAssetsText.Text = assets
                .Count(asset => asset.Status == AssetStatus.Available)
                .ToString();

            AssignedAssetsText.Text = assets
                .Count(asset => asset.Status == AssetStatus.Assigned)
                .ToString();

            RepairAssetsText.Text = assets
                .Count(asset => asset.Status == AssetStatus.InRepair)
                .ToString();

            RecentAssetsGrid.ItemsSource = assets.Take(8);

            DateOnly warningDate = DateOnly.FromDateTime(DateTime.Today.AddDays(90));

            WarrantyGrid.ItemsSource = assets
                .Where(asset =>
                    asset.WarrantyExpires.HasValue &&
                    asset.WarrantyExpires.Value <= warningDate)
                .OrderBy(asset => asset.WarrantyExpires);
        }
    }
}
