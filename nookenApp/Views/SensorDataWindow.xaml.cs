using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using nookenApp.Helper;
using System.IO.Ports;

namespace nookenApp.Views
{
    public partial class SensorDataWindow : Window
    {
        private static readonly SemaphoreSlim portSemaphore = new SemaphoreSlim(1, 1);
        private static bool work = true;
        private static DbContextOptions<AppDbContext>? optionsBuilder;
        private AppDbContext db;

        public SensorDataWindow()
        {
            InitializeComponent();
            InitializeSerialPort();
        }

        public async Task InitializeDatabaseAsync()
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            await dbHelper.InitializeDatabaseAsync();

            optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(Settings.ConnectionString)
                .Options;

            db = new AppDbContext(optionsBuilder);
            DataRotation.DataTextBox = DataTextBox;
            DataRotation.Initialize(db);
        }

        public void InitializeSerialPort()
        {
            SerialPortHelper.port = new SerialPort(Settings.PortName);
            SerialPortHelper.DataTextBox = DataTextBox;
        }

        public void InitializeAPI()
        {
            DataRotation.UpdateAPI();
        }

        private async void ReadDataButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await portSemaphore.WaitAsync();
                work = true;
                if (!SerialPortHelper.port.IsOpen)
                {
                    SerialPortHelper.port.Open();
                }
                while (work)
                {
                    await DataRotation.Opros();
                }
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => DataTextBox.Text = "Error: " + ex.Message + "\n");
            }
            finally
            {
                if (SerialPortHelper.port.IsOpen)
                {
                    SerialPortHelper.port.Close();
                }
                portSemaphore.Release();
            }
        }

        private void StopReadingButton_Click(object sender, RoutedEventArgs e)
        {
            work = false;
        }
    }
}
