using Microsoft.EntityFrameworkCore;

using gateway.domain.Entities;

namespace gateway.dal
{
    public class ADbContext : DbContext
    {
        public DbSet<Gateway> Gateways { get; set; }
        public DbSet<Peripheral> Peripherals { get; set; }
        
        public ADbContext(DbContextOptions<ADbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            #region ======== DEFAULTS VALUES ===================================================
            
            modelBuilder.Entity<Peripheral>()
                .Property(p => p.Created)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETUTCDATE()");
            
            #endregion ======== DEFAULTS VALUES ================================================
            
            #region ======== INDEXES ===========================================================

            modelBuilder.Entity<Gateway>()
                .HasIndex(g => g.SerialNumber)
                .IsUnique();
            
            modelBuilder.Entity<Peripheral>()
                .HasIndex(p => p.Uid)
                .IsUnique();
            
            #endregion ======== INDEXES ========================================================
            
            #region ======== RELATIONSHIPS =====================================================

            modelBuilder.Entity<Gateway>()
                .HasMany(g => g.Peripherals)
                .WithOne(p => p.Gateway)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion ======== RELATIONSHIPS ==================================================

        }
    }
}