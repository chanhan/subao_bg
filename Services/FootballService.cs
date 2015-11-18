using Common;
using IServices;
using Models;
using Models.ViewModel;
using Services.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class FootballService : RepositoryBase<FootballSchedules>, IFootballService
    {
        /// <summary>
        /// 分数修改记录
        /// </summary>
        private readonly IScoreModifyRecordService _smrs;

        public FootballService(IDatabaseFactory databaseFactory, IScoreModifyRecordService IScoreModifyRecordService, IUser user)
            : base(databaseFactory, user)
        {
            _smrs = IScoreModifyRecordService;
        }

        public List<FootballSchedules> GetFoolballSchedules(DateTime date)
        {
            var linq = (from sb in db.SBSchedules.Where(p => p.GameDate == date.Date && p.GameType != "message" && p.C != -1)
                        join allianceName in db.AllianceNameList.Where(p => p.LanguageCode == "zh-tw" && p.AllianceType == "SB")
                        on sb.AL equals allianceName.SimpleName into alliance
                        from sbAlliance in alliance.DefaultIfEmpty()
                        orderby sb.Orderby
                        select new
                        {
                            ID = sb.ID,
                            WebID = sb.WebID,
                            AL = string.IsNullOrEmpty(sbAlliance.FullName) ? sb.AL : sbAlliance.FullName,
                            KO = sb.KO,
                            UP = sb.UP,
                            NA = sb.NA,
                            NB = sb.NB,
                            CtrlStates = sb.CtrlStates,
                            Orderby = sb.Orderby,
                            IsScoreModifyRecord = (sb.CtrlAdmin != null)
                        }).ToList().Select(sb => new FootballSchedules
                        {
                            ID = sb.ID,
                            WebID = sb.WebID,
                            AL = sb.AL,
                            KO = sb.KO,
                            UP = sb.UP,
                            NA = sb.NA,
                            NB = sb.NB,
                            CtrlStates = sb.CtrlStates,
                            Orderby = sb.Orderby,
                            IsScoreModifyRecord = sb.IsScoreModifyRecord
                        }); ;
            return linq.ToList();
        }

        public List<FootballSchedules> GetFoolballSchedulesByType(DateTime date, string al)
        {
            var linq = (from sb in db.SBSchedules
                        where sb.GameDate == date.Date && sb.AL == al && sb.GameType != "message" && sb.C != -1
                        orderby sb.Orderby
                        select sb).ToList().Select(sb => new FootballSchedules
                        {
                            ID = sb.ID,
                            WebID = sb.WebID,
                            AL = sb.AL,
                            KO = sb.KO,
                            UP = sb.UP,
                            NA = sb.NA,
                            NB = sb.NB,
                            CtrlStates = sb.CtrlStates,
                            Orderby = sb.Orderby
                        });
            return linq.ToList();
        }

        public bool ScoreSave(FootballSchedules model, int modifyItem)
        {
            FootballSchedules sb = base.QueryById(model.ID);
            model.UP = model.UP == "" ? sb.UP : model.UP;
            model.CtrlStates = modifyItem;
            ScoreModifyRecords(sb, model);

            switch (modifyItem)
            {
                case 1:
                    sb.KO = model.KO;
                    sb.UP = model.UP;
                    sb.OA = model.OA;
                    sb.OB = model.OB;
                    sb.RA = model.RA;
                    sb.RB = model.RB;
                    sb.NAR = model.NAR;
                    sb.NBR = model.NBR;
                    sb.CA = model.CA;
                    sb.CB = model.CB;
                    break;
                case 21:
                    sb.OA = model.OA;
                    sb.OB = model.OB;
                    sb.RA = model.RA;
                    sb.RB = model.RB;
                    sb.NAR = model.NAR;
                    sb.NBR = model.NBR;
                    sb.CA = model.CA;
                    sb.CB = model.CB;
                    break;
                case 31:
                    sb.UP = model.UP;
                    sb.KO = model.KO;
                    break;
                default:
                    return false;
            }
            sb.C++;
            sb.CtrlStates = modifyItem;
            sb.CtrlAdmin = userService.UserName;

            base.Update(sb);

            return Convert.ToBoolean(base.Commit());
        }

        public void ScoreModifyRecords(FootballSchedules oldModel, FootballSchedules newModel)
        {
            ScoreModifyRecord modelSmr = new ScoreModifyRecord()
            {
                ScheduleID = oldModel.ID,
                GameType = oldModel.GameType,
                //RA+OA+CA+NAR  半场+全场+黄牌+红 
                RunsAOld = oldModel.RA + "," + oldModel.OA + "," + oldModel.CA + "," + oldModel.NAR,
                RunsBOld = oldModel.RB + "," + oldModel.OB + "," + oldModel.CB + "," + oldModel.NBR,
                StatusTextOld = oldModel.UP,
                RunsANew = newModel.RA + "," + newModel.OA + "," + newModel.CA + "," + newModel.NAR,
                RunsBNew = newModel.RB + "," + newModel.OB + "," + newModel.CB + "," + newModel.NBR,
                StatusTextNew = newModel.UP,
                ModifyTime = DateTime.Now,
                ModifyUser = userService.UserName,
                IpAddr = userService.UserIP,
                ModifyItem = newModel.CtrlStates,
                GameDateOld = oldModel.KO,
                GameDateNew = newModel.KO
            };
            _smrs.Add(modelSmr);
        }

        public bool ScoreDelete(int id)
        {
            FootballSchedules sb = base.QueryById(id);
            sb.CtrlStates = 2;
            sb.C++;
            this.Update(sb);

            ScoreModifyRecord modelSmr = new ScoreModifyRecord()
            {
                ScheduleID = sb.ID,
                GameType = sb.GameType,
                ModifyTime = DateTime.Now,
                ModifyUser = userService.UserName,
                IpAddr = userService.UserIP,
                ModifyItem = 5
            };
            _smrs.Add(modelSmr);
            return Convert.ToBoolean(base.Commit());
        }
    }
}