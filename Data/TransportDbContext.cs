using Microsoft.EntityFrameworkCore;
using UBB_SE_2025_EUROTRUCKERS.Models;

namespace UBB_SE_2025_EUROTRUCKERS.Data
{
    public class TransportDbContext : DbContext
    {
        public TransportDbContext(DbContextOptions<TransportDbContext> options)
            : base(options)
        {
        }


        public DbSet<Company> companies { get; set; }
        public DbSet<Driver> drivers { get; set; }
        public DbSet<Truck> trucks { get; set; }
        public DbSet<Delivery> deliveries { get; set; }
        public DbSet<City> cities { get; set; }
        public DbSet<Road> roads { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuraciones del modelo
            modelBuilder.Entity<Delivery>()
                .HasKey(d => d.delivery_id);

            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.driver)
                .WithMany()
                .HasForeignKey(d => d.driver_id);

            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.truck)
                .WithMany()
                .HasForeignKey(d => d.truck_id);

            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.company)
                .WithMany()
                .HasForeignKey(d => d.company_id);

            modelBuilder.Entity<Delivery>().ToTable("deliveries", "transport");


            // Cities and Roads Config
            modelBuilder.Entity<City>(city =>
            {
                city.HasKey(c => c.id);
                city.Property(c => c.x).HasColumnType("real");
                city.Property(c => c.y).HasColumnType("real");
            });
            modelBuilder.Entity<City>().ToTable("cities", "transport");

            modelBuilder.Entity<Road>(road =>
            {
                road.HasKey(r => new { r.startCityID, r.endCityID });
                road.Property(r => r.distance).HasColumnType("real");
            });
            modelBuilder.Entity<Road>()
                .HasOne(r => r.StartCity)
                .WithMany(c => c.Roads)
                .HasForeignKey(r => r.startCityID)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Road>()
            .HasOne(r => r.EndCity)
            .WithMany()
            .HasForeignKey(r => r.endCityID)
            .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Road>().ToTable("roads", "transport");

        }
    }
}
