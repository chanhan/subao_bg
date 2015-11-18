using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Models
{
    [Table("AFBAlliance")]
    public class AFBAlliance
    {
        [Key]
        public int AllianceID { get; set; }
        public string GameType { get; set; }
        public int Lever { get; set; }
        public string AllianceName { get; set; }
        public string LeverOther { get; set; }
        public bool IsDeleted { get; set; }
        public string AllianceUrl { get; set; }
    }
}
