using Microsoft.EntityFrameworkCore;
using nookenApp.Models;
using System.Configuration;

namespace nookenApp.Helper
{
    public class AppDbContext : DbContext
    {
        public DbSet<TQFH> TQFHs { get; set; }
        public DbSet<Topertime> Topertime { get; set; }
        public DbSet<Tbalans> Tbalans { get; set; }
        public DbSet<TObj> TOBJ { get; set; }
        public DbSet<TVodvod> Tvodvods { get; set; }
        public DbSet<MeasurementsModels> measurements { get; set; }

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
                entity.Property(e => e.TYPEVODVOD).IsRequired();
                entity.Property(e => e.KODPOTREB).HasColumnType("smallint");
                entity.Property(e => e.ALGRAS).HasColumnType("smallint");
                entity.Property(e => e.MAXTIME).HasColumnType("smallint");
                entity.Property(e => e.PRIORITET).HasColumnType("smallint");
                entity.Property(e => e.KODVODVOD).IsRequired();
                entity.Property(e => e.NAMEVODVOD).HasMaxLength(50).IsRequired();
                entity.Property(e => e.KONTUR).IsRequired();
                entity.Property(e => e.KODVOD).IsRequired();
                entity.Property(e => e.UROVEN).IsRequired();
                entity.Property(e => e.BALANS).HasMaxLength(255).IsRequired();
            });
        }
    }
}
