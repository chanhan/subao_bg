using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Models
{
    [Table("AFBSchedules")]
    public class AFBSchedules
    {
        [Key]
        public int GID { get; set; }
        public int OrderBy { get; set; }
        public string GameType { get; set; }
        public int AllianceID { get; set; }
        public int Number { get; set; }
        public DateTime GameDate { get; set; }
        public TimeSpan GameTime { get; set; }
        public string GameStates { get; set; }
        public int TeamAID { get; set; }
        public int TeamBID { get; set; }
        public int CtrlStates { get; set; }
        public string CtrlAdmin { get; set; }
        public string RunsA { get; set; }
        public string RunsB { get; set; }
        public int RA { get; set; }
        public int RB { get; set; }
        public string WebID { get; set; }
        public string TrackerText { get; set; }
        public string StatusText { get; set; }
        public bool IsDeleted { get; set; }
        public int ChangeCount { get; set; }
        public DateTime CreateTime { get; set; }
        public bool ShowJS { get; set; }

        [NotMapped]
        [JsonIgnore]
        public int IsDeletedChanged { get; set; }

        [NotMapped]
        [JsonIgnore]
        public int ShowJSChanged { get; set; }
        [NotMapped]
        [JsonIgnore]
        public string TeamAName { get; set; }
        [NotMapped]
        [JsonIgnore]
        public string TeamBName { get; set; }
    }
}
