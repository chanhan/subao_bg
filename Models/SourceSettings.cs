using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Models
{
    [Table("SourceSettings")]
    public class SourceSettings
    {
        [Key]
        public int SID { get; set; }
        public int AllianceID { get; set; }
        public string GameType { get; set; }
        public string SourceType { get; set; }
    }
}
