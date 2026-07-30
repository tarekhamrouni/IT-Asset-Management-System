using IT_Asset_Management_System.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Threading.Tasks;
using IT_Asset_Management_System.Services;


namespace IT_Asset_Management_System.Views
{
    public partial class AddAssetWindow : Window
    {
        public AddAssetWindow()
        {
            InitializeComponent();

            TypeComboBox.SelectionChanged += TypeComboBox_SelectionChanged;
            CancelButton.Click += CancelButton_Click;
            SaveButton.Click += SaveButton_Click;

            LoadDropdowns();
        }

        private void LoadDropdowns()
        {
            TypeComboBox.ItemsSource = Enum.GetValues(typeof(AssetType));
            StatusComboBox.ItemsSource = Enum.GetValues(typeof(AssetStatus));

            TypeComboBox.SelectedItem = AssetType.Laptop;
            StatusComboBox.SelectedItem = AssetStatus.Available;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorTextBlock.Text = "";

            if (TypeComboBox.SelectedItem is not AssetType selectedType)
            {
                ErrorTextBlock.Text = "Please select an asset type.";
                return;
            }

            if (StatusComboBox.SelectedItem is not AssetStatus selectedStatus)
            {
                ErrorTextBlock.Text = "Please select a status.";
                return;
            }

            if (string.IsNullOrWhiteSpace(BrandTextBox.Text))
            {
                ErrorTextBlock.Text = "Brand is required.";
                BrandTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(ModelTextBox.Text))
            {
                ErrorTextBlock.Text = "Model is required.";
                ModelTextBox.Focus();
                return;
            }

            if (AppSession.AssetService == null)
            {
                ErrorTextBlock.Text = "The asset service is not available.";
                return;
            }

            try
            {
                SaveButton.IsEnabled = false;
                SaveButton.Content = "Saving...";

                var asset = new Asset
                {
                    AssetTag = AssetTagTextBox.Text,
                    Type = selectedType,
                    Brand = BrandTextBox.Text.Trim(),
                    Model = ModelTextBox.Text.Trim(),
                    SerialNumber = NullIfEmpty(SerialNumberTextBox.Text),
                    Status = selectedStatus,
                    Department = NullIfEmpty(DepartmentTextBox.Text),
                    AssignedTo = NullIfEmpty(AssignedToTextBox.Text),

                    PurchaseDate = PurchaseDatePicker.SelectedDate.HasValue
                        ? DateOnly.FromDateTime(PurchaseDatePicker.SelectedDate.Value)  
                        : null,

                    WarrantyExpires = WarrantyExpiryDatePicker.SelectedDate.HasValue
                        ? DateOnly.FromDateTime(WarrantyExpiryDatePicker.SelectedDate.Value)
                        : null
                };

                await AppSession.AssetService.AddAsync(asset);

                DialogResult = true;
                Close();
            }
            catch (Exception exception)
            {
                ErrorTextBlock.Text = $"Could not save asset: {exception.Message}";
            }
            finally
            {
                SaveButton.IsEnabled = true;
                SaveButton.Content = "Save Asset";
            }
        }

        private static string? NullIfEmpty(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? null
                : value.Trim();
        }

        private async void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TypeComboBox.SelectedItem is not AssetType selectedType)
                return;

            AssetTagTextBox.Text = "Generating...";

            try
            {
                AssetTagTextBox.Text = await GenerateNextAssetTagAsync(selectedType);
            }
            catch
            {
                AssetTagTextBox.Text = GetAssetPrefix(selectedType) + "-ERROR";
            }
        }

        private async Task<string> GenerateNextAssetTagAsync(AssetType type)
        {
            if (AppSession.AssetService == null)
            {
                throw new InvalidOperationException(
                    "The asset service has not been initialized.");
            }

            string prefix = GetAssetPrefix(type);

            var assets = await AppSession.AssetService.GetAllAsync();

            int highestNumber = assets
                .Where(asset =>
                    !string.IsNullOrWhiteSpace(asset.AssetTag) &&
                    asset.AssetTag.StartsWith(prefix + "-"))
                .Select(asset =>
                {
                    string numberPart = asset.AssetTag.Substring(prefix.Length + 1);

                    return int.TryParse(numberPart, out int number)
                        ? number
                        : 0;
                })
                .DefaultIfEmpty(0)
                .Max();

            return $"{prefix}-{highestNumber + 1:D4}";
        }

        private static string GetAssetPrefix(AssetType type)
        {
            return type switch
            {
                AssetType.Laptop => "LAP",
                AssetType.Desktop => "DES",
                AssetType.Monitor => "MON",
                AssetType.MobilePhone => "PHO",
                AssetType.Tablet => "TAB",
                AssetType.Keyboard => "KEY",
                AssetType.Headset => "HED",
                AssetType.Printer => "PRI",
                AssetType.Server => "SER",
                AssetType.NetworkDevice => "NET",
                _ => "AST"
            };
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}