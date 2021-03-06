﻿using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
   public class BasketBallAllianceTeamComparer: IEqualityComparer<BasketBallAllianceTeam>
    {
        public bool Equals(BasketBallAllianceTeam x, BasketBallAllianceTeam y)    //比较x和y对象是否相同，按照地址比较
        {
            return x.AllianceID == y.AllianceID && x.AllianceName == y.AllianceName;
        }
        public int GetHashCode(BasketBallAllianceTeam obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
