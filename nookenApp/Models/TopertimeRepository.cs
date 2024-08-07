using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nookenApp.Helper;

namespace nookenApp.Models
{
    public class TopertimeRepository
    {
        private readonly AppDbContext _context;

        public TopertimeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> InsertAsync(int kodobj, int kodpokaz, DateTime datatime, double ti, int tc)
        {
            try
            {
                var topertime = new Topertime
                {
                    KODOBJ = kodobj,
                    KODPOKAZ = kodpokaz,
                    DATATIME = datatime,
                    TI = ti,
                    TC = tc
                };
                _context.Topertime.Add(topertime);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(int kodobj, byte kodpokaz, DateTime datatime, double ti, int tc)
        {
            try
            {
                var topertime = await _context.Topertime
                    .FirstOrDefaultAsync(t => t.KODOBJ == kodobj && t.KODPOKAZ == kodpokaz && t.DATATIME == datatime);
                if (topertime == null) return false;

                topertime.TI = ti;
                topertime.TC = tc;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<DateTime> GetMaxDatatimeAsync(int kodobj)
        {
            try
            {
                var result = await _context.Topertime
                    .Where(t => t.KODOBJ == kodobj && t.KODPOKAZ == 2)
                    .MaxAsync(t => (DateTime?)t.DATATIME);
                return result ?? DateTime.MinValue;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
    }

}
