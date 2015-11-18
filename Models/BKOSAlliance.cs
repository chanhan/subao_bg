using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 奥讯篮球联盟
    /// </summary>
    [Table("OSAlliance")]
    public class BKOSAlliance
    {
        /// <summary>
        /// 联盟ID
        /// </summary>
        [Key]
        public int AllianceID { get; set; }

        /// <summary>
        /// 联盟名（网页抓取）
        /// </summary>
        [StringLength(50)]
        public string AllianceName { get; set; }

        /// <summary>
        /// 联盟名（前台显示）
        /// </summary>
        [StringLength(50)]
        public string ShowName { get; set; }

        /// <summary>
        /// 是否显示该联盟的赛事
        /// </summary>
        public bool Display { get; set; }

        /// <summary>
        /// 联盟网址
        /// </summary>
        [StringLength(50)]        
        public string AllianceUrl { get; set; }   
        /// <summary>
        /// 联盟排序ID
        /// </summary>
        public int? AlianceSortID { get; set; }

        [NotMapped]
        public int Changed { get; set; }
    }
}
