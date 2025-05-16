using System;

namespace UBB_SE_2025_EUROTRUCKERS.Models
{
    public class Delivery
    {
        public int delivery_id { get; set; }
        public Order? order { get; set; }
        public Driver? driver { get; set; }
        public Truck? truck { get; set; }
        public string status { get; set; } = string.Empty;
        public DateTime departure_time { get; set; }
        public decimal total_distance_km { get; set; }
        public DateTime estimated_time_arrival { get; set; }
        public decimal fee_euros { get; set; }
    }
}
