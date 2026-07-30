using IT_Asset_Management_System.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace IT_Asset_Management_System
{
    public partial class MainWindow : Window
    {
        public double ContentScale { get; private set; } = 1.0;

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new DashboardPage());
            SetActiveButton(DashboardButton);
        }

        public void SetContentScale(double scale)
        {
            ContentScale = scale;
            MainFrame.LayoutTransform = new ScaleTransform(scale, scale);
        }

        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DashboardPage());

            while (MainFrame.CanGoBack)
            {
                MainFrame.RemoveBackEntry();
            }

            SetActiveButton(DashboardButton);
        }

        private void AssetsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AssetsPage());

            while (MainFrame.CanGoBack)
            {
                MainFrame.RemoveBackEntry();
            }

            SetActiveButton(AssetsButton);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new SettingsPage());

            while (MainFrame.CanGoBack)
            {
                MainFrame.RemoveBackEntry();
            }

            SetActiveButton(SettingsButton);
        }

        private void SetActiveButton(Button activeButton)
        {
            DashboardButton.Background = Brushes.Transparent;
            AssetsButton.Background = Brushes.Transparent;
            SettingsButton.Background = Brushes.Transparent;

            DashboardButton.Foreground = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString("#D1D5DB"));

            AssetsButton.Foreground = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString("#D1D5DB"));

            SettingsButton.Foreground = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString("#D1D5DB"));

            activeButton.Background = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString("#2563EB"));

            activeButton.Foreground = Brushes.White;
        }
    }
}
