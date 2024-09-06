using Microsoft.EntityFrameworkCore;
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

        public async Task<List<TQFH>> GetAllAsync()
        {
            return await _context.TQFHs.ToListAsync();
        }

        public async Task<TQFH> GetByKodobjAsync(int kodobj)
        {
            return await _context.TQFHs.FirstOrDefaultAsync(t => t.KODOBJ == kodobj) ?? new TQFH();
        }

        public async Task AddAsync(TQFH entity)
        {
            _context.TQFHs.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TQFH entity)
        {
            _context.TQFHs.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.TQFHs.FindAsync(id);
            if (entity != null)
            {
                _context.TQFHs.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }

}
