using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class IceHockeyComparer : IEqualityComparer<Models.ViewModel.IceHockey>
    {
        public bool Equals(Models.ViewModel.IceHockey x, Models.ViewModel.IceHockey y)
        {
            return x.Alliance == y.Alliance;
        }

        public int GetHashCode(Models.ViewModel.IceHockey obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
