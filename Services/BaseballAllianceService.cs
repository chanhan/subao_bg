using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using Services.Infrastructure;
using IServices;
using System.Text.RegularExpressions;

namespace Services
{
    public class BaseballAllianceService : RepositoryBase<BaseballAlliance>, IBaseballAllianceService
    {
        private readonly IModifyRecordService _mrs;
        public BaseballAllianceService(IDatabaseFactory databaseFactory, IUser user, IModifyRecordService mrs)
            : base(databaseFactory, user)
        {
            _mrs = mrs;
        }

        public List<BaseballAlliance> getAllianceList(string gameType)
        {
            List<BaseballAlliance> ba = QueryByCondition(p => p.GameType == gameType && !p.IsDeleted).ToList();
            int t = 0;
            string[] tmp, tmpName = new string[] { "", "" };
            foreach (BaseballAlliance item in ba)
            {
                tmpName[0] = tmpName[1] = "";
                tmp = (string.IsNullOrWhiteSpace(item.LeverOther) ? "" : item.LeverOther).Trim().Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < tmp.Length; i++)
                {
                    if (!int.TryParse(tmp[i], out t) && t == 0)
                    {
                        break;
                    }
                    tmpName[i] = ba.Where(p => p.AllianceID == t).ToList()[0].AllianceName;
                }
                item.LeverOther = string.Join("->", tmpName);
            }
            return ba;
        }


        public int CreateAlliance(BaseballAlliance ba)
        {
            int c = checkedModelAlliance(ba, false);
            if (c < 1)
            {
                return c;
            }
            ModifyRecord modelModifyRecord = base.SaveModifyRecord(null, ba, Common.ActionItem.Add, Common.CategoryItem.Alliance, ba.GameType, Common.MD5Password.GenerateId());
            Add(ba);
            _mrs.Add(modelModifyRecord);
            return Commit();
        }



        public int EditAlliance(BaseballAlliance ba)
        {
            int c = checkedModelAlliance(ba, true);
            if (c < 1)
            {
                return c;
            }
            BaseballAlliance oldModel = QueryById(ba.AllianceID);
            ModifyRecord modelModifyRecord = base.SaveModifyRecord(oldModel, ba, Common.ActionItem.Update, Common.CategoryItem.Alliance, ba.GameType, Common.MD5Password.GenerateId());
            oldModel.AllianceName = ba.AllianceName;
            oldModel.AllianceUrl = ba.AllianceUrl;
            oldModel.LeverOther = ba.LeverOther;

            Update(oldModel);
            _mrs.Add(modelModifyRecord);
            return Commit();
        }

        /// <summary>
        /// 检查合法性
        /// </summary>
        private int checkedModelAlliance(BaseballAlliance ba, bool isEdit)
        {
            if (ba == null)
            {
                return 0;
            }
            //檢查名稱 如果是修改不檢查自己
            if (QueryByCondition(p => p.AllianceName == ba.AllianceName && p.GameType == ba.GameType && !p.IsDeleted && (isEdit ? ba.AllianceID != p.AllianceID : true)).Count() > 0)
            {
                return -1;
            }
            //检查URL
            return CheckURL(ba.AllianceUrl); ;
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


        /// <summary>
        /// 根据id获取一条信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetDataById(int id)
        {
            string name = "";
            BaseballAlliance baseball = base.QueryByCondition(m => m.AllianceID == id).SingleOrDefault();
            if (baseball != null)
                name = baseball.AllianceName;
            return name;
        }
    }
}
