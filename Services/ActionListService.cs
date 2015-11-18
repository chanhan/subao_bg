using IServices;
using Models.ViewModel;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
    public class ActionListService : RepositoryBase<ActionList>, IActionListService
    {
        public ActionListService(IDatabaseFactory databaseFactory, IUser IUserService)
            : base(databaseFactory, IUserService)
        {
        }

        /// <summary>
        /// 篮球
        /// </summary>
        /// <param name="gamedate"></param>
        /// <param name="gametype"></param>
        /// <returns></returns>
        public List<BasketBall> GetBasketball(DateTime gamedate, string gametype)
        {
            var linq = from bsksd in db.BasketballSchedules
                       where bsksd.CtrlStates != 0 && bsksd.GameDate.CompareTo(gamedate) == 0
                       join bskball in db.BasketballAlliance on bsksd.AllianceID equals bskball.AllianceID
                       join bskt1 in db.BasketballTeam on bsksd.TeamAID equals bskt1.TeamID
                       join bskt2 in db.BasketballTeam on bsksd.TeamBID equals bskt2.TeamID
                       orderby bsksd.GameTime
                       select new BasketBall
                       {
                           AllianceName = bskball.AllianceName,
                           GameType = bsksd.GameType,
                           CtrlAdmin = bsksd.CtrlAdmin,
                           TeamAName = bskt1.ShowName,
                           TeamBName = bskt2.ShowName,
                           GID = bsksd.GID,
                           AllianceID = bsksd.AllianceID,
                           GameDate = bsksd.GameDate,
                           GameTime = bsksd.GameTime,
                           GameStates = bsksd.GameStates
                       };
            if (gametype != "AAAA")
            {
                linq = linq.Where(m => m.GameType.CompareTo(gametype) == 0);
            }
            return linq.ToList();
        }


        /// <summary>
        /// 棒球
        /// </summary>
        /// <param name="gamedate"></param>
        /// <param name="gametype"></param>
        /// <returns></returns>
        public List<Baseball> Getbaseball(DateTime gamedate, string gametype)
        {
            var linq = from bsbsd in db.BaseballSchedules
                       where bsbsd.CtrlStates != 0 && bsbsd.GameDate.CompareTo(gamedate) == 0
                       join bsball in db.BaseballAlliance on bsbsd.AllianceID equals bsball.AllianceID
                       join bsbt1 in db.BaseballTeam on bsbsd.TeamAID equals bsbt1.TeamID
                       join bsbt2 in db.BaseballTeam on bsbsd.TeamBID equals bsbt2.TeamID
                       orderby bsbsd.GameTime
                       select new Baseball
                           {
                               GameType = bsbsd.GameType,
                               CtrlAdmin = bsbsd.CtrlAdmin,
                               TeamA = bsbt1.ShowName,
                               TeamB = bsbt2.ShowName,
                               GID = bsbsd.GID,
                               Alliance = bsball.AllianceName,
                               GameDate = bsbsd.GameDate,
                               GameTime = bsbsd.GameTime,
                               GameStates = bsbsd.GameStates,
                           };
            if (gametype != "AAAA")
            {
                linq = linq.Where(m => m.GameType.CompareTo(gametype) == 0);
            }
            return linq.ToList();
        }

        /// <summary>
        /// 冰球
        /// </summary>
        /// <param name="gamedate"></param>
        /// <param name="gametype"></param>
        /// <returns></returns>
        public List<IceHockey> GetIceHockey(DateTime gamedate, string gametype)
        {
            var linq = from ice in db.IceHockeySchedules
                       where ice.CtrlStates != 0 && ice.GameDate.CompareTo(gamedate) == 0
                       join iceall in db.IceHockeyAlliance on ice.AllianceID equals iceall.AllianceID
                       join icet1 in db.IceHockeyTeam on ice.TeamAID equals icet1.TeamID
                       join icet2 in db.IceHockeyTeam on ice.TeamBID equals icet2.TeamID
                       orderby ice.GameTime
                       select new IceHockey
                       {
                           Alliance = iceall.AllianceName,
                           CtrlAdmin = ice.CtrlAdmin,
                           TeamA = icet1.ShowName,
                           TeamB = icet2.ShowName,
                           GID = ice.GID,
                           GameDate = ice.GameDate,
                           GameTime = ice.GameTime,
                           GameStates = ice.GameStates,
                           GameType = ice.GameType
                       };
            if (gametype != "AAAA")
            {
                linq = linq.Where(m => m.GameType.CompareTo(gametype) == 0);
            }
            return linq.ToList();
        }

        /// <summary>
        /// 美足
        /// </summary>
        /// <param name="gamedata"></param>
        /// <param name="gametype"></param>
        /// <returns></returns>
        public List<AFB> GetAFB(DateTime gamedate, string gametype)
        {
            var linq = from afb in db.AFBSchedules
                       where afb.CtrlStates != 0 && afb.GameDate.CompareTo(gamedate) == 0
                       join afball in db.AFBAlliance on afb.AllianceID equals afball.AllianceID
                       join afbt1 in db.AFBTeam on afb.TeamAID equals afbt1.TeamID
                       join afbt2 in db.AFBTeam on afb.TeamBID equals afbt2.TeamID
                       orderby afb.GameTime
                       select new AFB
                       {
                           AllianceName = afball.AllianceName,
                           CtrlAdmin = afb.CtrlAdmin,
                           TeamAName = afbt1.ShowName,
                           TeamBName = afbt2.ShowName,
                           GID = afb.GID,
                           GameDate = afb.GameDate,
                           GameTime = afb.GameTime,
                           GameStates = afb.GameStates,
                           GameType = afb.GameType
                       };
            if (gametype != "AAAA")
            {
                linq = linq.Where(m => m.GameType.CompareTo(gametype) == 0);
            }
            return linq.ToList();
        }
    }
}
