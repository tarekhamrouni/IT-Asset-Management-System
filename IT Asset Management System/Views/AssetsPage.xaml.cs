using IT_Asset_Management_System.Models;
using IT_Asset_Management_System.Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;

namespace IT_Asset_Management_System.Views
{
    public partial class AssetsPage : Page
    {
        private List<Asset> _assets = new();

        public AssetsPage()
        {
            InitializeComponent();

            SearchTextBox.TextChanged += SearchTextBox_TextChanged;
            TypeFilterComboBox.SelectionChanged += TypeFilterComboBox_SelectionChanged;
            StatusFilterComboBox.SelectionChanged += StatusFilterComboBox_SelectionChanged;
            AddAssetButton.Click += AddAssetButton_Click;

            Loaded += AssetsPage_Loaded;
        }

        private async void AssetsPage_Loaded(object sender, RoutedEventArgs e)
        {

            await LoadAssetsAsync();
        }

        private async void AddAssetButton_Click(object sender, RoutedEventArgs e)
        {
            var addAssetWindow = new AddAssetWindow
            {
                Owner = Window.GetWindow(this)
            };

            bool? result = addAssetWindow.ShowDialog();

            if (result == true)
            {
                await LoadAssetsAsync();
                await ShowSuccessToastAsync("Asset added successfully.");
            }
        }

        private async Task LoadAssetsAsync()
        {
            try
            {
                LoadingStateText.Visibility = Visibility.Visible;
                EmptyStatePanel.Visibility = Visibility.Collapsed;
                AssetsGrid.IsEnabled = false;

                if (AppSession.AssetService == null)
                {
                    throw new InvalidOperationException(
                        "The asset service has not been initialized.");
                }

                _assets = await AppSession.AssetService.GetAllAsync();

                ApplySearchTypeAndStatusFilter();
            }
            catch (Exception exception)
            {
                _assets = new List<Asset>();
                AssetsGrid.ItemsSource = _assets;

                EmptyStateTitle.Text = "Could not load assets";
                EmptyStateMessage.Text = exception.Message;
                EmptyStatePanel.Visibility = Visibility.Visible;
            }
            finally
            {
                LoadingStateText.Visibility = Visibility.Collapsed;
                AssetsGrid.IsEnabled = true;
            }
        }

        private void TypeFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplySearchTypeAndStatusFilter();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplySearchTypeAndStatusFilter();
        }

        private void StatusFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplySearchTypeAndStatusFilter();
        }

        private void ApplySearchTypeAndStatusFilter()
        {
            string searchText =
                SearchTextBox.Text.Trim().ToLower();

            string selectedType =
                (TypeFilterComboBox.SelectedItem as ComboBoxItem)?
                .Content?.ToString() ?? "All types";

            string selectedStatus =
                (StatusFilterComboBox.SelectedItem as ComboBoxItem)?
                .Content?.ToString() ?? "All statuses";

            var results = _assets.Where(asset =>
            {
                bool matchesSearch =
                    string.IsNullOrWhiteSpace(searchText) ||
                    asset.AssetTag.ToLower().Contains(searchText) ||
                    asset.Type.ToString().ToLower().Contains(searchText) ||
                    asset.Brand.ToLower().Contains(searchText) ||
                    asset.Model.ToLower().Contains(searchText) ||
                    asset.SerialNumber.ToLower().Contains(searchText) ||
                    asset.Status.ToString().ToLower().Contains(searchText) ||
                    (asset.AssignedTo?.ToLower().Contains(searchText) ?? false) ||
                    (asset.Department?.ToLower().Contains(searchText) ?? false) ||
                    (asset.Location?.ToLower().Contains(searchText) ?? false);

                bool matchesType =
                    selectedType == "All types" ||
                    asset.Type.ToString()
                        .Replace(" ", "")
                        .Equals(
                            selectedType.Replace(" ", ""),
                            StringComparison.OrdinalIgnoreCase);

                bool matchesStatus =
                    selectedStatus == "All statuses" ||
                    asset.Status.ToString()
                        .Replace(" ", "")
                        .Equals(
                            selectedStatus.Replace(" ", ""),
                            StringComparison.OrdinalIgnoreCase);

                return matchesSearch &&
                       matchesType &&
                       matchesStatus;
            }).ToList();

            AssetsGrid.ItemsSource = results;
            UpdateEmptyState(results.Count);
        }

        private void UpdateEmptyState(int resultCount)
        {
            bool hasNoAssets = _assets.Count == 0;
            bool hasNoResults = resultCount == 0;

      
            EmptyStatePanel.Visibility = hasNoResults
                ? System.Windows.Visibility.Visible
                : System.Windows.Visibility.Collapsed;

            if (!hasNoResults)
                return;

            if (hasNoAssets)
            {
                EmptyStateTitle.Text = "No assets found";
                EmptyStateMessage.Text = "Add your first asset to get started.";
            }
            else
            {
                EmptyStateTitle.Text = "No matching assets";
                EmptyStateMessage.Text = "Try changing your search or filters.";
            }
        }

        private async Task ShowSuccessToastAsync(string message)
        {
            SuccessToastText.Text = message;
            SuccessToast.Visibility = Visibility.Visible;

            await Task.Delay(4500);

            SuccessToast.Visibility = Visibility.Collapsed;
        }


    }
}