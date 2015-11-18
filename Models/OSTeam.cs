using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    ///奥讯篮球队伍 
    /// </summary>
    [Table("OSTeam")]
    public class OSTeam
    {
        /// <summary>
        /// 队伍ID
        /// </summary>
        [Key]
        public int TeamID { get; set; }

        /// <summary>
        /// 队伍名（网页抓取）
        /// </summary>
        [StringLength(50)]
        public string TeamName { get; set; }

        /// <summary>
        /// 队伍名（前台显示）
        /// </summary>
        [StringLength(50)]
        public string ShowName { get; set; }

        /// <summary>
        /// 队伍的联盟ID
        /// </summary>
        public int AllianceID { get; set; }
    }
}
