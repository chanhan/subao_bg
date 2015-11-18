using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.QueryModel
{
    public class AllianceNameListQuery
    {
        public int ddlItem { get; set; }
        public string ddlGameType { get; set; }

        public string ddlLanguagecode { get; set; }
        public string FullName { get; set; }
        public string SimpleName { get; set; }
    }
}
