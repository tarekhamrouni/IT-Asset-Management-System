using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using IT_Asset_Management_System.Models;

namespace IT_Asset_Management_System.Views
{
    public partial class DashboardPage : Page
    {
        private readonly List<Asset> _mockAssets;

        public DashboardPage()
        {
            InitializeComponent();

            _mockAssets = CreateMockAssets();
            LoadDashboard();
        }

        private void LoadDashboard()
        {
            TotalAssetsText.Text = _mockAssets.Count.ToString();

            AvailableAssetsText.Text = _mockAssets
                .Count(asset => asset.Status == AssetStatus.Available)
                .ToString();

            AssignedAssetsText.Text = _mockAssets
                .Count(asset => asset.Status == AssetStatus.Assigned)
                .ToString();

            RepairAssetsText.Text = _mockAssets
                .Count(asset => asset.Status == AssetStatus.InRepair)
                .ToString();

            RecentAssetsGrid.ItemsSource = _mockAssets.Take(8);

            DateOnly warningDate = DateOnly.FromDateTime(DateTime.Today.AddDays(90));

            WarrantyGrid.ItemsSource = _mockAssets
                .Where(asset =>
                    asset.WarrantyExpires.HasValue &&
                    asset.WarrantyExpires.Value <= warningDate)
                .OrderBy(asset => asset.WarrantyExpires);
        }

        private static List<Asset> CreateMockAssets()
        {
            return new List<Asset>
            {
                new Asset
                {
                    Id = Guid.NewGuid(),
                    AssetTag = "LAP-0042",
                    Type = AssetType.Laptop,
                    Brand = "Dell",
                    Model = "Latitude 5540",
                    SerialNumber = "ABC12345",
                    Status = AssetStatus.Assigned,
                    AssignedTo = "Erik Johansson",
                    Department = "Support",
                    WarrantyExpires = DateOnly.FromDateTime(DateTime.Today.AddDays(25)),
                    Location = "Stockholm Office"
                },
                new Asset
                {
                    Id = Guid.NewGuid(),
                    AssetTag = "MON-0015",
                    Type = AssetType.Monitor,
                    Brand = "LGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG",
                    Model = "UltraWide 34",
                    SerialNumber = "MON88921",
                    Status = AssetStatus.Available,
                    WarrantyExpires = DateOnly.FromDateTime(DateTime.Today.AddDays(70)),
                    Location = "Storage Room"
                },
                new Asset
                {
                    Id = Guid.NewGuid(),
                    AssetTag = "PHO-0008",
                    Type = AssetType.MobilePhone,
                    Brand = "Apple",
                    Model = "iPhone 13",
                    SerialNumber = "IPH22311",
                    Status = AssetStatus.InRepair,
                    WarrantyExpires = DateOnly.FromDateTime(DateTime.Today.AddDays(140)),
                    Location = "Repair Centre"
                },
                new Asset
                {
                    Id = Guid.NewGuid(),
                    AssetTag = "LAP-0043",
                    Type = AssetType.Laptop,
                    Brand = "Lenovo",
                    Model = "ThinkPad T14",
                    SerialNumber = "LEN99182",
                    Status = AssetStatus.Available,
                    WarrantyExpires = DateOnly.FromDateTime(DateTime.Today.AddDays(45)),
                    Location = "Stockholm Office"
                },
                new Asset
                {
                    Id = Guid.NewGuid(),
                    AssetTag = "DES-0012",
                    Type = AssetType.Desktop,
                    Brand = "HP",
                    Model = "EliteDesk 800",
                    SerialNumber = "HP882910",
                    Status = AssetStatus.Assigned,
                    AssignedTo = "Sara Nilsson",
                    Department = "Finance",
                    WarrantyExpires = DateOnly.FromDateTime(DateTime.Today.AddDays(200)),
                    Location = "Stockholm Office"
                }
                ,                new Asset
                {
                    Id = Guid.NewGuid(),
                    AssetTag = "DES-0012",
                    Type = AssetType.Desktop,
                    Brand = "HP",
                    Model = "EliteDesk 800",
                    SerialNumber = "HP882910",
                    Status = AssetStatus.Assigned,
                    AssignedTo = "Sara Nilsson",
                    Department = "Finance",
                    WarrantyExpires = DateOnly.FromDateTime(DateTime.Today.AddDays(20)),
                    Location = "Stockholm Office"
                },                new Asset
                {
                    Id = Guid.NewGuid(),
                    AssetTag = "DES-0012",
                    Type = AssetType.Desktop,
                    Brand = "HP",
                    Model = "EliteDesk 800",
                    SerialNumber = "HP882910",
                    Status = AssetStatus.Assigned,
                    AssignedTo = "Sara Nilsson",
                    Department = "Finance",
                    WarrantyExpires = DateOnly.FromDateTime(DateTime.Today.AddDays(20)),
                    Location = "Stockholm Office"
                },                new Asset
                {
                    Id = Guid.NewGuid(),
                    AssetTag = "DES-0012",
                    Type = AssetType.Desktop,
                    Brand = "HP",
                    Model = "EliteDesk 800",
                    SerialNumber = "HP882910",
                    Status = AssetStatus.Assigned,
                    AssignedTo = "Sara Nilsson",
                    Department = "Finance",
                    WarrantyExpires = DateOnly.FromDateTime(DateTime.Today.AddDays(200)),
                    Location = "Stockholm Office"
                },
                new Asset
                {
                    Id = Guid.NewGuid(),
                    AssetTag = "DES-0012",
                    Type = AssetType.Desktop,
                    Brand = "HP",
                    Model = "EliteDesk 800",
                    SerialNumber = "HP882910",
                    Status = AssetStatus.Assigned,
                    AssignedTo = "Sara Nilsson",
                    Department = "Finance",
                    WarrantyExpires = DateOnly.FromDateTime(DateTime.Today.AddDays(15)),
                    Location = "Stockholm Office"
                },
                new Asset
                {
                    Id = Guid.NewGuid(),
                    AssetTag = "DES-0012",
                    Type = AssetType.Desktop,
                    Brand = "HP",
                    Model = "EliteDesk 800",
                    SerialNumber = "HP882910",
                    Status = AssetStatus.Assigned,
                    AssignedTo = "Sara Nilsson",
                    Department = "Finance",
                    WarrantyExpires = DateOnly.FromDateTime(DateTime.Today.AddDays(20)),
                    Location = "Stockholm Office"
                },
                new Asset
                {
                    Id = Guid.NewGuid(),
                    AssetTag = "DES-0012",
                    Type = AssetType.Desktop,
                    Brand = "HP",
                    Model = "EliteDesk 800",
                    SerialNumber = "HP882910",
                    Status = AssetStatus.Assigned,
                    AssignedTo = "Sara Nilsson",
                    Department = "Finance",
                    WarrantyExpires = DateOnly.FromDateTime(DateTime.Today.AddDays(40)),
                    Location = "Stockholm Office"
                }
            };
        }
    }
}