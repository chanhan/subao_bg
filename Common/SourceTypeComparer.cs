using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class SourceTypeComparer : IEqualityComparer<SourceType>
    {
        public bool Equals(SourceType x, SourceType y)    //比较x和y对象是否相同，按照地址比较
        {
            return x.SourceID == y.SourceID && x.GameSource == y.GameSource;
        }
        public int GetHashCode(SourceType obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
