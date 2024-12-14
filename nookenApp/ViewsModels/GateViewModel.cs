using nookenApp.Helper;
using nookenApp.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace nookenApp.ViewModels
{
    public class GateViewModel : INotifyPropertyChanged, IDisposable
    {
        private System.Timers.Timer _pollingTimer;
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private bool _pollingEnabled = true;
        private double _currentPosition;
        private double _rashod;
        private string _log;
        private string _zatvorValue;
        private AppDbContext _db;
        List<TObj> _objects;

        public List<TObj> Objects
        {
            get => _objects;
            set
            {
                _objects = value;
                OnPropertyChanged();
            }
        }

        private TObj _selectedObject;
        public TObj SelectedObject
        {
            get => _selectedObject;
            set
            {
                _selectedObject = value;
                OnPropertyChanged();
            }
        }
        public double Rashod
        {
            get => _rashod;
            set
            {
                _rashod = value;
                OnPropertyChanged();
            }
        }
        public double CurrentPosition
        {
            get => _currentPosition;
            set
            {
                _currentPosition = value;
                OnPropertyChanged();
            }
        }

        public string Log
        {
            get => _log;
            set
            {
                _log = value;
                OnPropertyChanged();
            }
        }

        public string ZatvorValue
        {
            get => _zatvorValue;
            set
            {
                _zatvorValue = value;
                OnPropertyChanged();
            }
        }

        public ICommand StopCommand { get; }
        public ICommand LowerCommand { get; }
        public ICommand RaiseCommand { get; }

        public GateViewModel(AppDbContext db)
        {
            _db = db;
            StopCommand = new RelayCommand(async () => await StopCommandExecute());
            LowerCommand = new RelayCommand(async () => await LowerCommandExecute());
            RaiseCommand = new RelayCommand(async () => await RaiseCommandExecute());

            // Асинхронная загрузка объектов
            LoadObjectsAsync().GetAwaiter();

            try
            {
                if (SerialPortHelper.port != null && !SerialPortHelper.port.IsOpen)
                {
                    SerialPortHelper.port.Open();
                }
            }
            catch
            {
                MessageBox.Show("COM порт отсутствует");
                return;
            }

            SetupPollingTimer();
        }

        private async Task LoadObjectsAsync()
        {
            try
            {
                Objects = await Task.Run(() => _db.TOBJ.Where(o => o.TYPEOBJ > 60 && o.TYPEOBJ < 80).ToList());
            }
            catch (Exception ex)
            {
                Log += $"Ошибка загрузки объектов: {ex.Message}\n";
            }
        }


        private void SetupPollingTimer()
        {
            _pollingTimer = new System.Timers.Timer(500);
            _pollingTimer.Elapsed += OnPollingEvent;
            _pollingTimer.AutoReset = true;
            _pollingTimer.Enabled = true;
        }

        private async void OnPollingEvent(object source, ElapsedEventArgs e)
        {
            if (!_pollingEnabled) return;

            try
            {
                List<byte> list = new List<byte>();

                await _semaphore.WaitAsync(); // Synchronize access to the port
                try
                {
                    if (SerialPortHelper.port == null || !SerialPortHelper.port.IsOpen)
                    {
                        Log += "Serial port is not open or not initialized.\n";
                        return;
                    }

                    // Sensor polling logic
                    bool success = await SerialPortHelper.OutIn(list, (byte)SelectedObject.KODDAT, 1, 0x62, 4, null);
                    if (success && list.Count > 0)
                    {
                        DataProcessing(list); // Process the received data
                    }
                    else
                    {
                        Log += "Ошибка при опросе датчиков: данные не получены или неправильные.\n";
                    }
                }
                finally
                {
                    _semaphore.Release();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при опросе датчиков: {ex.ToString()}");
            }
        }

        private void StopSensorPolling()
        {
            _pollingTimer?.Stop();
        }

        private void StartSensorPolling()
        {
            _pollingTimer?.Start();
        }

        private async Task<bool> SendCommand(List<byte> list, byte numplc, byte command, int parameter, byte length)
        {
            bool success = false;
            await _semaphore.WaitAsync(); // Synchronize access to the port
            try
            {
                if (SerialPortHelper.port == null || !SerialPortHelper.port.IsOpen)
                {
                    Log += "Serial port is not open or not initialized.\n";
                    return false;
                }

                success = await SerialPortHelper.OutIn(list, numplc, command, parameter, length, null);
            }
            finally
            {
                _semaphore.Release();
            }
            return success;
        }

        private async Task StopCommandExecute()
        {
            _pollingEnabled = false;
            StopSensorPolling();

            List<byte> list = new List<byte>();
            byte numplc = (byte)SelectedObject.KODDAT;

            Log += "Sending Stop Command...\n";

            bool success = await SendCommand(list, numplc, 0x07, 0x62, 4);

            if (success)
            {
                await SendCommand(list, (byte)(numplc + 100), 0x07, 0x60, 3);
                Log += "Stop command sent successfully. Waiting for response...\n";
            }
            else
            {
                Log += "Ошибка при отправке команды стопа.\n";
            }

            _pollingEnabled = true;
            StartSensorPolling();
        }

        private async Task LowerCommandExecute()
        {
            _pollingEnabled = false;
            StopSensorPolling();

            if (double.TryParse(ZatvorValue, out double zatvorValue))
            {
                List<byte> list = new List<byte>();
                int zatvorPosition = (int)(Math.Round(zatvorValue * 1000)) & 0x7FFF;
                byte numplc = (byte)SelectedObject.KODDAT;

                bool success = await SendCommand(list, numplc, 9, zatvorPosition, 3);

                if (success)
                {
                    Log += $"Lower command sent successfully. Waiting for response...\n";
                    await SendCommand(list, (byte)(numplc + 100), 9, 0x60, 4);
                }
                else
                {
                    Log += "Ошибка при опускании затвора.\n";
                }
            }
            else
            {
                Log += "Некорректное значение.\n";
            }

            _pollingEnabled = true;
            StartSensorPolling();
        }

        private async Task RaiseCommandExecute()
        {
            _pollingEnabled = false;
            StopSensorPolling();

            if (double.TryParse(ZatvorValue, out double zatvorValue))
            {
                List<byte> list = new List<byte>();
                int zatvorPosition = (int)(Math.Round(zatvorValue * 1000)) & 0x7FFF;
                byte numplc = 12;

                bool success = await SendCommand(list, numplc, 8, zatvorPosition, 3);

                if (success)
                {
                    Log += $"Raise command sent successfully. Waiting for response...\n";
                    await SendCommand(list, (byte)(numplc + 100), 8, 0x60, 4);
                }
                else
                {
                    Log += "Ошибка при поднятии затвора.\n";
                }
            }
            else
            {
                Log += "Некорректное значение.\n";
            }

            _pollingEnabled = true;
            StartSensorPolling();
        }

        private async void DataProcessing(List<byte> data)
        {
            try
            {
                // Processing data from the zatvor
                CurrentPosition = Calculations.BacktrancAVR(data[4], data[5], data[6]);
                Rashod = await Calculations.CalculateVolumeAsync(6, CurrentPosition, _db);
                // Additional data processing logic can be added here
            }
            catch (Exception ex)
            {
                Log += $"Ошибка обработки данных: {ex.Message}\n";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            _pollingTimer?.Dispose();
            if (SerialPortHelper.port != null && SerialPortHelper.port.IsOpen)
            {
                SerialPortHelper.port.Close();
            }
        }
    }
}
