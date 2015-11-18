using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 篮球队伍 
    /// </summary>
    [Table("BasketballTeam")]
    public class BasketballTeam
    {
        /// <summary>
        /// 隊伍編號
        /// </summary>
        [Key]
        public int TeamID { get; set; }

        /// <summary>
        /// 聯盟類型
        /// </summary>
        [StringLength(10)]
        public string GameType { get; set; }

        /// <summary>
        /// 隊伍名稱
        /// </summary>
        [StringLength(50)]
        public string TeamName { get; set; }

        /// <summary>
        /// 隊伍顯示名稱
        /// </summary>
        [StringLength(50)]
        public string ShowName { get; set; }

        /// <summary>
        /// 網頁名稱，使用在建立賽程，或跟盤比賽
        /// </summary>
        [StringLength(50)]
        public string WebName { get; set; }

        /// <summary>
        /// 聯盟編號
        /// </summary>
        public int AllianceID { get; set; }

        [JsonIgnore]
        [NotMapped]
        public string AllianceName { get; set; }


        /// <summary>
        /// 勝利次數
        /// </summary>
        public int W { get; set; }

        /// <summary>
        /// 失敗次數
        /// </summary>
        public int L { get; set; }

        /// <summary>
        /// 平手次數
        /// </summary>
        public int T { get; set; }

        /// <summary>
        /// 0=作用中；1=已刪除此筆資料
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 来源网ID
        /// </summary>
        [StringLength(10)]
        public string SourceID { get; set; }
    }
}
