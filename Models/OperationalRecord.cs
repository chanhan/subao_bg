using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("OperationalRecord")]
    public class OperationalRecord
    {
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 更改时间
        /// </summary>
        public DateTime Chgtime { get; set; }
        /// <summary>
        /// 动作  1=修改；2=刪除；3=新增
        /// </summary>
        public string ActionStatus { get; set; }

        /// <summary>
        /// 類別；1=聯盟；2=隊伍；4=賽程；5=帳號；6=訊息管理；7=登入IP管理；8=名稱對應表
        /// </summary>
        public string ItemCategory { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public string ByWho { get; set; }
        /// <summary>
        /// 操作IP
        /// </summary>
        public string ByIP { get; set; }
        /// <summary>
        /// 修改内容(HTML)
        /// </summary>
        public string Content { get; set; }
    }
}
