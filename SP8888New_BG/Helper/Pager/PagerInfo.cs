using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Helper.Pager
{
    public class PagerInfo
    {
        public PagerInfo(int currentPageIndex, int pageSize, int recordCount)
        {
            CurrentPageIndex = currentPageIndex;
            PageSize = pageSize;
            RecordCount = recordCount;
        }
        public int RecordCount { get; set; }
        public int CurrentPageIndex { get; set; }
        public int PageSize { get; set; }
    }
}