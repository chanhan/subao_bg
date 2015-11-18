using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.ViewModel
{
    public class Tennis
    {
        public int GID { get; set; }
        public int AllianceID { get; set; }
        public string AllianceName { get; set; }
        public DateTime GameDate { get; set; }
        public TimeSpan GameTime { get; set; }
        public string GameStates { get; set; }
        public int TeamAID { get; set; }
        public int TeamBID { get; set; }
        public string TeamAName { get; set; }
        public string TeamBName { get; set; }
        public bool IsDeleted { get; set; }
        public bool AllianceDisPlay { get; set; }
        //public int OrderBy { get; set; }
        public string AllianceShowName { get; set; }
    }
}
