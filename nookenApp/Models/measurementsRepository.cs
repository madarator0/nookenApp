using System;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using nookenApp.Helper;

namespace nookenApp.Models
{
    public class MeasurementsRepository
    {
        private readonly AppDbContext _context;

        public MeasurementsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> InsertAsync(int sensorNumber, string nameObj, double urovL, double reykaH, double rashodQ, double charge, double signal, DateTime date, int synchron)
        {
            try
            {
                var measurement = new MeasurementsModels
                {
                    SensorNumber = sensorNumber,
                    NameObj = nameObj,
                    UrovL = urovL,
                    ReykaH = reykaH,
                    RashodQ = rashodQ,
                    Charge = charge,
                    Signal = signal,
                    Date = date,
                    Synchron = synchron
                };

                _context.measurements.Add(measurement);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
