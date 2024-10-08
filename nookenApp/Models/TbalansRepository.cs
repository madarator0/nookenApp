﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nookenApp.Helper;

namespace nookenApp.Models
{
    public class TbalansRepository
    {
        private readonly AppDbContext _context;

        public TbalansRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> InsertAsync(byte kodvodvod, byte typevodvod, DateTime datatime, double ti)
        {
            try
            {
                var tbalans = new Tbalans
                {
                    KODVODVOD = kodvodvod,
                    TYPEVODVOD = typevodvod,
                    DATATIME = datatime,
                    RASHOD = ti
                };
                _context.Tbalans.Add(tbalans);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(byte kodvodvod, DateTime datatime, double ti)
        {
            try
            {
                var tbalans = await _context.Tbalans
                    .FirstOrDefaultAsync(t => t.KODVODVOD == kodvodvod && t.DATATIME == datatime);
                if (tbalans == null) return false;

                tbalans.RASHOD = ti;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int kodvodvod, DateTime datatime, DateTime datatime1)
        {
            try
            {
                var tbalans = await _context.Tbalans
                    .Where(t => t.KODVODVOD == kodvodvod && t.DATATIME >= datatime && t.DATATIME <= datatime1)
                    .ToListAsync();
                if (!tbalans.Any()) return false;

                _context.Tbalans.RemoveRange(tbalans);
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
