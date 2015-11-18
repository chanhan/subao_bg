using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Models
{
    [Table("BasketballAlliance")]
    public class BasketballAlliance
    {
        [Key]
        public int AllianceID { get; set; }

        [StringLength(10)]
        public string GameType { get; set; }

        public int Lever { get; set; }

        [StringLength(50)]
        public string AllianceName { get; set; }

        [StringLength(50)]
        public string LeverOther { get; set; }

        public bool IsDeleted { get; set; }
        public string AllianceUrl { get; set; }
    }
}
