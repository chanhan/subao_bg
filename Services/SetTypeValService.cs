using Common;
using IServices;
using Models;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
    public class SetTypeValService : RepositoryBase<SetTypeVal>, ISetTypeValService
    {
        private readonly IModifyRecordService modifyRecord;
        public SetTypeValService(IDatabaseFactory databaseFactory, IUser IUserService, IModifyRecordService IModifyRecordService)
            : base(databaseFactory, IUserService)
        {
            modifyRecord = IModifyRecordService;
        }

        public string GetDataSetTypeVal()
        {
            var linq = base.QueryByCondition(s => s.Type == "DefaultPage").OrderByDescending(s => s.Language).Select(s => s.Val);
            string val = string.Join(",", linq);
            return val;
        }

        public int EditSetTypeVal(string val1, string val2)
        {
            int count = 0;
            try
            {
                Edit(val1, "zh-tw");
                Edit(val2, "zh-cn");
                count = base.Commit();
            }
            catch
            {
                return 0;
            }
            return count;
        }

        /// <summary>
        /// 修改或添加
        /// </summary>
        /// <param name="val"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public void Edit(string val, string language)
        {
            List<SetTypeVal> settypeval_cn = base.QueryByCondition(s => s.Type == "DefaultPage" && s.Language == language).ToList();
            if (settypeval_cn != null)
            {
                foreach (var stv in settypeval_cn)
                {
                    SetTypeVal olddata = stv;
                    ModifyRecord record = base.SaveModifyRecord(olddata, new SetTypeVal() { Type = "DefaultPage",Val=val,Language=language }, ActionItem.Update, CategoryItem.HomePage, "SetTypeVal", MD5Password.GenerateId());
                    stv.Type = "DefaultPage";
                    stv.Val = val;
                    stv.Language = language;
                    base.Update(stv);
                    modifyRecord.Add(record);
                }
            }
            else
            {
                SetTypeVal stv = new SetTypeVal { Type = "DefaultPage", Val = val, Language = language };
                ModifyRecord record = base.SaveModifyRecord(null, stv, ActionItem.Add, CategoryItem.HomePage, "SetTypeVal", MD5Password.GenerateId());
                base.Add(stv);
                modifyRecord.Add(record);
            }
        }
    }
}
