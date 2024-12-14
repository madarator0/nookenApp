using Microsoft.EntityFrameworkCore;
using nookenApp.Helper;
using nookenApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nookenApp.Repositories
{
    public class QobjkonturvRepository
    {
        private readonly AppDbContext _context;

        public QobjkonturvRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<QobjkonturvModel>> GetKonturByKodObj(int kodobj)
        {
            var query = from v in _context.TVOD
                        join o in _context.TOBJ on v.KodVod equals o.KODVOD
                        join k in _context.TKONTUR on v.Kontur equals k.Kontur
                        where o.KODOBJ == kodobj
                        select new QobjkonturvModel
                        {
                            KONTUR = v.Kontur,
                            NUMVB = v.NumVb,
                            NUMNB = v.NumNb,
                            KOEFC = o.KOEFC,
                            KODVOD = v.KodVod,
                            NAMEOBJ = o.NAMEOBJ,
                            NAMEVOD = v.NameVod,
                            TYPEOBJ = o.TYPEOBJ,
                            INTERVAL = v.Interval,
                            KODTVOD = v.KodTvod,
                            USTAVKA = v.Ustavka,
                            DOPUSK = v.Dopuska,
                            NAMETVOD = v.NameTvod,
                            RASHFAKT = v.Rashfakt,
                            DELTA = v.Delta,
                            DATATIME = v.Datatime,
                            REGVKL = v.RegVkl,
                            KODALG = v.KodAlg,
                            PRIORITET = v.Prioritet,
                            TEHPOT = v.Tehpot,
                            UPRAV = v.Uprav,
                            KODOBJ = o.KODOBJ,
                            KODPOKAZ = o.KODPOKAZ,
                            KODDAT = o.KODDAT,
                            TYPEDAT = o.TYPEDAT,
                            VKL = o.VKL,
                            OTMSET = o.OTMSET,
                            NULDAT = o.NULDAT,
                            MAXP = o.MAXP,
                            MINP = o.MINP,
                            KOEFK = o.KOEFK,
                            KOEFB = o.KOEFB,
                            KANAL = o.KANAL,
                            PRIORITET_1 = o.PRIORITET,
                            KOEFSPEED = o.KOEFSPEED,
                            ALGRASH = o.ALGRASH,
                            KD = v.Kd,
                            KI = v.Ki,
                            KP = v.Kp,
                            MAXUPR = k.MaxUpr
                        };

            return await query.ToListAsync();
        }
    }
}
