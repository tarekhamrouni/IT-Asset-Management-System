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

namespace IT_Asset_Management_System.Views
{
    /// <summary>
    /// Interaction logic for SetupWindow.xaml
    /// </summary>
    public partial class SetupWindow : Window
    {
        public SetupWindow()
        {
            InitializeComponent();
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

        private void SaveContinueButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            this.Close();
        }
    }


}
