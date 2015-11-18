using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Models
{
    [Table("ScoreModifyRecord")]
    public class ScoreModifyRecord
    {
        [Key]
        public int UID { get; set; }
        public int ScheduleID { get; set; }
        public string GameType { get; set; }
        public string RunsAOld { get; set; }
        public string RunsBOld { get; set; }
        public int? RAOld { get; set; }
        public int? HAOld { get; set; }
        public int? EAOld { get; set; }
        public int? RBOld { get; set; }
        public int? HBOld { get; set; }
        public int? EBOld { get; set; }
        public string RunsANew { get; set; }
        public string RunsBNew { get; set; }
        public int? RANew { get; set; }
        public int? HANew { get; set; }
        public int? EANew { get; set; }
        public int? RBNew { get; set; }
        public int? HBNew { get; set; }
        public int? EBNew { get; set; }
        public string StatusTextOld { get; set; }
        public string StatusTextNew { get; set; }
        public DateTime ModifyTime { get; set; }
        public string ModifyUser { get; set; }
        public string IpAddr { get; set; }
        public int ModifyItem { get; set; }
        public string GameDateOld { get; set; }
        public string GameTimeOld { get; set; }
        public string GameDateNew { get; set; }
        public string GameTimeNew { get; set; }
    }
}
