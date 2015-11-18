using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    /// <summary>
    /// 足球赛事
    /// </summary>
    public class FootballSchedules
    {
        [Key]
        public int ID { get; set; }

        public string GameType { get; set; }

        public string WebID { get; set; }

        public DateTime GameDate { get; set; }

        /// <summary>
        /// 联盟
        /// </summary>
        public string AL { get; set; }

        /// <summary>
        /// 比赛时间
        /// </summary>
        public string KO { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [DisplayFormat(NullDisplayText = "", ConvertEmptyStringToNull = true)]
        public string UP { get; set; }

        /// <summary>
        /// 主队
        /// </summary>
        public string NA { get; set; }

        /// <summary>
        /// 客队
        /// </summary>
        public string NB { get; set; }

        /// <summary>
        /// 主队总分
        /// </summary>
        [DisplayFormat(NullDisplayText = "", ConvertEmptyStringToNull = true)]
        public int? OA { get; set; }

        /// <summary>
        /// 客队名称
        /// </summary>
        [DisplayFormat(NullDisplayText = "", ConvertEmptyStringToNull = true)]
        public int? OB { get; set; }

        /// <summary>
        /// 主队半场
        /// </summary>
        [DisplayFormat(NullDisplayText = "", ConvertEmptyStringToNull = true)]
        public int? RA { get; set; }

        /// <summary>
        /// 客队半场
        /// </summary>
        [DisplayFormat(NullDisplayText = "", ConvertEmptyStringToNull = true)]
        public int? RB { get; set; }

        /// <summary>
        ///主队红牌
        /// </summary>
        [DisplayFormat(NullDisplayText = "", ConvertEmptyStringToNull = true)]
        public string NAR { get; set; }

        /// <summary>
        /// 客队红牌
        /// </summary>
        [DisplayFormat(NullDisplayText = "", ConvertEmptyStringToNull = true)]
        public string NBR { get; set; }

        /// <summary>
        /// 主队黄牌
        /// </summary>
        [DisplayFormat(NullDisplayText = "", ConvertEmptyStringToNull = true)]
        public string CA { get; set; }

        /// <summary>
        /// 客队黄牌
        /// </summary>
        [DisplayFormat(NullDisplayText = "", ConvertEmptyStringToNull = true)]
        public string CB { get; set; }

        /// <summary>
        /// 变动累加
        /// </summary>        
        public int? C { get; set; }

        /// <summary>
        ///  操盘状态 1=修改全部， 21 存储比分， 31 存储状态
        /// </summary>
        public int CtrlStates { get; set; }
        public string CtrlAdmin { get; set; }
        public int Orderby { get; set; }
        /// <summary>
        /// 是否存在修改分數記錄
        /// </summary>
        [NotMapped]
        public bool IsScoreModifyRecord { get; set; }
    }
}
