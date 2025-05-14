using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace UBB_SE_2025_EUROTRUCKERS.Models
{
    [Table("roads")]
    public class Road
    {
        [Column("id")]
        public int id { get; set; }

        [Column("startcityid")]
        public int startCityID { get; set; }

        [Column("endcityid")]
        public int endCityID { get; set; }

        [Column("distance")]
        public float distance { get; set; }

        // asa mi-a dat Copilotu ca sa pot sa definesc foreign key-ul in TransportDbContext.
        // Intreaba-l pe Razvan cum sa fac sa mearga
        public City StartCity { get; set; }
        public City EndCity { get; set; }
    }
}
