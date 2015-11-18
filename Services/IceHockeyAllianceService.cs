using Common;
using IServices;
using Models;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Services
{
    public class IceHockeyAllianceService : RepositoryBase<IceHockeyAlliance>, IIceHockeyAllianceService
    {
        /// <summary>
        /// 修改记录
        /// </summary>
        private readonly IModifyRecordService _mrs;
        public IceHockeyAllianceService(IDatabaseFactory databaseFactory, IUser user, IModifyRecordService mrs)
            : base(databaseFactory, user)
        {
            _mrs = mrs;
        }

        public List<IceHockeyAlliance> getAllianceList(string gameType)
        {
            List<IceHockeyAlliance> ihList = QueryByCondition(p => p.GameType == gameType && p.Display).ToList();
            int t = 0;
            string[] tmp, tmpName = new string[] { "", "" };
            foreach (IceHockeyAlliance item in ihList)
            {
                tmpName[0] = tmpName[1] = "";
                tmp = (string.IsNullOrWhiteSpace(item.LeverOther) ? "" : item.LeverOther).Trim().Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < tmp.Length; i++)
                {
                    if (!int.TryParse(tmp[i], out t) && t == 0)
                    {
                        break;
                    }
                    tmpName[i] = ihList.Where(p => p.AllianceID == t).ToList()[0].AllianceName;
                }
                item.LeverOther = string.Join("->", tmpName);
            }
            return ihList;
        }

        public List<IceHockeyAlliance> getAllianceListByIHBF(string gameType, string keyWord, int pageIndex, int pageSize, out int count)
        {
            bool isNull = string.IsNullOrWhiteSpace(keyWord);
            return QueryByConditionForPage(p => (isNull ? true : (p.AllianceName.Contains(keyWord) || p.ShowName.Contains(keyWord))) && p.GameType == gameType, (o => o.AllianceSortID != null ? o.AllianceSortID.Value : Int32.MaxValue), pageIndex, pageSize, out  count).ToList();
        }

        public int CreateAlliance(IceHockeyAlliance ia)
        {
            int c = checkedModelAlliance(ia, false);
            if (c < 1)
            {
                return c;
            }
            //默认值
            ia.CreateDate = DateTime.Now;
            ia.Display = true;
            ModifyRecord modelModifyRecord = base.SaveModifyRecord(null, ia, Common.ActionItem.Add, Common.CategoryItem.Alliance, ia.GameType, Common.MD5Password.GenerateId());
            Add(ia);
            _mrs.Add(modelModifyRecord);
            return Commit();
        }

        public int EditAlliance(IceHockeyAlliance ia)
        {
            int c = checkedModelAlliance(ia, true);
            if (c < 1)
            {
                return c;
            }
            IceHockeyAlliance oldModel = QueryById(ia.AllianceID);
            ModifyRecord modelModifyRecord = base.SaveModifyRecord(oldModel, ia, Common.ActionItem.Update, Common.CategoryItem.Alliance, ia.GameType, Common.MD5Password.GenerateId());
            oldModel.AllianceName = ia.AllianceName;
            oldModel.LeverOther = ia.LeverOther;
            oldModel.AllianceSortID = ia.AllianceSortID;
            oldModel.ShowName = ia.ShowName;
            oldModel.AllianceUrl = ia.AllianceUrl;
            Update(oldModel);
            _mrs.Add(modelModifyRecord);
            return Commit();
        }

        /// <summary>
        /// 检查合法性
        /// </summary>
        private int checkedModelAlliance(IceHockeyAlliance ia, bool isEdit)
        {
            if (ia == null)
            {
                return 0;
            }
            if (ia.GameType == "IHBF" && QueryByCondition(p => p.AllianceSortID == ia.AllianceSortID && p.AllianceID != ia.AllianceID).Any())
            { 
                return -3;
            }
            //檢查名稱 如果是修改不檢查自己
            if (QueryByCondition(p => (ia.GameType == "IHBF" ? p.ShowName == ia.ShowName : p.AllianceName == ia.AllianceName) && p.GameType == ia.GameType && p.Display && (isEdit ? ia.AllianceID != p.AllianceID : true)).Count() > 0)
            {
                return -1;
            }
            return CheckURL(ia.AllianceUrl);
        }

        private int CheckURL(string strUrl)
        {
            if (string.IsNullOrWhiteSpace(strUrl))
            {
                return 1;
            }
            String check = @"((http|https|ftp)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?";

            Regex regex = new Regex(check);
            Match match = regex.Match(strUrl);
            if (match.Success)
            {
                return 1;
            }
            else
            {
                return -2;
            }
        }



        public int SaveDisplaySetting(List<IceHockeyAlliance> models)
        {
            IEnumerable<int> gids = models.Select(p => p.AllianceID);
            List<IceHockeyAlliance> list = QueryByCondition(p => gids.Contains(p.AllianceID)).ToList();
            IceHockeyAlliance alliance;
            IceHockeyAlliance temp;
            string MD5ID = Common.MD5Password.GenerateId();
            foreach (var item in list)
            {
                alliance = null;
                alliance = models.SingleOrDefault(p => p.AllianceID == item.AllianceID);
                if (alliance != null && item.Display != alliance.Display)
                {
                    temp = copy(item);
                    temp.Display = alliance.Display;
                    ModifyRecord modelModifyRecord = base.SaveModifyRecord(item, temp, Common.ActionItem.Update, Common.CategoryItem.Alliance, item.GameType, MD5ID);
                    _mrs.Add(modelModifyRecord);
                    item.Display = alliance.Display;
                }
            }
            return this.Commit();
        }

        public string  GetDataById(int id)
        {
            string name = "";
            IceHockeyAlliance  ice = base.QueryByCondition(m => m.AllianceID == id).SingleOrDefault();
            if(ice!=null)
                name = ice.ShowName;
            return name;
        }

        private IceHockeyAlliance copy(IceHockeyAlliance ia)
        {
            IceHockeyAlliance newIa = new IceHockeyAlliance()
            {
                AllianceID = ia.AllianceID,
                AllianceName = ia.AllianceName,
                AllianceSortID = ia.AllianceSortID,
                AllianceUrl = ia.AllianceUrl,
                CreateDate = ia.CreateDate,
                Display = ia.Display,
                GameType = ia.GameType,
                Lever = ia.Lever,
                LeverOther = ia.LeverOther,
                ShowName = ia.ShowName
            };

            return newIa;
        }
    }
}
