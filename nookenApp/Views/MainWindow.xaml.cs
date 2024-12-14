using Microsoft.EntityFrameworkCore;
using nookenApp.Helper;
using System.IO.Ports;
using System.Windows;

namespace nookenApp.Views
{
    public partial class MainWindow : Window
    {
        private SensorDataWindow sensorDataWindow;

        public MainWindow()
        {
            InitializeComponent();
        }

        public async void ReloadSettings()
        {
            if (sensorDataWindow != null)
            {
                await sensorDataWindow.InitializeDatabaseAsync();
                sensorDataWindow.InitializeSerialPort();
                sensorDataWindow.InitializeAPI();
            }
        }

        private async void OpenSensorDataWindowButton_Click(object sender, RoutedEventArgs e)
        {
            if (sensorDataWindow == null || !sensorDataWindow.IsLoaded)
            {
                sensorDataWindow = new SensorDataWindow();
                sensorDataWindow.Show();
                await sensorDataWindow.InitializeDatabaseAsync();
            }
            else
            {
                sensorDataWindow.Activate();
            }
        }

        private async void OpenGateWindowButton_Click(object sender, RoutedEventArgs e)
        {
            Gate gate = new Gate();
            gate.Show();
        }

        private void OpenSettingsWindowButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }
    }
}


