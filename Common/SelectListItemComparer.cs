using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Common
{
    public class SelectListItemComparer : IEqualityComparer<SelectListItem>
    {
        public bool Equals(SelectListItem x, SelectListItem y)    //比较x和y对象是否相同，按照地址比较
        {
            return x.Value == y.Value && x.Text == y.Text;
        }
        public int GetHashCode(SelectListItem obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
