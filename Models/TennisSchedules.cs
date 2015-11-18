using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Models
{
    public class TennisSchedules
    {
        /// <summary>
        /// 賽程編號
        /// </summary>
        [Key]
        public int GID { get; set; }
        /// <summary>
        /// 聯盟編號
        /// </summary>
        public int AllianceID { get; set; }
        public DateTime GameDate { get; set; }
        public TimeSpan GameTime { get; set; }
        public string GameStates { get; set; }
        public int TeamAID { get; set; }
        public int TeamBID { get; set; }
        public string RunsA { get; set; }
        public string RunsB { get; set; }
        public string RA { get; set; }
        public string RB { get; set; }
        public int WN { get; set; }
        public int PR { get; set; }
        public string WebID { get; set; }
        public string TrackerText { get; set; }
        public bool IsDeleted { get; set; }
        public int ChangeCount { get; set; }
        public DateTime CreateTime { get; set; }
        public int OrderBy { get; set; }
        [NotMapped]
        public int Changed { get; set; }
    }
}
