using System.Configuration;
using System.Windows;

namespace nookenApp.Views
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            // Load current settings
            PortNameTextBox.Text = ConfigurationManager.AppSettings["PortName"];
            ApiUrlTextBox.Text = ConfigurationManager.AppSettings["ApiUrl"];
            ConnectionStringTextBox.Text = ConfigurationManager.AppSettings["connectionString"];
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Save settings
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["PortName"].Value = PortNameTextBox.Text;
            config.AppSettings.Settings["ApiUrl"].Value = ApiUrlTextBox.Text;
            config.AppSettings.Settings["connectionString"].Value = ConnectionStringTextBox.Text;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            // Notify MainWindow to update settings
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.ReloadSettings();
            }
            this.Close();
        }
    }
}
