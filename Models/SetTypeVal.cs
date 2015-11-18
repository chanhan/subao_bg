using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Models
{
    [Table("SetTypeVal")]
    public class SetTypeVal
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string Val { get; set; }
        public string Language { get; set; }
    }
}
