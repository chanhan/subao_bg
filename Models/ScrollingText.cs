using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Models
{
    [Table("Scrolling_Text")]
    public class ScrollingText
    {
        [Key]
        public int ScrollingTextID { get; set; }
        public int SettingType { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        [Column("ScrollingText")]
        public string Text { get; set; }
        public string HyperLink { get; set; }
        public int Liveticker { get; set; }
        public int OrderBy { get; set; }
        public int? Visible { get; set; }
        public string LanguageCode { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateTime { get; set; }
        public string Modifier { get; set; }
        public DateTime? ModifyTime { get; set; }
        [NotMapped]
        public bool IsAdd { get;set;}
    }
}
