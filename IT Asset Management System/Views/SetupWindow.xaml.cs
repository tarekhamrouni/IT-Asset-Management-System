using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IT_Asset_Management_System.Config;
using IT_Asset_Management_System.Services;

namespace IT_Asset_Management_System.Views
{
    /// <summary>
    /// Interaction logic for SetupWindow.xaml
    /// </summary>
    public partial class SetupWindow : Window
    {
        private readonly ISupabaseConnectionService _connectionService = new SupabaseConnectionService();

        public SetupWindow() : this(null)
        {
        }

        public SetupWindow(AppConfiguration? existingConfiguration)
        {
            InitializeComponent();

            if (existingConfiguration is not null)
            {
                ProjectUrlTextBox.Text = existingConfiguration.SupabaseUrl;
                PublishableKeyBox.Password = existingConfiguration.SupabasePublishableKey;
            }
        }

        private void ShowKeyCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            PublishableKeyTextBox.Text = PublishableKeyBox.Password;
            PublishableKeyBox.Visibility = Visibility.Collapsed;
            PublishableKeyTextBox.Visibility = Visibility.Visible;
        }

        private void ShowKeyCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            PublishableKeyBox.Password = PublishableKeyTextBox.Text;
            PublishableKeyTextBox.Visibility = Visibility.Collapsed;
            PublishableKeyBox.Visibility = Visibility.Visible;
        }

        private async void SaveContinueButton_Click(object sender, RoutedEventArgs e)
        {
            var url = ProjectUrlTextBox.Text.Trim();
            var key = PublishableKeyTextBox.Visibility == Visibility.Visible
                ? PublishableKeyTextBox.Text.Trim()
                : PublishableKeyBox.Password.Trim();

            if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(key))
            {
                StatusText.Text = "Please enter both the project URL and key.";
                StatusText.Foreground = Brushes.Firebrick;
                return;
            }

            SaveContinueButton.IsEnabled = false;
            StatusText.Text = "Connecting...";
            StatusText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6B7280"));

            var configuration = new AppConfiguration
            {
                SupabaseUrl = url,
                SupabasePublishableKey = key
            };

            var connected = await _connectionService.ConnectAsync(configuration);

            if (!connected)
            {
                StatusText.Text = "Could not connect. Check your project URL and key and try again.";
                StatusText.Foreground = Brushes.Firebrick;
                SaveContinueButton.IsEnabled = true;
                return;
            }

            new AppConfigService().Save(configuration);
            AppSession.AssetService = new AssetService(_connectionService);

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            this.Close();
        }
    }


}
