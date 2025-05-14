using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UBB_SE_2025_EUROTRUCKERS.Models
{
    [Table("users")]
    public class User
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("username")]
        public String Username { get; set; }
        [Column("password")]
        public String Password { get; set; }
    }
}
