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

        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Road> Roads { get; set; }
        public DbSet<RoadFinancials> RoadFinancials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("transport");

            modelBuilder.Entity<Delivery>(entity =>
            {
                // Delivery
                entity.ToTable("deliveries");
                entity.HasKey(d => d.DeliveryId);
                entity.Property(d => d.DeliveryId).HasColumnName("delivery_id");
                entity.Property(d => d.Status).HasColumnName("status");
                entity.Property(d => d.DepartureTime).HasColumnName("departure_time");
                entity.Property(d => d.EstimatedTimeArrival).HasColumnName("estimated_time_arrival");
                entity.Property(d => d.TotalDistanceKm).HasColumnName("total_distance_km");
                entity.Property(d => d.FeeEuros).HasColumnName("fee_euros");

                entity.HasOne(d => d.Order)
                    .WithMany()
                    .HasForeignKey("order_id")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Driver)
                    .WithMany()
                    .HasForeignKey("driver_id")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Truck)
                    .WithMany()
                    .HasForeignKey("truck_id")
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Delivery>().ToTable("deliveries","transport");


            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Username).IsRequired();
                entity.Property(u => u.Password).IsRequired();
            });

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

            // Orders Config
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");
                entity.HasKey(o => o.OrderId);
                entity.Property(o => o.ClientName).IsRequired();
                entity.Property(o => o.CargoType).IsRequired();
                entity.Property(o => o.CargoWeight).IsRequired();

                entity.HasOne(o => o.SourceCity)
                    .WithMany()
                    .HasForeignKey("source_city_id")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.DestinationCity)
                    .WithMany()
                    .HasForeignKey("destination_city_id")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Driver>(entity =>
            {
                entity.ToTable("drivers");
                entity.HasKey(d => d.DriverId);
                entity.Property(d => d.DriverId).HasColumnName("driver_id");
                entity.Property(d => d.FirstName).IsRequired().HasColumnName("first_name");
                entity.Property(d => d.LastName).IsRequired().HasColumnName("last_name");
                entity.Property(d => d.LicenseNumber).IsRequired().HasColumnName("license_number");
                entity.Property(d => d.Phone).HasColumnName("phone");
                entity.Property(d => d.Email).HasColumnName("email");
                entity.Property(d => d.HireDate).IsRequired().HasColumnName("hire_date");
                entity.Property(d => d.Status).IsRequired().HasColumnName("status");
            });

            modelBuilder.Entity<Truck>(entity =>
            {
                entity.ToTable("trucks");
                entity.HasKey(t => t.TruckId);
                entity.Property(t => t.TruckId).HasColumnName("truck_id");
                entity.Property(t => t.LicensePlate).IsRequired().HasColumnName("license_plate");
                entity.Property(t => t.Make).IsRequired().HasColumnName("make");
                entity.Property(t => t.Model).IsRequired().HasColumnName("model");
                entity.Property(t => t.Year).IsRequired().HasColumnName("year");
                entity.Property(t => t.CapacityKg).IsRequired().HasColumnName("capacity_kg");
                entity.Property(t => t.Status).IsRequired().HasColumnName("status");
                entity.Property(t => t.LastMaintenanceDate).HasColumnName("last_maintenance_date");
                entity.Property(t => t.NextMaintenanceDate).HasColumnName("next_maintenance_date");
            });

            // Configure the RoadFinancials entity.
            modelBuilder.Entity<RoadFinancials>(entity =>
            {
                // Map to the new "roadfinancials" table.
                entity.ToTable("roadfinancials", "transport");
                // Set composite primary key.
                entity.HasKey(rf => new { rf.StartCityID, rf.EndCityID });

                // Configure one-to-one relationship between Road and RoadFinancials.
                entity.HasOne(rf => rf.Road)
                      .WithOne(r => r.Financials)
                      .HasForeignKey<RoadFinancials>(rf => new { rf.StartCityID, rf.EndCityID });
            });
        }
    }
}


