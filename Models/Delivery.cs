using System;

namespace UBB_SE_2025_EUROTRUCKERS.Models
{
    public class Delivery
    {
        public int DeliveryId { get; set; }
        public Order? Order { get; set; }
        public Driver? Driver { get; set; }
        public Truck? Truck { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime DepartureTime { get; set; }
        public decimal TotalDistanceKm { get; set; }
        public DateTime EstimatedTimeArrival { get; set; }
        public decimal FeeEuros { get; set; }
    }
}
