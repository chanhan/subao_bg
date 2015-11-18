using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 登錄用戶列表
    /// </summary>
    [Table("Employee")]
    public class Employee
    {
        /// <summary>
        /// 操作人員編號
        /// </summary>
        [Key]
        [Required(ErrorMessage="* 帐号不能为空！")]
        public string EmployeeName { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        [Required(ErrorMessage="* 密码不能为空！")]
        public string Password { get; set; }

        /// <summary>
        /// 暱稱
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 登陸日期
        /// </summary>
        public DateTime? LoginDate { get; set; }

        /// <summary>
        /// 登錄時間
        /// </summary>
        public TimeSpan? LoginTime { get; set; }

        /// <summary>
        /// 等級
        /// </summary>
        public string Rank { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Active { get; set; }

        /// <summary>
        /// 網頁 Session ID，操盤軟體使用
        /// </summary>
        public string Sid { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IP { get; set; }
    }
}
