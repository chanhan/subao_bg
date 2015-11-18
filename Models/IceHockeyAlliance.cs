using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 冰球联盟
    /// </summary>
    [Table("IceHockeyAlliance")]
    public class IceHockeyAlliance
    {
        [Key]
        public int AllianceID { get; set; }

        public string GameType { get; set; }

        public int Lever { get; set; }

        public string AllianceName { get; set; }

        public string LeverOther { get; set; }

        public bool Display { get; set; }

        [Column("AlianceSortID")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public int? AllianceSortID { get; set; }

        [DisplayFormat(NullDisplayText = "", ConvertEmptyStringToNull = true)]
        public string ShowName { get; set; }

        public DateTime? CreateDate { get; set; }

        public string AllianceUrl { get; set; }

    }
}
