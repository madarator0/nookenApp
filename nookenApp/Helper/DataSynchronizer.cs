using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace nookenApp.Helper
{
    public class DataSynchronizer
    {
        private readonly AppDbContext _context;
        private readonly ApiHelper _apiHelper;
        private readonly TextBox _textBox;

        public DataSynchronizer(AppDbContext context, string apiUrl, TextBox textBox)
        {
            _context = context;
            _apiHelper = new ApiHelper();
            _textBox = textBox;
        }

        public async Task SynchronizeAsync(int idReg)
        {
            var unsynchronizedData = await _context.Measurements
                .Where(m => m.Synchron == 0)
                .ToListAsync();

            foreach (var measurement in unsynchronizedData)
            {
                var data = new
                {
                    id_reg = idReg,
                    sensor_number = measurement.SensorNumber,
                    name_obj = measurement.NameObj,
                    urov_L = measurement.UrovL,
                    reyka_H = measurement.ReykaH,
                    rashod_Q = measurement.RashodQ,
                    charge = measurement.Charge,
                    signal = measurement.Signal,
                    date = measurement.Date
                };

                try
                {
                    var response = await _apiHelper.SendJsonAsync(data);
                    if (!string.IsNullOrEmpty(response))
                    {
                        measurement.Synchron = 1;
                        _context.Measurements.Update(measurement);
                    }
                }
                catch (Exception ex)
                {
                    // Log or handle the error
                    _textBox.Text = $"Error sending data: {ex.Message}\n";
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
