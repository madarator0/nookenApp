using Microsoft.EntityFrameworkCore;
using nookenApp.Models;
using System.Configuration;

namespace nookenApp.Helper
{
    public class AppDbContext : DbContext
    {
        public DbSet<Qoprosv> Qoprosv { get; set; }
        public DbSet<Qvodzatv> Qvodzatv { get; set; }
        public DbSet<QzatvPiorit> QzatvPiorit { get; set; }
        public DbSet<Qtopertimeobj> Qtopertimeobj { get; set; }
        public DbSet<Qvodvodu> Qvodvodu { get; set; }
        public DbSet<Qvodzatvor1> Qvodzatvor1 { get; set; }
        public DbSet<Qvodu> Qvodu { get; set; }
        public DbSet<Qutvod> Qutvod { get; set; }
        public DbSet<Qobjkonturv> Qobjkonturv { get; set; }
        public DbSet<Qobjkonturv2> Qobjkonturv2 { get; set; }
        public DbSet<Qtypevod> Qtypevod { get; set; }
        public DbSet<Qtvod> Qtvod { get; set; }
        public DbSet<TObj> TOBJ { get; set; }
        public DbSet<Topertime> Topertime { get; set; }
        public DbSet<Tbalans> Tbalans { get; set; }
        public DbSet<TVodvod> Tvodvods { get; set; }
        public DbSet<MeasurementsModels> Measurements { get; set; }
        public DbSet<TVod> TVOD { get; set; }
        public DbSet<TKontur> TKONTUR { get; set; }
        public DbSet<TQFH> TQFHs { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public AppDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Settings.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Конфигурация модели Qoprosv
            modelBuilder.Entity<Qoprosv>(entity =>
            {
                entity.HasKey(e => new { e.KODVOD, e.KODOBJ });
                entity.Property(e => e.NAMEOBJ).HasMaxLength(50).IsRequired();
                entity.Property(e => e.VKL).HasMaxLength(1).IsRequired();
                entity.Property(e => e.MAXP).HasColumnType("float");
                entity.Property(e => e.MINP).HasColumnType("float");
                entity.Property(e => e.OTMSET).HasColumnType("float");
                entity.Property(e => e.KOEFC).HasColumnType("float");
                entity.Property(e => e.KOEFK).HasColumnType("float");
                entity.Property(e => e.KOEFB).HasColumnType("float");
                entity.Property(e => e.KOEFSPEED).HasColumnType("float");
                entity.Property(e => e.ALGRASH).HasColumnType("smallint");
                entity.Property(e => e.KODPOKAZ).HasColumnType("smallint");
                entity.Property(e => e.TYPEDAT).HasColumnType("smallint");
                entity.Property(e => e.NULDAT).HasColumnType("float");
                entity.Property(e => e.KANAL).HasColumnType("smallint");
                entity.Property(e => e.PRIORITET).HasColumnType("smallint");
            });

            // Конфигурация модели Qvodzatv
            modelBuilder.Entity<Qvodzatv>(entity =>
            {
                entity.HasKey(e => new { e.KODVOD, e.KODOBJ });
                entity.Property(e => e.NAMEOBJ).HasMaxLength(50).IsRequired();
                entity.Property(e => e.VKL).HasMaxLength(1).IsRequired();
                entity.Property(e => e.MAXP).HasColumnType("float");
                entity.Property(e => e.MINP).HasColumnType("float");
                entity.Property(e => e.OTMSET).HasColumnType("float");
                entity.Property(e => e.KOEFC).HasColumnType("float");
                entity.Property(e => e.KOEFK).HasColumnType("float");
                entity.Property(e => e.KOEFB).HasColumnType("float");
                entity.Property(e => e.KOEFSPEED).HasColumnType("float");
                entity.Property(e => e.ALGRASH).HasColumnType("smallint");
                entity.Property(e => e.KODPOKAZ).HasColumnType("smallint");
                entity.Property(e => e.TYPEDAT).HasColumnType("smallint");
                entity.Property(e => e.KODDAT).HasColumnType("smallint");
                entity.Property(e => e.PRIORITET).HasColumnType("smallint");
            });

            // Конфигурация модели QzatvPiorit
            modelBuilder.Entity<QzatvPiorit>(entity =>
            {
                entity.HasKey(e => new { e.KODVOD, e.KODOBJ });
                entity.Property(e => e.NAMEOBJ).HasMaxLength(50).IsRequired();
                entity.Property(e => e.VKL).HasMaxLength(1).IsRequired();
                entity.Property(e => e.MAXP).HasColumnType("float");
                entity.Property(e => e.MINP).HasColumnType("float");
                entity.Property(e => e.OTMSET).HasColumnType("float");
                entity.Property(e => e.KOEFC).HasColumnType("float");
                entity.Property(e => e.KOEFK).HasColumnType("float");
                entity.Property(e => e.KOEFB).HasColumnType("float");
                entity.Property(e => e.KOEFSPEED).HasColumnType("float");
                entity.Property(e => e.ALGRASH).HasColumnType("smallint");
                entity.Property(e => e.KODPOKAZ).HasColumnType("smallint");
                entity.Property(e => e.TYPEDAT).HasColumnType("smallint");
                entity.Property(e => e.KODDAT).HasColumnType("smallint");
                entity.Property(e => e.PRIORITET).HasColumnType("smallint");
            });

            // Конфигурация модели Qtopertimeobj
            modelBuilder.Entity<Qtopertimeobj>(entity =>
            {
                entity.HasKey(e => new { e.KODOBJ, e.DATATIME });
                entity.Property(e => e.DATATIME).HasColumnType("datetime").IsRequired();
                entity.Property(e => e.TI).HasColumnType("float");
                entity.Property(e => e.KODPOKAZ).HasColumnType("smallint");
                entity.Property(e => e.TC).HasColumnType("smallint");
            });

            // Конфигурация модели Qvodvodu
            modelBuilder.Entity<Qvodvodu>(entity =>
            {
                entity.HasKey(e => e.KODVODVOD);
                entity.HasIndex(e => e.NAMEVODVOD).IsUnique();
                entity.Property(e => e.NAMEVODVOD).HasMaxLength(50).IsRequired();
                entity.Property(e => e.BALANS).HasMaxLength(50).IsRequired();
                entity.Property(e => e.SHIRB).HasColumnType("float");
                entity.Property(e => e.KOEFL).HasColumnType("float");
                entity.Property(e => e.KOEFP).HasColumnType("float");
                entity.Property(e => e.UKLON).HasColumnType("float");
                entity.Property(e => e.DLINA).HasColumnType("float");
                entity.Property(e => e.MAXQ).HasColumnType("float");
                entity.Property(e => e.MINQ).HasColumnType("float");
                entity.Property(e => e.POPRAVKA).HasColumnType("float");
                entity.Property(e => e.SHEROH).HasColumnType("float");
                entity.Property(e => e.KPD).HasColumnType("float");
                entity.Property(e => e.TYPEVODVOD).HasColumnType("smallint").IsRequired();
                entity.Property(e => e.KODPOTREB).HasColumnType("smallint");
                entity.Property(e => e.ALGRAS).HasColumnType("smallint");
                entity.Property(e => e.MAXTIME).HasColumnType("smallint");
                entity.Property(e => e.PRIORITET).HasColumnType("smallint");
                entity.Property(e => e.KONTUR).HasColumnType("smallint").IsRequired();
                entity.Property(e => e.KODVOD).HasColumnType("smallint").IsRequired();
                entity.Property(e => e.UROVEN).HasColumnType("smallint").IsRequired();
            });

            // Конфигурация модели Qvodzatvor1
            modelBuilder.Entity<Qvodzatvor1>(entity =>
            {
                entity.HasKey(e => new { e.KODVOD, e.KODOBJ });
                entity.Property(e => e.NAMEOBJ).HasMaxLength(50).IsRequired();
                entity.Property(e => e.VKL).HasMaxLength(1).IsRequired();
                entity.Property(e => e.MAXP).HasColumnType("float");
                entity.Property(e => e.MINP).HasColumnType("float");
                entity.Property(e => e.OTMSET).HasColumnType("float");
                entity.Property(e => e.KOEFC).HasColumnType("float");
                entity.Property(e => e.KOEFK).HasColumnType("float");
                entity.Property(e => e.KOEFB).HasColumnType("float");
                entity.Property(e => e.KOEFSPEED).HasColumnType("float");
                entity.Property(e => e.ALGRASH).HasColumnType("smallint");
                entity.Property(e => e.KODPOKAZ).HasColumnType("smallint");
                entity.Property(e => e.TYPEDAT).HasColumnType("smallint");
                entity.Property(e => e.KODDAT).HasColumnType("smallint");
                entity.Property(e => e.PRIORITET).HasColumnType("smallint");
            });

            // Конфигурация модели Qvodu
            modelBuilder.Entity<Qvodu>(entity =>
            {
                entity.HasKey(e => e.KODVOD);
                entity.Property(e => e.NAMEVOD).HasMaxLength(30).IsRequired();
                entity.Property(e => e.REGKOD).HasMaxLength(1);
                entity.Property(e => e.SHIRB).HasColumnType("float");
                entity.Property(e => e.KOEFL).HasColumnType("float");
                entity.Property(e => e.KOEFP).HasColumnType("float");
                entity.Property(e => e.UKLON).HasColumnType("float");
                entity.Property(e => e.DLINA).HasColumnType("float");
                entity.Property(e => e.MAXQ).HasColumnType("float");
                entity.Property(e => e.MINQ).HasColumnType("float");
                entity.Property(e => e.POPRAVKA).HasColumnType("float");
                entity.Property(e => e.SHEROH).HasColumnType("float");
                entity.Property(e => e.KPD).HasColumnType("float");
                entity.Property(e => e.TYPEVOD).HasColumnType("smallint").IsRequired();
                entity.Property(e => e.KODPOTREB).HasColumnType("smallint");
                entity.Property(e => e.ALGRAS).HasColumnType("smallint");
                entity.Property(e => e.MAXTIME).HasColumnType("smallint");
                entity.Property(e => e.PRIORITET).HasColumnType("smallint");
            });

            // Конфигурация модели Qutvod
            modelBuilder.Entity<Qutvod>(entity =>
            {
                entity.HasKey(e => new { e.KODVOD, e.KODTVOD });
                entity.Property(e => e.NAMEVOD).HasMaxLength(30).IsRequired();
                entity.Property(e => e.INTERVAL).HasColumnType("smallint");
                entity.Property(e => e.USTAVKA).HasColumnType("float");
                entity.Property(e => e.DOPUSK).HasColumnType("float");
                entity.Property(e => e.NUMNB).HasColumnType("smallint");
                entity.Property(e => e.NUMVB).HasColumnType("smallint");
                entity.Property(e => e.KONTUR).HasColumnType("smallint");
                entity.Property(e => e.RASHFAKT).HasColumnType("float");
                entity.Property(e => e.DELTA).HasColumnType("float");
                entity.Property(e => e.REGKOD).HasMaxLength(1);
                entity.Property(e => e.TEHPOT).HasColumnType("float");
                entity.Property(e => e.KODALG).HasColumnType("smallint");
                entity.Property(e => e.PRIORITET).HasColumnType("smallint");
                entity.Property(e => e.DATATIME).HasColumnType("datetime");
            });

            // Конфигурация модели Qobjkonturv
            modelBuilder.Entity<Qobjkonturv>(entity =>
            {
                entity.HasKey(e => new { e.KODOBJ, e.KODVOD });
                entity.Property(e => e.NAMEOBJ).HasMaxLength(50).IsRequired();
                entity.Property(e => e.NAMEVOD).HasMaxLength(30).IsRequired();
                entity.Property(e => e.VKL).HasMaxLength(1);
                entity.Property(e => e.KOEFC).HasColumnType("float");
                entity.Property(e => e.KOEFK).HasColumnType("float");
                entity.Property(e => e.KOEFB).HasColumnType("float");
                entity.Property(e => e.KOEFSPEED).HasColumnType("float");
                entity.Property(e => e.KODTVOD).HasColumnType("smallint");
                entity.Property(e => e.INTERVAL).HasColumnType("smallint");
                entity.Property(e => e.OTMSET).HasColumnType("float");
                entity.Property(e => e.NULDAT).HasColumnType("float");
                entity.Property(e => e.MAXP).HasColumnType("float");
                entity.Property(e => e.MINP).HasColumnType("float");
                entity.Property(e => e.KANAL).HasColumnType("smallint");
                entity.Property(e => e.PRIORITET).HasColumnType("smallint");
                entity.Property(e => e.DATATIME).HasColumnType("datetime");
            });

            // Конфигурация модели Qobjkonturv2
            modelBuilder.Entity<Qobjkonturv2>(entity =>
            {
                entity.HasKey(e => new { e.KODOBJ, e.KODVOD });
                entity.Property(e => e.NAMEOBJ).HasMaxLength(50).IsRequired();
                entity.Property(e => e.NAMEVOD).HasMaxLength(30).IsRequired();
                entity.Property(e => e.VKL).HasMaxLength(1);
                entity.Property(e => e.KOEFC).HasColumnType("float");
                entity.Property(e => e.KOEFK).HasColumnType("float");
                entity.Property(e => e.KOEFB).HasColumnType("float");
                entity.Property(e => e.KOEFSPEED).HasColumnType("float");
                entity.Property(e => e.KODTVOD).HasColumnType("smallint");
                entity.Property(e => e.INTERVAL).HasColumnType("smallint");
                entity.Property(e => e.OTMSET).HasColumnType("float");
                entity.Property(e => e.NULDAT).HasColumnType("float");
                entity.Property(e => e.MAXP).HasColumnType("float");
                entity.Property(e => e.MINP).HasColumnType("float");
                entity.Property(e => e.KANAL).HasColumnType("smallint");
                entity.Property(e => e.PRIORITET).HasColumnType("smallint");
                entity.Property(e => e.DATATIME).HasColumnType("datetime");
            });

            // Конфигурация модели Qtypevod
            modelBuilder.Entity<Qtypevod>(entity =>
            {
                entity.HasKey(e => e.KODVOD);
                entity.Property(e => e.NAMEVOD).HasMaxLength(30).IsRequired();
                entity.Property(e => e.INTERVAL).HasColumnType("smallint");
                entity.Property(e => e.USTAVKA).HasColumnType("float");
                entity.Property(e => e.DOPUSK).HasColumnType("float");
                entity.Property(e => e.NUMNB).HasColumnType("smallint");
                entity.Property(e => e.NUMVB).HasColumnType("smallint");
                entity.Property(e => e.KONTUR).HasColumnType("smallint");
                entity.Property(e => e.RASHFAKT).HasColumnType("float");
                entity.Property(e => e.DELTA).HasColumnType("float");
                entity.Property(e => e.REGKOD).HasMaxLength(1);
                entity.Property(e => e.TEHPOT).HasColumnType("float");
                entity.Property(e => e.KODALG).HasColumnType("smallint");
                entity.Property(e => e.PRIORITET).HasColumnType("smallint");
                entity.Property(e => e.DATATIME).HasColumnType("datetime");
            });

            // Конфигурация модели Qtvod
            modelBuilder.Entity<Qtvod>(entity =>
            {
                entity.HasKey(e => e.KODVOD);
                entity.Property(e => e.NAMEVOD).HasMaxLength(30).IsRequired();
                entity.Property(e => e.INTERVAL).HasColumnType("smallint");
                entity.Property(e => e.USTAVKA).HasColumnType("float");
                entity.Property(e => e.DOPUSK).HasColumnType("float");
                entity.Property(e => e.NUMNB).HasColumnType("smallint");
                entity.Property(e => e.NUMVB).HasColumnType("smallint");
                entity.Property(e => e.KONTUR).HasColumnType("smallint");
                entity.Property(e => e.RASHFAKT).HasColumnType("float");
                entity.Property(e => e.DELTA).HasColumnType("float");
                entity.Property(e => e.REGKOD).HasMaxLength(1);
                entity.Property(e => e.TEHPOT).HasColumnType("float");
                entity.Property(e => e.KODALG).HasColumnType("smallint");
                entity.Property(e => e.PRIORITET).HasColumnType("smallint");
                entity.Property(e => e.DATATIME).HasColumnType("datetime");
            });

            // Конфигурация модели Tobj
            modelBuilder.Entity<TObj>(entity =>
            {
                entity.HasKey(e => e.KODOBJ);
                entity.Property(e => e.NAMEOBJ).HasMaxLength(50).IsRequired();
                entity.Property(e => e.TYPEOBJ).HasColumnType("smallint").IsRequired();
                entity.Property(e => e.KODPOKAZ).HasColumnType("smallint").IsRequired();
                entity.Property(e => e.KANAL).HasColumnType("smallint").IsRequired();
                entity.Property(e => e.KODDAT).HasColumnType("smallint");
                entity.Property(e => e.TYPEDAT).HasColumnType("smallint").IsRequired();
                entity.Property(e => e.VKL).HasMaxLength(1).IsRequired();
                entity.Property(e => e.OTMSET).HasColumnType("float").HasDefaultValue(0);
                entity.Property(e => e.NULDAT).HasColumnType("float").HasDefaultValue(0);
                entity.Property(e => e.MAXP).HasColumnType("float").HasDefaultValue(4);
                entity.Property(e => e.MINP).HasColumnType("float").HasDefaultValue(0);
                entity.Property(e => e.KOEFK).HasColumnType("float").HasDefaultValue(6);
                entity.Property(e => e.KOEFB).HasColumnType("float").HasDefaultValue(10);
                entity.Property(e => e.KOEFC).HasColumnType("float").HasDefaultValue(1);
                entity.Property(e => e.KOEFSPEED).HasColumnType("float").HasDefaultValue(0.91);
                entity.Property(e => e.PRIORITET).HasColumnType("smallint");
                entity.Property(e => e.ALGRASH).HasColumnType("smallint");
            });

            // Конфигурация модели TVodvod
            modelBuilder.Entity<TVodvod>(entity =>
            {
                entity.HasKey(e => e.KODVODVOD);
                entity.HasIndex(e => e.NAMEVODVOD).IsUnique();
                entity.Property(e => e.SHEROH).HasColumnType("float");
                entity.Property(e => e.KPD).HasColumnType("float");
                entity.Property(e => e.MAXQ).HasColumnType("float");
                entity.Property(e => e.MINQ).HasColumnType("float");
                entity.Property(e => e.SHIRB).HasColumnType("float");
                entity.Property(e => e.KOEFL).HasColumnType("float");
                entity.Property(e => e.KOEFP).HasColumnType("float");
                entity.Property(e => e.UKLON).HasColumnType("float");
                entity.Property(e => e.DLINA).HasColumnType("float");
                entity.Property(e => e.POPRAVKA).HasColumnType("float");
                entity.Property(e => e.TYPEVODVOD).HasColumnType("smallint").IsRequired();
                entity.Property(e => e.KODPOTREB).HasColumnType("smallint");
                entity.Property(e => e.ALGRAS).HasColumnType("smallint");
                entity.Property(e => e.MAXTIME).HasColumnType("smallint");
                entity.Property(e => e.PRIORITET).HasColumnType("smallint");
                entity.Property(e => e.KONTUR).HasColumnType("smallint").IsRequired();
                entity.Property(e => e.KODVOD).HasColumnType("smallint").IsRequired();
                entity.Property(e => e.UROVEN).HasColumnType("smallint").IsRequired();
                entity.Property(e => e.BALANS).HasMaxLength(255).IsRequired();
            });

            // Конфигурация модели TVOD
            //modelBuilder.Entity<TVod>(entity =>
            //{
            //    entity.HasKey(e => e.KODVOD);
            //    entity.Property(e => e.NAMEVOD).HasMaxLength(30).IsRequired();
            //    entity.Property(e => e.REGKOD).HasMaxLength(1);
            //    entity.Property(e => e.SHIRB).HasColumnType("float");
            //    entity.Property(e => e.KOEFL).HasColumnType("float");
            //    entity.Property(e => e.KOEFP).HasColumnType("float");
            //    entity.Property(e => e.UKLON).HasColumnType("float");
            //    entity.Property(e => e.DLINA).HasColumnType("float");
            //    entity.Property(e => e.MAXQ).HasColumnType("float");
            //    entity.Property(e => e.MINQ).HasColumnType("float");
            //    entity.Property(e => e.POPРАВКА).HasColumnType("float");
            //    entity.Property(e => e.SHEROH).HasColumnType("float");
            //    entity.Property(e => e.KPD).HasColumnType("float");
            //    entity.Property(e => e.TYPEVOD).HasColumnType("smallint").IsRequired();
            //    entity.Property(e => e.KODPOTREB).HasColumnType("smallint");
            //    entity.Property(e => e.ALGRAS).HasColumnType("smallint");
            //    entity.Property(e => e.MAXTIME).HasColumnType("smallint");
            //    entity.Property(e => e.PRIORИТЕТ).HasColumnType("smallint");
            //});
        }
    }
}



//using Microsoft.EntityFrameworkCore;
//using nookenApp.Models;
//using System.Configuration;

//namespace nookenApp.Helper
//{
//    public class AppDbContext : DbContext
//    {
//        public DbSet<TQFH> TQFHs { get; set; }
//        public DbSet<Topertime> Topertime { get; set; }
//        public DbSet<Tbalans> Tbalans { get; set; }
//        public DbSet<TObj> TOBJ { get; set; }
//        public DbSet<TVodvod> Tvodvods { get; set; }
//        public DbSet<MeasurementsModels> measurements { get; set; }
//        public DbSet<TVod> TVOD { get; set; }
//        public DbSet<TKontur> TKONTUR { get; set; }

//        public AppDbContext(DbContextOptions<AppDbContext> options)
//            : base(options)
//        {
//        }

//        public AppDbContext()
//        {
//        }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//                optionsBuilder.UseSqlServer(Settings.ConnectionString);
//            }
//        }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<TVodvod>(entity =>
//            {
//                entity.HasKey(e => e.KODVODVOD);
//                entity.HasIndex(e => e.NAMEVODVOD).IsUnique();
//                entity.Property(e => e.SHEROH).HasColumnType("float");
//                entity.Property(e => e.KPD).HasColumnType("float");
//                entity.Property(e => e.MAXQ).HasColumnType("float");
//                entity.Property(e => e.MINQ).HasColumnType("float");
//                entity.Property(e => e.SHIRB).HasColumnType("float");
//                entity.Property(e => e.KOEFL).HasColumnType("float");
//                entity.Property(e => e.KOEFP).HasColumnType("float");
//                entity.Property(e => e.UKLON).HasColumnType("float");
//                entity.Property(e => e.DLINA).HasColumnType("float");
//                entity.Property(e => e.POPRAVKA).HasColumnType("float");
//                entity.Property(e => e.TYPEVODVOD).IsRequired();
//                entity.Property(e => e.KODPOTREB).HasColumnType("smallint");
//                entity.Property(e => e.ALGRAS).HasColumnType("smallint");
//                entity.Property(e => e.MAXTIME).HasColumnType("smallint");
//                entity.Property(e => e.PRIORITET).HasColumnType("smallint");
//                entity.Property(e => e.KODVODVOD).IsRequired();
//                entity.Property(e => e.NAMEVODVOD).HasMaxLength(50).IsRequired();
//                entity.Property(e => e.KONTUR).IsRequired();
//                entity.Property(e => e.KODVOD).IsRequired();
//                entity.Property(e => e.UROVEN).IsRequired();
//                entity.Property(e => e.BALANS).HasMaxLength(255).IsRequired();
//            });
//        }
//    }
//}
