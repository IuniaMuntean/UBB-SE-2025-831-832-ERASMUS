using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace UBB_SE_2025_EUROTRUCKERS.Models
{
    [Table("roadfinancials")]
    public class RoadFinancials
    {
        // Composite primary key properties, matching the roads table.
        [Column("startcityid")]
        public int StartCityID { get; set; }
        
        [Column("endcityid")]
        public int EndCityID { get; set; }

        // Financial properties
        [Column("cost")]
        public float Cost { get; set; }
        
        [Column("revenue")]
        public float Revenue { get; set; }

        public Road Road { get; set; }

        public float GetDistance() => Road.distance;
        public float GetCost() => Cost;
        public float GetRevenue() => Revenue;
    }
    
}
