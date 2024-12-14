using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace nookenApp.Helper
{
    public class SerialPortHelper
    {
        public static SerialPort port { get; set; } = InitializeSerialPort();

        public static bool sboi = false;

        public static TextBox DataTextBox { get; set; }

        public static async Task<bool> OutIn(List<byte> bufin, byte numplc, byte func, int address, byte nambe, byte[]? data = null)
        {
            bufin.Clear();
            try
            {
                List<byte> bufout = SendPacketToSlaveAsync(numplc, func, address, nambe, data);
                int sizebufout = bufout.Count;

                port.DiscardOutBuffer();
                port.DiscardInBuffer();
                await port.BaseStream.WriteAsync(bufout.ToArray(), 0, sizebufout);

                if (func == 7)
                {
                    return true;
                }

                bufin.AddRange(await ReadBytesFromSensorAsync());
                int sizebufin = bufin.Count;
                ushort crc = Calculations.CalculateCRC(bufin, bufin.Count - 2);

                return (bufin[sizebufin - 2] == (byte)(crc & 0xFF)) && (bufin[sizebufin - 1] == (byte)((crc >> 8) & 0xFF));
            }
            catch (Exception ex)
            {
                //DataTextBox.Text += $"Error: 3{ex.Message}\n";
                Console.WriteLine($"Out In Error{ ex.ToString()}");
                return false;
            }
        }

        public static async Task<List<byte>> ReadBytesFromSensorAsync()
        {
            List<byte> dataBuffer = new List<byte>();
            DateTime lastDataReceivedTime = DateTime.Now;

            try
            {
                byte[] buffer = new byte[256];
                bool isFirstRead = true;
                int retryCount = 0;
                const int maxRetries = 3;

                while (retryCount < maxRetries)
                {
                    if (port.BytesToRead > 0 || isFirstRead)
                    {
                        isFirstRead = false;
                        int bytesRead = await port.BaseStream.ReadAsync(buffer, 0, buffer.Length);
                        if (bytesRead > 0)
                        {
                            dataBuffer.AddRange(buffer.Take(bytesRead));
                            lastDataReceivedTime = DateTime.Now;
                            retryCount = 0; // Сброс счетчика попыток после успешного чтения
                        }
                    }

                    if ((DateTime.Now - lastDataReceivedTime).TotalMilliseconds >= 200)
                    {
                        retryCount++;
                        if (retryCount >= maxRetries)
                        {
                            break;
                        }
                    }

                    await Task.Delay(10);
                }

                return dataBuffer;
            }
            catch (Exception ex)
            {
                //DataTextBox.Text += $"Error reading data: {ex.Message}\n";
                Console.WriteLine($"Error reading data: {ex.ToString()}");
                return new List<byte>();
            }
        }

        //public static async Task<List<byte>> ReadBytesFromSensorAsync()
        //{
        //    List<byte> dataBuffer = new List<byte>();
        //    DateTime lastDataReceivedTime = DateTime.Now;

        //    try
        //    {
        //        byte[] buffer = new byte[256];
        //        bool isFirstRead = true;
        //        int i = 0;
        //        while (i != 5)
        //        {
        //            if (port.BytesToRead > 0 || isFirstRead)
        //            {
        //                isFirstRead = false;
        //                int bytesRead = await port.BaseStream.ReadAsync(buffer, 0, buffer.Length);
        //                if (bytesRead > 0)
        //                {
        //                    dataBuffer.AddRange(buffer.Take(bytesRead));
        //                    lastDataReceivedTime = DateTime.Now;
        //                }
        //            }

        //            if ((DateTime.Now - lastDataReceivedTime).TotalSeconds >= 2)
        //            {
        //                break;
        //            }

        //            await Task.Delay(10);
        //        }

        //        return dataBuffer;
        //    }
        //    catch (Exception ex)
        //    {
        //        DataTextBox.Text += $"Error reading data: {ex.Message}\n";
        //        return new List<byte>();
        //    }
        //}

        public static List<byte> SendPacketToSlaveAsync(byte numplc, byte func, int address, byte nambe, byte[] data)
        {
            try
            {
                List<byte> bufout = new List<byte>
                {
                    numplc,
                    func,
                    (byte)((address & 0xFF0000) >> 16),
                    (byte)((address & 0xFF00) >> 8),
                    (byte)(address & 0xFF),
                    nambe
                };

                if (data != null)
                {
                    bufout.AddRange(data);
                }

                int sizebufout, sizebufin;
                switch (func)
                {
                    case 1:
                    case 3:
                    case 5:
                    case 11:
                        sizebufout = 8;
                        sizebufin = 5 + nambe;
                        break;
                    case 2:
                    case 4:
                    case 6:
                    case 12:
                        sizebufout = 8 + nambe;
                        sizebufin = 8;
                        break;
                    case 7:
                    case 8:
                    case 9:
                        sizebufout = 8;
                        sizebufin = 8;
                        break;
                    default:
                        throw new ArgumentException("Invalid function code");
                }

                ushort crc = Calculations.CalculateCRC(bufout, 6);
                bufout.Add((byte)(crc & 0xFF));
                bufout.Add((byte)((crc >> 8) & 0xFF));

                return bufout;
            }
            catch (Exception ex)
            {
                DataTextBox.Text += $"Error: 2{ex.Message}\n";
                Console.WriteLine($"Send Packet {ex.Message}");
                return new List<byte>();
            }
        }

        public static SerialPort InitializeSerialPort()
        {
            var serialPort = new SerialPort(Settings.PortName)
            {
                BaudRate = 9600,
                DataBits = 8,
                Parity = Parity.None,
                StopBits = StopBits.One,
                ReadTimeout = 5000,
                WriteTimeout = 5000
            };

            return serialPort;
        }

        public async static Task Remove_from_sensor()
        {
            byte[] bufout = new byte[] { 0xFF, 0x04, 0x00, 0x00, 0x10, 0x03, 0x00, 0x00, 0x00, 0x8F, 0x70 };
            await port.BaseStream.WriteAsync(bufout, 0, bufout.Length);
            await ReadBytesFromSensorAsync();
        }

        public async static Task<List<byte>> Read(int kk, int schet, byte numplc)
        {
            List<byte> data = new List<byte>();
            List<byte> tmp = new List<byte>();
            int rr = kk + schet;
            while (kk < rr && kk < 8190)
            {
                try
                {
                    byte toRead = (byte)((rr - kk > 240) ? 240 : rr - kk);
                    if (await OutIn(tmp, numplc, 5, kk, toRead))
                    {
                        if (tmp.Count > 2)
                        {
                            data.AddRange(tmp.GetRange(2, tmp.Count - 2));
                        }
                        else
                        {
                            DataTextBox.Text += "Недостаточно данных для чтения\n";
                            sboi = true;
                            break;
                        }
                    }
                    else
                    {
                        DataTextBox.Text += "Сбой связи при чтении данных\n";
                        sboi = true;
                        break;
                    }
                    kk += toRead;
                }
                catch (Exception ex)
                {
                    DataTextBox.Text += $"Error: 1{ex.Message}\n";
                }
            }
            return data;
        }

        public static int CalculateModbusAddress(int kodobj, ModbusCommand command)
        {
            int baseAddress = 0x0001; // Базовый адрес для всех объектов
            int commandOffset = GetCommandOffset(command);
            return baseAddress + (kodobj - 1) * 10 + commandOffset;
        }

        private static int GetCommandOffset(ModbusCommand command)
        {
            switch (command)
            {
                case ModbusCommand.Lower:
                    return 0; // Смещение для команды опускания
                case ModbusCommand.Raise:
                    return 1; // Смещение для команды подъема
                case ModbusCommand.Stop:
                    return 2; // Смещение для команды остановки
                case ModbusCommand.Diagnose:
                    return 3; // Смещение для команды диагностики
                default:
                    throw new ArgumentOutOfRangeException(nameof(command), "Неизвестная команда Modbus");
            }
        }

        public static async Task<bool> SendModbusCommand(ModbusCommand command, int address, double value = 0)
        {
            switch (command)
            {
                case ModbusCommand.SetUstavka:
                    // Подготовка данных для отправки уставки
                    byte[] data = BitConverter.GetBytes(value);
                    // Отправка команды по Modbus
                    List<byte> response = new List<byte>();
                    bool success = await OutIn(response, numplc: 1, func: 6, address: address, nambe: 2, data: data);
                    return success;

                    // Другие команды...

            }
            return false;
        }

        public enum ModbusCommand
        {
            Lower,
            Raise,
            Stop,
            Diagnose,
            SetUstavka
        }
    }
}



//using System;
//using System.Collections.Generic;
//using System.IO.Ports;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Controls;
//using Newtonsoft.Json;
//using System.Net.Sockets;
//using System.Net;

//namespace nookenApp.Helper
//{
//    public class SerialPortHelper
//    {
//        public static SerialPort port { get; set; }
//        public static bool sboi = false;
//        public static TextBox DataTextBox { get; set; }

//        public static async Task<bool> OutIn(List<byte> bufin, byte numplc, byte func, int address, byte nambe, byte[] data = null)
//        {
//            bufin.Clear();
//            try
//            {
//                List<byte> bufout = await SendPacketToSlaveAsync(numplc, func, address, nambe, data);
//                int sizebufout = bufout.Count;

//                port.DiscardOutBuffer();
//                port.DiscardInBuffer();
//                await port.BaseStream.WriteAsync(bufout.ToArray(), 0, sizebufout);

//                if (func == 7)
//                {
//                    return true;
//                }

//                bufin.AddRange(await ReadBytesFromSensorAsync());
//                int sizebufin = bufin.Count;
//                ushort crc = Calculations.CalculateCRC(bufin, bufin.Count - 2);

//                return (bufin[sizebufin - 2] == (byte)(crc & 0xFF)) && (bufin[sizebufin - 1] == (byte)((crc >> 8) & 0xFF));
//            }
//            catch (Exception ex)
//            {
//                DataTextBox.Text += $"Error: 3{ex.Message}\n";
//                return false;
//            }
//        }

//        public static async Task<List<byte>> ReadBytesFromSensorAsync()
//        {
//            List<byte> dataBuffer = new List<byte>();
//            DateTime lastDataReceivedTime = DateTime.Now;

//            try
//            {
//                byte[] buffer = new byte[256];
//                bool isFirstRead = true;
//                int i = 0;
//                while (i != 5)
//                {
//                    if (port.BytesToRead > 0 || isFirstRead)
//                    {
//                        isFirstRead = false;
//                        int bytesRead = await port.BaseStream.ReadAsync(buffer, 0, buffer.Length);
//                        if (bytesRead > 0)
//                        {
//                            dataBuffer.AddRange(buffer.Take(bytesRead));
//                            lastDataReceivedTime = DateTime.Now;
//                        }
//                    }

//                    if ((DateTime.Now - lastDataReceivedTime).TotalSeconds >= 2)
//                    {
//                        break;
//                    }

//                    await Task.Delay(10);
//                }

//                return dataBuffer;
//            }
//            catch (Exception ex)
//            {
//                DataTextBox.Text += $"Error reading data: {ex.Message}\n";
//                return new List<byte>();
//            }
//        }

//        public static async Task<List<byte>> SendPacketToSlaveAsync(byte numplc, byte func, int address, byte nambe, byte[] data = null)
//        {
//            try
//            {
//                List<byte> bufout = new List<byte>
//                {
//                    numplc,
//                    func,
//                    (byte)((address & 0xFF0000) >> 16),
//                    (byte)((address & 0xFF00) >> 8),
//                    (byte)(address & 0xFF),
//                    nambe
//                };

//                if (data != null)
//                {
//                    bufout.AddRange(data);
//                }

//                int sizebufout, sizebufin;
//                switch (func)
//                {
//                    case 1:
//                    case 3:
//                    case 5:
//                    case 11:
//                        sizebufout = 8;
//                        sizebufin = 5 + nambe;
//                        break;
//                    case 2:
//                    case 4:
//                    case 6:
//                    case 12:
//                        sizebufout = 8 + nambe;
//                        sizebufin = 8;
//                        break;
//                    case 7:
//                    case 8:
//                    case 9:
//                        sizebufout = 8;
//                        sizebufin = 8;
//                        break;
//                    default:
//                        throw new ArgumentException("Invalid function code");
//                }

//                ushort crc = Calculations.CalculateCRC(bufout, 6);
//                bufout.Add((byte)(crc & 0xFF));
//                bufout.Add((byte)((crc >> 8) & 0xFF));

//                return bufout;
//            }
//            catch (Exception ex)
//            {
//                DataTextBox.Text += $"Error: 2{ex.Message}\n";
//                return new List<byte>();
//            }
//        }

//        public static SerialPort InitializeSerialPort(string portName)
//        {
//            var serialPort = new SerialPort(portName)
//            {
//                BaudRate = 9600,
//                DataBits = 8,
//                Parity = Parity.None,
//                StopBits = StopBits.One,
//                ReadTimeout = 5000,
//                WriteTimeout = 5000
//            };

//            return serialPort;
//        }

//        public async static Task Remove_from_sensor()
//        {
//            byte[] bufout = new byte[] { 0xFF, 0x04, 0x00, 0x00, 0x10, 0x03, 0x00, 0x00, 0x00, 0x8F, 0x70 };
//            await port.BaseStream.WriteAsync(bufout, 0, bufout.Length);
//            await ReadBytesFromSensorAsync();
//        }

//        public async static Task<List<byte>> Read(int kk, int schet, byte numplc)
//        {
//            List<byte> data = new List<byte>();
//            List<byte> tmp = new List<byte>();
//            int rr = kk + schet;
//            while (kk < rr && kk < 8190)
//            {
//                try
//                {
//                    byte toRead = (byte)((rr - kk > 240) ? 240 : rr - kk);
//                    if (await OutIn(tmp, numplc, 5, kk, toRead))
//                    {
//                        if (tmp.Count > 2)
//                        {
//                            data.AddRange(tmp.GetRange(2, tmp.Count - 2));
//                        }
//                        else
//                        {
//                            DataTextBox.Text += "Недостаточно данных для чтения\n";
//                            sboi = true;
//                            break;
//                        }
//                    }
//                    else
//                    {
//                        DataTextBox.Text += "Сбой связи при чтении данных\n";
//                        sboi = true;
//                        break;
//                    }
//                    kk += toRead;
//                }
//                catch (Exception ex)
//                {
//                    DataTextBox.Text += $"Error: 1{ex.Message}\n";
//                }
//            }
//            return data;
//        }

//        // New method to send data to API through TCP/IP
//        public static async Task<bool> SendDataToApiThroughTcpIp(object data, string apiUrl, string serverIp, int serverPort)
//        {
//            try
//            {
//                string jsonData = JsonConvert.SerializeObject(data);
//                byte[] dataBytes = Encoding.UTF8.GetBytes(jsonData);

//                using (TcpClient client = new TcpClient())
//                {
//                    await client.ConnectAsync(serverIp, serverPort);
//                    using (NetworkStream stream = client.GetStream())
//                    {
//                        await stream.WriteAsync(dataBytes, 0, dataBytes.Length);
//                        await stream.FlushAsync();

//                        // Optional: Read response from the server
//                        byte[] responseBuffer = new byte[1024];
//                        int bytesRead = await stream.ReadAsync(responseBuffer, 0, responseBuffer.Length);
//                        string responseString = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);

//                        DataTextBox.Text += $"Server Response: {responseString}\n";

//                        return responseString.Contains("200");
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                DataTextBox.Text += $"Error sending data to API through TCP/IP: {ex.Message}\n";
//                return false;
//            }
//        }
//    }
//}
