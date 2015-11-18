using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 登录IP表
    /// </summary>
    [Table("IPAccess")]
    public class IPAccess
    {
        /// <summary>
        /// 自增id
        /// </summary>
        [Key]
        public int uid { get; set; }
        /// <summary>
        /// ip地址
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 变更人员
        /// </summary>
        public string ModifyUser { get; set; }
        /// <summary>
        /// 变更时间
        /// </summary>
        public DateTime ModifyTime { get; set; }
        /// <summary>
        /// 是否允许变更  true：不允许，false：允许
        /// </summary>
        public bool AllowChange { get; set; }
    }
}
