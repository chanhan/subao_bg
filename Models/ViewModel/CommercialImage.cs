using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.ViewModel
{
    public class CommercialImage
    {
        public string ImageName { get; set; }
        public string Link { get; set; }
        public string SourceUrl { get; set; }
        public override string ToString()
        {
            return string.Format("{0}{1}", Link, string.IsNullOrEmpty(SourceUrl) ? string.Empty : "#" + SourceUrl);
        }
    }
}
