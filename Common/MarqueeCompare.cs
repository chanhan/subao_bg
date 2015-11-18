using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
   public class MarqueeCompare:IComparer<string>
    {
       private string data = "MYN";
        public int Compare(string x, string y)
        {
            int indexX = data.IndexOf(x);
            int indexY = data.IndexOf(y);
            if (indexX >= 0 && indexY >= 0) return indexX - indexY;
            return 0;
        }
    }
}
