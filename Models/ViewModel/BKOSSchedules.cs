using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.ViewModel
{
    public class BKOSSchedules
    {
        public int GID { get; set; }
        public int AllianceID { get; set; }
        public string AllianceName { get; set; }
        public bool AllianceDisPlay { get; set; }
        public int TeamAID { get; set; }
        public int TeamBID { get; set; }
        public string TeamAName { get; set; }
        public string TeamBName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string GameStates { get; set; }
        public bool ShowJS { get; set; }
        public string TrackerText { get; set; }
        public bool IsDeleted { get; set; }
        public int CtrlStates { get; set; }
        public string GameType { get; set; }
    }
}