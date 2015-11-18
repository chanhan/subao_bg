using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class BKOSAllianceComparer : IEqualityComparer<BKOSAlliance>
    {
        public bool Equals(BKOSAlliance x, BKOSAlliance y)    //比较x和y对象是否相同，按照地址比较
        {
            return x.AllianceID == y.AllianceID && x.AllianceName == y.AllianceName && x.Display == y.Display;
        }
        public int GetHashCode(BKOSAlliance obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
