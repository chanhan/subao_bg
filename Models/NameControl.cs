using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Models
{
    [Table("NameControl")]
    public class NameControl
    {
        public int ID { get; set; }
        public string Category { get; set; }
        public string Langx { get; set; }
        public string GameType { get; set; }
        public string GTLangx { get; set; }
        public string AppType { get; set; }
        public string SourceText { get; set; }
        public string ChangeText { get; set; }
        public int Indexs { get; set; }
        public DateTime ChangeDate { get; set; }
        [NotMapped]
        public bool IsAdd { get; set; }
    }
}
