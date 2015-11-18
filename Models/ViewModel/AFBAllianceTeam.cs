using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.ViewModel
{
    public class AFBAllianceTeam
    {
        public int? AllianceID { get; set; }
        public string AllianceName { get; set; }
        public int? Lever { get; set; }
        public string LeverOther { get; set; }
        public int? TeamID { get; set; }
        public string TeamName { get; set; }
        public string ShowName { get; set; }
        public int? W { get; set; }
        public int? T { get; set; }
        public int? L { get; set; }
        public string SourceID { get; set; }
        public string GameType { get; set; }
        public string WebName { get; set; }
    }
}
