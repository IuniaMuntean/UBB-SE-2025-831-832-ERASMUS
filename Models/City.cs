using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UBB_SE_2025_EUROTRUCKERS.Models
{
    [Table("cities")]
    public class City
    {
        [Column("id")]
        public int id { get; set; }

        [Column("name")]
        public string name { get; set; }

        [Column("x")]
        public float x;

        [Column("y")]
        public float y;

        public ICollection<Road> Roads { get; set; }

        public City()
        {
            this.id = 0;
            this.name = string.Empty;
            this.x = 0;
            this.y = 0;
        }

        public City(int id, string name, int x, int y)
        {
            this.id = id;
            this.name = name;
            this.x = x;
            this.y = y;
        }
        public City(City other)
        {
            this.id = other.id;
            this.name = other.name;
            this.x = other.x;
            this.y = other.y;
        }
    }
}
