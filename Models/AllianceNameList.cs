using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Models
{
    [Table("AllianceNameList")]
    public class AllianceNameList
    {
        [Key]
        public int GUID { get; set; }

        public string AllianceType { get; set; }

        [StringLength(30)]
        public string SimpleName { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        public string LanguageCode { get; set; }

        public string Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public string Modifier { get; set; }

        public DateTime? ModifyTime { get; set; }
    }
}
