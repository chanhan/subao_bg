using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.ViewModel
{
    public class BasketBall
    {
        public string GameSource { get; set; }
        public string SourceID { get; set; }
        public string AllianceName { get; set; }
        public string CtrlAdmin { get; set; }
        public int? TeamAID { get; set; }
        public string TeamAName { get; set; }
        public string TeamAAlliance { get; set; }
        public int? TeamBID { get; set; }
        public string TeamBName { get; set; }
        public string TeamBAlliance { get; set; }
        public int GID { get; set; }
        public int AllianceID { get; set; }
        public string GameType { get; set; }
        public DateTime GameDate { get; set; }
        public TimeSpan GameTime { get; set; }
        public string GameStates { get; set; }
        public int CtrlStates { get; set; }
        public int Number { get; set; }
        public string WebID { get; set; }
        public int OrderBy { get; set; }
        public bool ShowJS { get; set; }
        public bool IsDeleted { get; set; }
        public int? TeamAAllianceID { get; set; }
        public int? TeamBAllianceID { get; set; }
        public string TrackerText { get; set; }
    }
}
