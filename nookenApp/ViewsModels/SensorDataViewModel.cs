using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using nookenApp.Models;
using nookenApp.Helper;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace nookenApp.ViewModels
{
    public class SensorDataViewModel : INotifyPropertyChanged
    {
        private readonly AppDbContext _dbContext;
        private ApiHelper _apiHelper;
        private readonly TopertimeRepository _topRepository;
        private readonly TbalansRepository _tbalansRepository;
        private readonly MeasurementsRepository _measurementsRepository;
        private string _dataLog;
        private bool _isReading;

        private List<byte> flesh = new List<byte>();
        private int nFlesh = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public SensorDataViewModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _apiHelper = new ApiHelper();
            _topRepository = new TopertimeRepository(_dbContext);
            _tbalansRepository = new TbalansRepository(_dbContext);
            _measurementsRepository = new MeasurementsRepository(_dbContext);
        }

        public string DataLog
        {
            get => _dataLog;
            set
            {
                _dataLog = value;
                OnPropertyChanged(nameof(DataLog));
            }
        }

        public bool IsReading
        {
            get => _isReading;
            set
            {
                _isReading = value;
                OnPropertyChanged(nameof(IsReading));
            }
        }

        public ICommand StartReadingCommand => new RelayCommand(async () => await StartReading(), () => !IsReading);
        public ICommand StopReadingCommand => new RelayCommand(StopReading, () => IsReading);

        public async Task StartReading()
        {
            IsReading = true;
            try
            {
                if (!SerialPortHelper.port.IsOpen)
                {
                    SerialPortHelper.port.Open();
                }
                while (IsReading)
                {
                    await Opros();
                }
            }
            catch (Exception ex)
            {
                DataLog += $"Error: {ex.Message}\n";
            }
            finally
            {
                if (SerialPortHelper.port.IsOpen)
                {
                    SerialPortHelper.port.Close();
                }
            }
            IsReading = false;
        }

        public void StopReading()
        {
            IsReading = false;
        }

        private async Task Opros()
        {
            byte numplc = 0;
            int counter;
            byte flag;
            DateTime sensorTime = DateTime.MinValue;
            int sutki = 45;

            var tobj = await _dbContext.TOBJ.FirstOrDefaultAsync(o => o.KODDAT == numplc);

            for (byte i = 0; i < 3; i++)
            {
                flesh = await SerialPortHelper.ReadBytesFromSensorAsync();
                if (flesh != null && flesh.Count > 0 && flesh[1] == 3 && flesh[2] == 19)
                {
                    numplc = flesh[0];
                    flag = flesh[19];
                    counter = (flesh[20] << 8) + flesh[21]; // Correct bitwise shift
                    sensorTime = Calculations.DecodeTime(flesh.ToArray());
                    DataLog += $"Sensor Time: {sensorTime:yyyy-MM-dd HH:mm:ss}\n";
                }
                else
                {
                    DataLog += "Communication failure.\n";
                    continue;
                }

                if ((DateTime.Now - sensorTime).TotalSeconds > 3600)
                {
                    List<byte> data = new List<byte> { 0, 0, 0 };
                    await SerialPortHelper.OutIn(flesh, numplc, 4, 0x10, 3, data.ToArray());
                    counter = 0;
                    flag = 0;
                }

                if ((DateTime.Now - sensorTime).TotalSeconds > 60)
                {
                    List<byte> data = new List<byte>
                    {
                        4, // Alarm resolution
                        0, // ms
                        (byte)((sensorTime.Second / 10 << 4) + sensorTime.Second % 10),
                        (byte)((sensorTime.Minute / 10 << 4) + sensorTime.Minute % 10),
                        (byte)((sensorTime.Hour / 10 << 4) + sensorTime.Hour % 10),
                        (byte)((sensorTime.Day / 10 << 4) + sensorTime.Day % 10 + (sensorTime.Year % 100 % 4 << 6)),
                        (byte)(((int)sensorTime.DayOfWeek - 1 << 5) + (sensorTime.Month / 10 << 4) + sensorTime.Month % 10)
                    };
                    if (await SerialPortHelper.OutIn(flesh, numplc, 4, 0, 7, data.ToArray()))
                    {
                        DataLog += "Sensor time updated.\n";
                    }
                }

                bool success = await SerialPortHelper.OutIn(flesh, numplc, 1, 0x60, 6);
                if (success)
                {
                    await HandleSensorData(flesh, sensorTime, numplc, sutki, counter, flag);
                }
                else
                {
                    DataLog += "Communication failure.\n";
                    continue;
                }
            }
        }

        private async Task HandleSensorData(List<byte> flesh, DateTime sensorTime, byte numplc, int sutki, int counter, int flag)
        {
            // Extract values from flesh and convert as needed
            double uu = ((flesh[4] & 0xF0) >> 4) * 10 + (flesh[4] & 0x0F); // Voltage %
            int _signal = flesh[3];
            int tc = (flesh[5] & 0xF0) >> 8;

            // Convert uu to float for consistent use
            float UU = (float)(0.0079 * uu + 3.407); // Ensure explicit conversion from double to float

            // Convert ti to float
            float ti = (float)Calculations.BacktrancAVR(flesh[6], flesh[7], flesh[8]);
            float L = 0;

            var tobj = await _dbContext.TOBJ.FirstOrDefaultAsync(o => o.KODDAT == numplc);
            if (tobj == null)
            {
                DataLog += "TOBJ not found.\n";
                return;
            }

            switch (tobj.KODPOKAZ)
            {
                case 4:
                    L = ti;
                    DataLog += $"L, m={ti:0.000} ";
                    await _topRepository.InsertAsync(tobj.KODOBJ, 4, sensorTime, ti, tc);
                    ti = (float)tobj.KOEFK - ti;
                    break;
                case 2:
                    L = (float)tobj.KOEFK - ti;
                    DataLog += $"L, m={(tobj.KOEFK - ti):0.000} ";
                    await _topRepository.InsertAsync(tobj.KODOBJ, 4, sensorTime, tobj.KOEFK - ti, tc);
                    break;
            }

            float H = ti;
            DataLog += $"H = {ti:0.000}";
            ti = ti < 0 ? 0 : ti;

            float Q = (float)await Calculations.CalculateVolumeAsync((byte)tobj.KODOBJ, H, _dbContext);
            DataLog += $"Q = {Q:0.000}\n";

            DataLog += $"Charge,%={uu:0.0} Signal = {_signal}\n";

            var data = new
            {
                id_reg = Settings.region,
                sensor_number = numplc,
                name_obj = "",
                urov_L = L,
                reyka_H = H,
                rashod_Q = Q,
                charge = uu,
                signal = _signal,
                date = sensorTime
            };

            var data1 = new
            {
                sensor_id = numplc,
                degree = L,
                reika = H,
                rate = Q,
                charge = uu,
                signal = _signal,
                reading_date = sensorTime
            };

            int synchron = 0; // Initialize synchron as 0
            try
            {
                string res = await _apiHelper.SendJsonAsync(data);
                DataLog += $"API response: {res}\n";
                synchron = 1; // Set synchron to 1 if data is successfully sent to the server
            }
            catch
            {
                //DataLog += "Failed to send data to the server.\n";
                Console.WriteLine("Failed to send data to the server.\n");
            }

            bool res1 = await _measurementsRepository.InsertAsync(data.sensor_number, data.name_obj, data.urov_L, data.reyka_H, data.rashod_Q, data.charge, data.signal, data.date, synchron);

            DateTime now = DateTime.Now;
            DateTime maxtime = await _topRepository.GetMaxDatatimeAsync(tobj.KODOBJ);
            int rr = tobj.KOEFSPEED < 60
                ? 1
                : CalculateRR(sutki, tobj, now, maxtime, numplc);

            DataLog += $"{rr} {counter} ";

            flesh.Clear();

            if (counter < rr)
            {
                if (flag > 0)
                {
                    flesh.AddRange(await SerialPortHelper.Read(counter, rr - counter, numplc));
                }
                if (!SerialPortHelper.sboi)
                {
                    List<byte> tmp = await SerialPortHelper.Read(0, counter, numplc);
                    flesh.AddRange(tmp.GetRange(2, tmp.Count - 4));
                }
            }
            else
            {
                flesh.AddRange(await SerialPortHelper.Read(counter - rr, rr, numplc));
            }
            nFlesh = flesh.Count;
            DataLog += $"{nFlesh}\n";

            if (SerialPortHelper.sboi)
            {
                DataLog += "Connection lost " + DateTime.Now.ToString("HH:mm:ss") + "\n";
            }
            else
            {
                List<byte> bytes = new List<byte>();
                DataLog += "Data read " + DateTime.Now.ToString("dd.MM.yy HH:mm:ss") + "\n";
                await SerialPortHelper.Remove_from_sensor();
                await SerialPortHelper.OutIn(bytes, numplc, 7, 0, 0);
            }

            await ProcessFleshData(flesh, sensorTime, sutki, tobj);
        }

        private int CalculateRR(int sutki, TObj tobj, DateTime now, DateTime maxtime, int numplc)
        {
            int rr = 0;
            if (tobj.KOEFSPEED < 60)
            {
                rr = 1;
            }
            else
            {
                if (maxtime == DateTime.MinValue)
                {
                    rr = (int)Math.Round(sutki * 24 * 3600 / tobj.KOEFSPEED);
                }
                else
                {
                    rr = (int)Math.Round((now - maxtime).TotalSeconds / tobj.KOEFSPEED);
                }
            }

            if (rr > (int)Math.Round(sutki * 24 * 3600 / tobj.KOEFSPEED))
            {
                rr = (int)Math.Round(sutki * 24 * 3600 / tobj.KOEFSPEED);
            }

            rr *= 6;
            if (rr > 8184)
            {
                rr = 8184;
            }

            return rr;
        }

        private async Task ProcessFleshData(List<byte> flesh, DateTime sensorTime, int sutki, TObj tobj)
        {
            try
            {
                if (nFlesh > 5)
                {
                    int month = flesh[5] & 0x0F; // Месяц

                    // Extract and calculate minutes
                    int min = flesh[3] * 256 + flesh[4];

                    // Calculate day, hour, and minute
                    int day = min / 1440 % DateTime.DaysInMonth(sensorTime.Year, month);
                    int hour = (min / 60) % 24;
                    min = min % 60;

                    DateTime dattim = DateTime.Now;

                    if (month < 13 && month > 0 && hour < 24 && day > 0 && day <= DateTime.DaysInMonth(sensorTime.Year, month) && min < 60)
                    {
                        if (month < DateTime.Now.Month && month == 1 && DateTime.Now.Month == 12)
                        {
                            dattim = new DateTime(sensorTime.Year + 1, month, day, hour, min, 0);
                        }
                        else
                        {
                            dattim = new DateTime(sensorTime.Year, month, day, hour, min, 0);
                        }
                    }

                    var kodobj = tobj.KODOBJ;
                    
                    var Tvodovd = await _dbContext.Tvodvods.FirstOrDefaultAsync(o => o.UROVEN == tobj.KODOBJ);

                    if (Tvodovd != null && kodobj > 0)
                    {
                        if (dattim.Day * 24 * 60 > 40000)
                        {
                            await _tbalansRepository.DeleteAsync(Tvodovd.KODVODVOD, dattim, DateTime.Now);
                        }

                        int i = 1;
                        while (i <= (nFlesh / 6))
                        {
                            int tc = (flesh[i * 6 - 1] & 0xf0) >> 8; // ТС
                            month = flesh[i * 6 - 1] & 0xf; // Месяц
                            double ti = Calculations.BacktrancAVR(flesh[i * 6 - 6], flesh[i * 6 - 5], flesh[i * 6 - 4]); // Измеренное Значение
                            int totalMinutes = flesh[i * 6 - 3] * 256 + flesh[i * 6 - 2];
                            (day, hour, min) = Calculations.ConvertMinutesToDayHourMinute(totalMinutes, month);

                            if (month < 13 && month > 0 && hour < 24 && day > 0 &&
                                day <= DateTime.DaysInMonth(sensorTime.Year, month) && min < 60)
                            {
                                DateTime datatime;
                                if (month < DateTime.Now.Month && month == 1 && DateTime.Now.Month == 12)
                                    datatime = new DateTime(sensorTime.Year + 1, month, day, hour, min, 0, 0);
                                else
                                    datatime = new DateTime(sensorTime.Year, month, day, hour, min, 0, 0);

                                if (datatime > DateTime.Now.AddDays(-sutki) && datatime < DateTime.Now)
                                {
                                    var objRecord = await _dbContext.TOBJ.FirstOrDefaultAsync(o => o.KODOBJ == kodobj);

                                    if (objRecord != null)
                                    {
                                        int kodpokaz = objRecord.KODPOKAZ;

                                        switch (kodpokaz)
                                        {
                                            case 4: // Гидропост Расстояние до воды
                                                if (ti < 0.16)
                                                    tc |= 0x400;
                                                ti = objRecord.KOEFK - ti; // Уровень
                                                kodpokaz = 2;
                                                break;
                                            case 2: // Гидропост уровень
                                                if (ti > objRecord.KOEFK)
                                                    tc |= 0x400;
                                                break;
                                        }

                                        if (ti < 0)
                                            ti = 0;

                                        if (!await _topRepository.InsertAsync(objRecord.KODOBJ, kodpokaz, datatime, ti, tc)) // Запись уровня воды
                                        {
                                            DataLog += "Failed to save.\n";
                                        }
                                        if ((tc & 0x702) == 0)
                                        {
                                            ti = await Calculations.CalculateVolumeAsync(Convert.ToByte(objRecord.KODOBJ), ti, _dbContext);
                                            await _tbalansRepository.InsertAsync((byte)Tvodovd.KODVODVOD, (byte)Tvodovd.TYPEVODVOD, datatime, ti); // Запись оперативных расходов по водоводам
                                        }
                                    }
                                }
                            }
                            await Task.Delay(10);
                            i++;
                        }
                    }
                    else
                    {
                        DataLog += "TVodvod not found or kodobj is invalid.\n";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in ProcessFleshData: " + ex.Message);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateAPI()
        {
            _apiHelper = new ApiHelper();
        }
    }
}
