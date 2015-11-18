using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Models
{
    [Table("SourceType")]
    public class SourceType
    {
        [Key]
        [StringLength(10)]
        public string SourceID { get; set; }
        [StringLength(10)]
        public string GameType { get; set; }
        [StringLength(10)]
        public string GameSource { get; set; }
    }
}
