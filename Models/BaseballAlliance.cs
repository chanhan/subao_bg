using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 棒球联盟
    /// </summary>
    [Table("BaseballAlliance")]
    public class BaseballAlliance
    {
        [Key]
        public int AllianceID { get; set; }

        public string GameType { get; set; }

        public int Lever { get; set; }

        public string AllianceName { get; set; }

        /// <summary>
        /// 組合上層等級的資料
        /// </summary>
        public string LeverOther { get; set; }

        /// <summary>
        /// 0=作用中；1=已刪除此筆資料
        /// </summary>
        public Boolean IsDeleted { get; set; }

        /// <summary>
        /// 聯盟鏈接網址
        /// </summary>
        public string AllianceUrl { get; set; }
    }
}
