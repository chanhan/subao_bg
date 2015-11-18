using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class baseballComparer : IEqualityComparer<Models.ViewModel.Baseball>
    {
        public bool Equals(Models.ViewModel.Baseball x, Models.ViewModel.Baseball y)    //比较x和y对象是否相同，按照地址比较
        {
            return x.Alliance == y.Alliance;
        }
        public int GetHashCode(Models.ViewModel.Baseball obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
