using Newtonsoft.Json;
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
    /// 篮球赛事
    /// </summary>
    public class BasketballSchedules
    {
        /// <summary>
        /// 賽程編號
        /// </summary>
        [Key]
        public int GID { get; set; }

        /// <summary>
        /// 赛程排序
        /// </summary>
        [Required]
        public int OrderBy { get; set; }

        /// <summary>
        /// 聯盟類型
        /// </summary>
        [Required]
        [StringLength(10)]
        public string GameType { get; set; }

        /// <summary>
        /// 聯盟編號
        /// </summary>
        [Required]
        public int AllianceID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int Number { get; set; }

        /// <summary>
        /// 比賽日期
        /// </summary>
        [DisplayName("比賽日期")]
        public DateTime GameDate { get; set; }

        /// <summary>
        /// 比賽時間
        /// </summary>
        [DisplayName("比賽時間")]
        public TimeSpan GameTime { get; set; }

        /// <summary>
        /// 比賽狀態；X=未開賽；S=已開賽；E=已結束；P=中止；C=取消；D=Delay(跟盤程式用)
        /// </summary>
        [StringLength(50)]
        public string GameStates { get; set; }

        /// <summary>
        /// 隊伍編號(客隊)
        /// </summary>
        public int TeamAID { get; set; }

        /// <summary>
        /// 隊伍編號(主隊)
        /// </summary>
        public int TeamBID { get; set; }

        /// <summary>
        /// 操盤狀態；0=無；1=操盤中；2=自動操盤；（奥讯篮球 修改全部：4 修改狀態：3 修改比分：1 自动操盘：0、2 其中2表示先为手动操盘，后改为自动 ）
        /// </summary>
        public int CtrlStates { get; set; }

        /// <summary>
        /// 操盤人員
        /// </summary>
        [StringLength(50)]
        public string CtrlAdmin { get; set; }

        /// <summary>
        /// 比賽分數(客隊)
        /// </summary>
        [StringLength(100)]
        public string RunsA { get; set; }

        /// <summary>
        /// 比賽分數(主隊)
        /// </summary>
        [StringLength(100)]
        public string RunsB { get; set; }

        /// <summary>
        /// 总分（客隊）
        /// </summary>
        public int RA { get; set; }

        /// <summary>
        ///  总分（主隊）
        /// </summary>
        public int RB { get; set; }

        /// <summary>
        /// 跟盤用的ID(网页抓取)
        /// </summary>
        [StringLength(50)]
        public string WebID { get; set; }

        /// <summary>
        /// 比賽过程的文字內容
        /// </summary>
        public string TrackerText { get; set; }

        /// <summary>
        /// 比賽状态的文字內容
        /// </summary>
        [StringLength(50)]
        public string StatusText { get; set; }

        /// <summary>
        /// 是否显示（0 显示；1：不显示）
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 記錄跟盤資料
        /// </summary>
        public string Record { get; set; }

        /// <summary>
        /// 資料變更的次數
        /// </summary>
        [Required]
        public int ChangeCount { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否手动修改过状态文字
        /// </summary>
        public bool IsTracker { get; set; }


        /// <summary>
        /// 是否显示走势
        /// </summary>
        public bool ShowJS { get; set; }
         
        [NotMapped]
        [JsonIgnore]
        public int IsDeletedChanged { get; set; }

        [NotMapped]
        [JsonIgnore]
        public int ShowJSChanged { get; set; }
        [NotMapped]
        [JsonIgnore]
        public int TrackerTextChanged { get; set; }

    }
}
