using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.ViewModel
{
 public   class AFBScoreRecord
    {
        public string AllianceName { get; set; }
        public string TeamAName { get; set; }
        public string TeamBName { get; set; }
        public DateTime GameDate { get; set; }
        public TimeSpan GameTime { get; set; }
        public string RunsAOld { get; set; }
        public string RunsBOld { get; set; }
        public string RunsANew { get; set; }
        public string RunsBNew { get; set; }
        //public string StatusTextOld { get; set; }
        //public string StatusTextNew { get; set; }
        public DateTime ModifyTime { get; set; }
        public string ModifyUser { get; set; }
        public string IpAddr { get; set; }
        //public int ModifyItem { get; set; }
        //public string GameDateOld { get; set; }
        //public string GameTimeOld { get; set; }
        //public string GameDateNew { get; set; }
        //public string GameTimeNew { get; set; }
        public int? RAOld { get; set; }
        public int? RBOld { get; set; }
        public int? RANew { get; set; }
        public int? RBNew { get; set; }
    }
}
