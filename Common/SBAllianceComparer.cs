using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models.ViewModel;
namespace Common
{
    public class SBAllianceComparer : IEqualityComparer<FollballAlliance>
    {
        public bool Equals(FollballAlliance x, FollballAlliance y)    //比较x和y对象是否相同，按照地址比较
        {
            return x.AllianceName == y.AllianceName;
        }
        public int GetHashCode(FollballAlliance obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
