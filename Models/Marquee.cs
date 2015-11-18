using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Models
{
    [Table("Marquee")]
    public class Marquee
    {
        public int ID { get; set; }
        public string GameType { get; set; }
        public string MessageTW { get; set; }
        public string MessageCN { get; set; }
        public string MessageUS { get; set; }
        public string EnableYN { get; set; }
        public string AutoCloseYN { get; set; }
        public DateTime EffectiveTime { get; set; }
        public DateTime ChgTime { get; set; }
        [NotMapped]
        public bool IsAdd { get; set; }
    }
}
