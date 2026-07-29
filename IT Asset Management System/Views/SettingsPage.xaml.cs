using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using IT_Asset_Management_System.Services;

namespace IT_Asset_Management_System.Views
{
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            Loaded += SettingsPage_Loaded;
        }

        private void SettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            HighlightFontButton(mainWindow?.ContentScale ?? 1.0);
            HighlightThemeButton(App.IsDarkTheme);
        }

        private void SmallFontButton_Click(object sender, RoutedEventArgs e) => SetFontSize(0.9);
        private void MediumFontButton_Click(object sender, RoutedEventArgs e) => SetFontSize(1.0);
        private void LargeFontButton_Click(object sender, RoutedEventArgs e) => SetFontSize(1.15);

        private void SetFontSize(double scale)
        {
            (Window.GetWindow(this) as MainWindow)?.SetContentScale(scale);
            HighlightFontButton(scale);
        }

        private void HighlightFontButton(double scale)
        {
            SetOptionButtonSelected(SmallFontButton, scale == 0.9);
            SetOptionButtonSelected(MediumFontButton, scale == 1.0);
            SetOptionButtonSelected(LargeFontButton, scale == 1.15);
        }

        private void LightThemeButton_Click(object sender, RoutedEventArgs e) => SetTheme(false);
        private void DarkThemeButton_Click(object sender, RoutedEventArgs e) => SetTheme(true);

        private void SetTheme(bool isDark)
        {
            ((App)Application.Current).ApplyTheme(isDark);
            HighlightThemeButton(isDark);
        }

        private void HighlightThemeButton(bool isDark)
        {
            SetOptionButtonSelected(LightThemeButton, !isDark);
            SetOptionButtonSelected(DarkThemeButton, isDark);
        }

        private static void SetOptionButtonSelected(Button button, bool isSelected)
        {
            if (isSelected)
            {
                button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2563EB"));
                button.Foreground = Brushes.White;
            }
            else
            {
                // Revert to the style's DynamicResource setters so these keep following
                // theme changes instead of holding a brush reference from the old theme.
                button.ClearValue(Button.BackgroundProperty);
                button.ClearValue(Button.ForegroundProperty);
            }
        }

        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            var existingConfiguration = new AppConfigService().Load();
            AppSession.AssetService = null;

            var setupWindow = new SetupWindow(existingConfiguration);
            setupWindow.Show();

            Window.GetWindow(this)?.Close();
        }

        private void ChangeDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            new AppConfigService().Clear();
            AppSession.AssetService = null;

            var setupWindow = new SetupWindow();
            setupWindow.Show();

            Window.GetWindow(this)?.Close();
        }
    }
}
