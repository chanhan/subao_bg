using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 棒球队伍
    /// </summary>
    [Table("BaseballTeam")]
    public class BaseballTeam
    {
        [Key]
        public int TeamID { get; set; }

        public string  GameType { get; set; }

        public string  TeamName { get; set; }

        public string ShowName { get; set; }

        [DisplayFormat(NullDisplayText = "", ConvertEmptyStringToNull = true)]
        public string WebName { get; set; }

        public int AllianceID { get; set; }

        [NotMapped]
        public string AllianceName { get; set; }

        public int W { get; set; }

        public int L { get; set; }

        public int T { get; set; }

        public Boolean IsDeleted { get; set; }


        [DisplayFormat(NullDisplayText = "", ConvertEmptyStringToNull = true)]
        public string SourceID { get; set; }
    }
}
