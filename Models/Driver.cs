using System;

namespace UBB_SE_2025_EUROTRUCKERS.Models
{
    
    public class Driver
    {
        public int DriverId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LicenseNumber { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime HireDate { get; set; }
        public string Status { get; set; }
    }
}
