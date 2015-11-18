using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models.ViewModel;
namespace Common
{
    public class IHBFAllianceComparer : IEqualityComparer<IceHockey>
    {
        public bool Equals(IceHockey x, IceHockey y)    //比较x和y对象是否相同，按照地址比较
        {
            return x.Alliance == y.Alliance;
        }
        public int GetHashCode(IceHockey obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
