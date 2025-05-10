using System;

namespace UBB_SE_2025_EUROTRUCKERS.Models
{
    public class Truck
    {
        public int TruckId { get; set; }
        public string LicensePlate { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal CapacityKg { get; set; }
        public string Status { get; set; }
        public DateTime? LastMaintenanceDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
    }
}
