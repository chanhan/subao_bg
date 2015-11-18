using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Models
{
    [Table("ModifyRecord")]
    public class ModifyRecord
    {
        public int ID { get; set; }
        public DateTime ChangeTime { get; set; }
        public int ActionStatus { get; set; }
        public int ItemCategory { get; set; }
        public string ByWho { get; set; }
        public string ByIP { get; set; }
        public string OldData { get; set; }
        public string NewData { get; set; }
        public string GameType { get; set; }
        public string Identifier { get; set; }
    }
}
