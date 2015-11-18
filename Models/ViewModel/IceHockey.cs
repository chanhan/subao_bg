using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.ViewModel
{
    public class IceHockey
    {

        public int GID { get; set; }

        public string GameType { get; set; }
        public int Number { get; set; }

        public string Alliance { get; set; }

        public DateTime GameDate { get; set; }

        public TimeSpan GameTime { get; set; }

        /// <summary>
        /// 比賽狀態；X=未開賽；S=已開賽；E=已結束；P=中止；C=取消；D=Delay(跟盤程式用)
        /// </summary>
        public string GameStates { get; set; }

        /// <summary>
        /// A队
        /// </summary>
        public string TeamA { get; set; }

        /// <summary>
        /// B队
        /// </summary>
        public string TeamB { get; set; }

        /// <summary>
        /// 操盤狀態；0=無；1=操盤中；2=自動操盤
        /// </summary>
        public int CtrlStates { get; set; }

        /// <summary>
        /// 跟盘ID
        /// </summary>       
        public string WebID { get; set; }


        /// <summary>
        /// 是否显示走势
        /// </summary>
        public Boolean ShowJS { get; set; }

        /// <summary>
        /// 是否显示1隐藏0
        /// </summary>
        public Boolean Display { get; set; }
        public string CtrlAdmin { get; set; }
        /// <summary>
        /// 是否存在修改分數記錄
        /// </summary>
        public bool IsScoreModifyRecord;

        /// <summary>
        /// 狀態文字     BF冰球用
        /// </summary>
        public string TrackerText { get; set; }

        /// <summary>
        /// 联盟是否显示（X/O）  BF冰球用
        /// </summary>
        public bool AllianceDisplay { get; set; }
    }
}
