using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UBB_SE_2025_EUROTRUCKERS.Models
{
    [Table("orders")]
    public class Order
    {
        [Column("order_id")]
        public int OrderId { get; set; }
        [Column("client_name")]
        public string ClientName { get; set; }
        [Column("cargo_type")]
        public string CargoType { get; set; }
        [Column("cargo_weight")]
        public double CargoWeight { get; set; }
        [Column("source_city_id")]
        public City SourceCity { get; set; }
        [Column("destination_city_id")]
        public City DestinationCity { get; set; }
    }
}
