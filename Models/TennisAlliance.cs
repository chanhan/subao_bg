using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Models
{
    [Table("TNAlliance")]
    public class TennisAlliance
    {
        [Key]
        public int AllianceID { get; set; }
        public string AllianceName { get; set; }
        public string ShowName { get; set; }
        public bool Display { get; set; }
        public string AllianceUrl { get; set; }   
        [NotMapped]
        public int Changed { get; set; }
    }
}
