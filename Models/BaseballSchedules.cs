using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 棒球赛事
    /// </summary>
    public class BaseballSchedules
    {
        [Key]
        public int GID { get; set; }
        
        public int OrderBy { get; set; }

        public string GameType { get; set; }

        public int AllianceID { get; set; }

        public DateTime GameDate { get; set; }

        public TimeSpan GameTime { get; set; }

        public int Number { get; set; }

        /// <summary>
        /// 比賽狀態；X=未開賽；S=已開賽；E=已結束；P=中止；C=取消；D=Delay(跟盤程式用)
        /// </summary>
        public string GameStates { get; set; }

        /// <summary>
        /// A队
        /// </summary>
        public int TeamAID { get; set; }


        /// <summary>
        /// B队
        /// </summary>
        public int TeamBID { get; set; }

        /// <summary>
        /// 球員編號(A投手)(目前沒用到)
        /// </summary>
        public int? PitcherAPID { get; set; }

        /// <summary>
        /// 球員編號(B投手)(目前沒用到)
        /// </summary>
        public int? PitcherBPID { get; set; }

        /// <summary>
        /// 球員編號(A打者)(目前沒用到)
        /// </summary>
        public int? BatAID { get; set; }

        /// <summary>
        /// 球員編號(B打者)(目前沒用到)
        /// </summary>
        public int? BatBID { get; set; }

        /// <summary>
        /// 球員編號(A下一個打者)(目前沒用到)
        /// </summary>
        public int? BatOrderA { get; set; }

        /// <summary>
        /// 球員編號(B下一個打者)(目前沒用到)
        /// </summary>
        public int? BatOrderB { get; set; }

        /// <summary>
        /// 操盤狀態；0=無；1=操盤中；2=自動操盤
        /// </summary>
        public int CtrlStates { get; set; }

        /// <summary>
        /// 操盤人員
        /// </summary>
        [DisplayFormat(NullDisplayText = "", ConvertEmptyStringToNull = true)]
        public string CtrlAdmin { get; set; }

        /// <summary>
        /// A队分数
        /// </summary>
        [DisplayFormat(NullDisplayText = "", ConvertEmptyStringToNull = true)]
        public string RunsA { get; set; }

        /// <summary>
        /// B队分数
        /// </summary>
        [DisplayFormat(NullDisplayText = "", ConvertEmptyStringToNull = true)]
        public string RunsB { get; set; }

        /// <summary>
        /// A队得分
        /// </summary>
        public int? RA { get; set; }

        /// <summary>
        /// A队安打次数
        /// </summary>
        public int? HA { get; set; }

        /// <summary>
        /// A队失误次数
        /// </summary>
        public int? EA { get; set; }

        /// <summary>
        /// B队得分
        /// </summary>
        public int? RB { get; set; }

        /// <summary>
        /// B队安打次数
        /// </summary>
        public int? HB { get; set; }

        /// <summary>
        /// B队失误次数
        /// </summary>
        public int? EB { get; set; }

        /// <summary>
        /// 坏球数
        /// </summary>
        public int B { get; set; }

        /// <summary>
        /// 好球数
        /// </summary>
        public int S { get; set; }

        /// <summary>
        /// 出局数
        /// </summary>
        public int O { get; set; }

        /// <summary>
        /// 垒包 壘包狀態；0=無；1=1壘；2=2壘；3=1，2壘；4=3壘；5=1，3壘；6=2，3壘；7=滿壘
        /// </summary>
        public int Bases { get; set; }

        /// <summary>
        /// 比賽備註
        /// </summary>
        [DisplayFormat(NullDisplayText = "", ConvertEmptyStringToNull = true)]
        public string TrackerText { get; set; }

        /// <summary>
        /// 記錄台棒動畫資料
        /// </summary>
        [DisplayFormat(NullDisplayText = "", ConvertEmptyStringToNull = true)]
        public string Record { get; set; }

        /// <summary>
        /// 比賽情況，如：倒數時間
        /// </summary>
        [DisplayFormat(NullDisplayText = "", ConvertEmptyStringToNull = true)]
        public string StatusText { get; set; }

        /// <summary>
        /// 0=作用中；1=已刪除此筆資料
        /// </summary>
        public Boolean IsDeleted { get; set; }

        /// <summary>
        /// 跟盘ID
        /// </summary>
        [DisplayFormat(NullDisplayText = "", ConvertEmptyStringToNull = true)]
        public string WebID { get; set; }

        /// <summary>
        /// 变动次数
        /// </summary>
        public int ChangeCount { get; set; }

        /// <summary>
        /// 创建赛事时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否补赛
        /// </summary>
        public Boolean IsReschedule { get; set; }

        /// <summary>
        /// 是否显示走势
        /// </summary>
        public Boolean ShowJS { get; set; }

        /// <summary>
        /// 变动时间
        /// </summary>
        public DateTime ChangeTime { get; set; }
    }
}
