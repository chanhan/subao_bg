using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 收藏夾設定
    /// </summary>
    [Table("FavoriteInfo")]
    public class FavoriteInfo
    {
        [Key]
        public int FavoriteID { get; set; }
        public string SimplifiedDisplay { get; set; }
        public string TraditionalDisplay { get; set; }
        public string JumpUrl { get; set; }
        public string SimplifiedPrompt { get; set; }
        public string TraditionalPrompt { get; set; }
    }
}
