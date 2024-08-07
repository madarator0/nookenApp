
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nookenApp.Helper;

namespace nookenApp.Models
{
    public class TQFHRepository
    {
        private readonly AppDbContext _context;

        public TQFHRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> InsertAsync(float _urov, float _rashod, short _kodObj)
        {
            try
            {
                var tqfh = new TQFH { UROV = _urov, RASHOD = _rashod, KODOBJ = _kodObj };
                _context.TQFHs.Add(tqfh);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Другие методы для работы с TQFH
    }

}
