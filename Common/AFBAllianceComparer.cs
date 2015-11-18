using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class AFBAllianceComparer : IEqualityComparer<AFBAlliance>
    {
        public bool Equals(AFBAlliance x, AFBAlliance y)    //比较x和y对象是否相同，按照地址比较
        {
            return x.AllianceID == y.AllianceID && x.AllianceName == y.AllianceName;
        }
        public int GetHashCode(AFBAlliance obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
