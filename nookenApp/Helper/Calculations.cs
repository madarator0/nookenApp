//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using nookenTest.Models;

//namespace nookenTest2.Halper
//{
//    static class Calculations
//    {
//        static public double BacktrancAVR(byte poryadok, byte man_hi, byte man_lo)
//        {
//            int mant = (man_hi << 8) + man_lo;

//            if ((poryadok & 0x80) > 0)
//            {
//                mant = -mant;
//            }

//            poryadok = (byte)(poryadok & 0x7F);

//            int por = 0x50 - poryadok;

//            double ish = Math.Pow(2, por);
//            ish = 1 / ish;

//            return mant * ish;
//        }

//        static public double CalculateVolume(byte kodobj, double H)
//        {
//            double result = 0;
//            double hmax, vmax;
//            int i;

//            try
//            {
//                DataTable TOBJ = DatabaseHelper.GetDataTable($"SELECT NULDAT FROM TOBJ WHERE KODOBJ = {kodobj}");
//                if (TOBJ != null && TOBJ.Rows.Count > 0)
//                {
//                    H += Convert.ToDouble(TOBJ.Rows[0]["NULDAT"]);
//                    H *= 100; // Convert to centimeters
//                    if (H < 0) H = 0;

//                    result = 0;

//                    DataTable TQFH = DatabaseHelper.GetDataTable($"SELECT * FROM TQFH WHERE KODOBJ = {kodobj}");

//                    if (TQFH != null && TQFH.Rows.Count > 0)
//                    {
//                        i = 0;
//                        foreach (DataRow row in TQFH.Rows)
//                        {
//                            if (H < Convert.ToDouble(row["UROV"]))
//                            {
//                                break;
//                            }
//                            i++;
//                        }

//                        if (i == 0)
//                        {
//                            DataRow firstRow = TQFH.Rows[0];
//                            if (Convert.ToDouble(firstRow["UROV"]) - H > 0.001)
//                            {
//                                hmax = Convert.ToDouble(firstRow["UROV"]);
//                                vmax = Convert.ToDouble(firstRow["RASHOD"]);
//                                if (TQFH.Rows.Count > 1)
//                                {
//                                    DataRow nextRow = TQFH.Rows[1];
//                                    if (Math.Abs(Convert.ToDouble(nextRow["UROV"]) - hmax) > 0.0001)
//                                    {
//                                        result = vmax - (Convert.ToDouble(nextRow["RASHOD"]) - vmax) * (hmax - H) / (Convert.ToDouble(nextRow["UROV"]) - hmax);
//                                    }
//                                }
//                            }
//                            else
//                            {
//                                result = Convert.ToDouble(firstRow["RASHOD"]);
//                            }
//                        }
//                        else
//                        {
//                            DataRow currentRow = TQFH.Rows[i - 1];
//                            if (H > Convert.ToDouble(currentRow["UROV"]))
//                            {
//                                currentRow = TQFH.Rows[TQFH.Rows.Count - 1];
//                            }
//                            if (Math.Abs(H - Convert.ToDouble(currentRow["UROV"])) < 0.001)
//                            {
//                                result = Convert.ToDouble(currentRow["RASHOD"]);
//                            }
//                            else
//                            {
//                                hmax = Convert.ToDouble(currentRow["UROV"]);
//                                vmax = Convert.ToDouble(currentRow["RASHOD"]);
//                                DataRow priorRow = TQFH.Rows[i - 2];
//                                if (Math.Abs(hmax - Convert.ToDouble(priorRow["UROV"])) < 0.001)
//                                {
//                                    result = vmax;
//                                }
//                                else
//                                {
//                                    result = (vmax - Convert.ToDouble(priorRow["RASHOD"])) / (hmax - Convert.ToDouble(priorRow["UROV"])) *
//                                             (H - Convert.ToDouble(priorRow["UROV"])) + Convert.ToDouble(priorRow["RASHOD"]);
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//            catch
//            {
//                result = 0;
//            }

//            if (H < 0.001) result = 0;
//            if (result < 0) result = 0;

//            return result;
//        }

//        static public DateTime DecodeTime(byte[] data)
//        {
//            int sec = ((data[5] & 0xF0) >> 4) * 10 + (data[5] & 0x0F);
//            int min = ((data[6] & 0xF0) >> 4) * 10 + (data[6] & 0x0F);
//            int hour = ((data[7] & 0xF0) >> 4) * 10 + (data[7] & 0x0F);
//            int day = ((data[8] & 0x30) >> 4) * 10 + (data[8] & 0x0F);
//            int month = ((data[9] & 0x10) >> 4) * 10 + (data[9] & 0x0F);

//            int year = DateTime.Now.Year;

//            if (month == 1 && DateTime.Now.Month == 12)
//            {
//                year--;
//            }

//            return new DateTime(year, month, day, hour, min, sec);
//        }

//        public static (int day, int hour, int minute) ConvertMinutesToDayHourMinute(int totalMinutes, int month)
//        {
//            if (month >= 12)
//            {
//                month = month % 12;
//            }
//            // Array to store the number of days in each month (index 0 for January, 1 for February, etc.)
//            int[] daysInMonth = new int[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

//            // Check if the year is a leap year for February adjustment
//            // This example assumes a non-leap year; adjust as needed
//            if (month == 2) // February
//            {
//                daysInMonth[1] = 29; // Adjust for leap year if necessary
//            }

//            int minutesInDay = 1440; // 24 hours * 60 minutes
//            int daysInCurrentMonth = daysInMonth[month];

//            int day = (totalMinutes / minutesInDay) % daysInCurrentMonth;
//            int hour = (totalMinutes / 60) % 24;
//            int minute = totalMinutes % 60;

//            return (day, hour, minute);
//        }

//        static public ushort CalculateCRC(List<byte> data, int length)
//        {
//            ushort crc = 0xFFFF;
//            for (int i = 0; i < length; i++)
//            {
//                crc ^= data[i];
//                for (int j = 0; j < 8; j++)
//                {
//                    if ((crc & 1) > 0)
//                    {
//                        crc >>= 1;
//                        crc ^= 0xA001;
//                    }
//                    else
//                    {
//                        crc >>= 1;
//                    }
//                }
//            }
//            return crc;
//        }
//    }
//}


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using nookenApp.Models;
using Microsoft.EntityFrameworkCore;

namespace nookenApp.Helper
{
    static class Calculations
    {
        static public double BacktrancAVR(byte poryadok, byte man_hi, byte man_lo)
        {
            int mant = (man_hi << 8) + man_lo;

            if ((poryadok & 0x80) > 0)
            {
                mant = -mant;
            }

            poryadok = (byte)(poryadok & 0x7F);

            int por = 0x50 - poryadok;

            double ish = Math.Pow(2, por);
            ish = 1 / ish;

            return mant * ish;
        }

        static public async Task<double> CalculateVolumeAsync(byte kodobj, double H, AppDbContext context)
        {
            double result = 0;
            double hmax, vmax;
            int i;

            try
            {
                var TOBJ = await context.TOBJ
                    .Where(t => t.KODOBJ == kodobj)
                    .Select(t => t.NULDAT)
                    .FirstOrDefaultAsync();

                if (TOBJ != 0)
                {
                    H += TOBJ;
                    H *= 100; // Convert to centimeters
                    if (H < 0) H = 0;

                    result = 0;

                    var TQFH = await context.TQFHs
                        .Where(t => t.KODOBJ == kodobj)
                        .ToListAsync();

                    if (TQFH.Any())
                    {
                        i = 0;
                        foreach (var row in TQFH)
                        {
                            if (H < row.UROV)
                            {
                                break;
                            }
                            i++;
                        }

                        if (i == 0)
                        {
                            var firstRow = TQFH[0];
                            if (firstRow.UROV - H > 0.001)
                            {
                                hmax = firstRow.UROV;
                                vmax = firstRow.RASHOD;
                                if (TQFH.Count > 1)
                                {
                                    var nextRow = TQFH[1];
                                    if (Math.Abs(nextRow.UROV - hmax) > 0.0001)
                                    {
                                        result = vmax - (nextRow.RASHOD - vmax) * (hmax - H) / (nextRow.UROV - hmax);
                                    }
                                }
                            }
                            else
                            {
                                result = firstRow.RASHOD;
                            }
                        }
                        else
                        {
                            var currentRow = TQFH[i - 1];
                            if (H > currentRow.UROV)
                            {
                                currentRow = TQFH.Last();
                            }
                            if (Math.Abs(H - currentRow.UROV) < 0.001)
                            {
                                result = currentRow.RASHOD;
                            }
                            else
                            {
                                hmax = currentRow.UROV;
                                vmax = currentRow.RASHOD;
                                var priorRow = TQFH[i - 2];
                                if (Math.Abs(hmax - priorRow.UROV) < 0.001)
                                {
                                    result = vmax;
                                }
                                else
                                {
                                    result = (vmax - priorRow.RASHOD) / (hmax - priorRow.UROV) *
                                             (H - priorRow.UROV) + priorRow.RASHOD;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                result = 0;
            }

            if (H < 0.001) result = 0;
            if (result < 0) result = 0;

            return result;
        }

        static public DateTime DecodeTime(byte[] data)
        {
            int sec = ((data[5] & 0xF0) >> 4) * 10 + (data[5] & 0x0F);
            int min = ((data[6] & 0xF0) >> 4) * 10 + (data[6] & 0x0F);
            int hour = ((data[7] & 0xF0) >> 4) * 10 + (data[7] & 0x0F);
            int day = ((data[8] & 0x30) >> 4) * 10 + (data[8] & 0x0F);
            int month = ((data[9] & 0x10) >> 4) * 10 + (data[9] & 0x0F);

            int year = DateTime.Now.Year;

            if (month == 1 && DateTime.Now.Month == 12)
            {
                year--;
            }

            return new DateTime(year, month, day, hour, min, sec);
        }

        public static (int day, int hour, int minute) ConvertMinutesToDayHourMinute(int totalMinutes, int month)
        {
            if (month >= 12)
            {
                month = month % 12;
            }
            // Array to store the number of days in each month (index 0 for January, 1 for February, etc.)
            int[] daysInMonth = new int[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            // Check if the year is a leap year for February adjustment
            // This example assumes a non-leap year; adjust as needed
            if (month == 2) // February
            {
                daysInMonth[1] = 29; // Adjust for leap year if necessary
            }

            int minutesInDay = 1440; // 24 hours * 60 minutes
            int daysInCurrentMonth = daysInMonth[month];

            int day = (totalMinutes / minutesInDay) % daysInCurrentMonth;
            int hour = (totalMinutes / 60) % 24;
            int minute = totalMinutes % 60;

            return (day, hour, minute);
        }

        static public ushort CalculateCRC(List<byte> data, int length)
        {
            ushort crc = 0xFFFF;
            for (int i = 0; i < length; i++)
            {
                crc ^= data[i];
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 1) > 0)
                    {
                        crc >>= 1;
                        crc ^= 0xA001;
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }
            }
            return crc;
        }
    }
}

