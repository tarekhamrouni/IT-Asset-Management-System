using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Media;

namespace IT_Asset_Management_System
{
 
    public partial class App : Application
    {
        public static bool IsDarkTheme { get; private set; }

        public void ApplyTheme(bool isDark)
        {
            IsDarkTheme = isDark;

            SetBrush("WindowBackgroundBrush", isDark ? "#111827" : "#F5F7FA");
            SetBrush("SurfaceBrush", isDark ? "#1F2937" : "#FFFFFF");
            SetBrush("PrimaryTextBrush", isDark ? "#F3F4F6" : "#111827");
            SetBrush("SecondaryTextBrush", isDark ? "#9CA3AF" : "#6B7280");
            SetBrush("BorderBrush", isDark ? "#374151" : "#E5E7EB");
            SetBrush("AlternatingSurfaceBrush", isDark ? "#242E3D" : "#F7F8FA");
        }

        private void SetBrush(string resourceKey, string hexColor)
        {
            
            Resources[resourceKey] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
        }
    }
}
