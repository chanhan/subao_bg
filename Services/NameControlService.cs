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
    public class NameControlService : RepositoryBase<NameControl>, INameControlService
    {
        private readonly IModifyRecordService modifyRecord;
        public NameControlService(IDatabaseFactory databaseFactory, IUser IUserService, IModifyRecordService IModifyRecordService)
            : base(databaseFactory, IUserService)
        {
            modifyRecord = IModifyRecordService;
        }
        public List<NameControl> GetNameControls(NameControl nameControl)
        {
            if (nameControl.Category == "0") nameControl.GameType = nameControl.GTLangx;
            else nameControl.GameType = "Name";
            bool b = IsTennisTeamName(nameControl);
            var linq = this.QueryByCondition(p => p.Category == nameControl.Category && p.Langx == nameControl.Langx && p.GameType == nameControl.GameType && p.GTLangx == nameControl.GTLangx && p.AppType == nameControl.AppType && (b ? !(p.SourceText.Contains("/") || p.SourceText.Contains("(")) : true)).OrderBy(p => p.Indexs).OrderByDescending(p => p.SourceText);
            return linq.ToList();
        }
        private bool IsTennisTeamName(NameControl nameControl)
        {
            return nameControl.Langx == "en" && nameControl.Category == "2" && nameControl.GTLangx == "TN" && nameControl.AppType == "First";
        }
        public int EditNameControl(NameControl nameControl)
        {
            nameControl.ChangeDate = DateTime.Now;
            if (nameControl.Category == "0") nameControl.GameType = nameControl.GTLangx;
            else nameControl.GameType = "Name";
            string gameType = "NameControl";
            string Identifier = MD5Password.GenerateId();
            ModifyRecord record;
            if (nameControl.IsAdd)
            {
                this.Add(nameControl);
                this.Commit();
                record = base.SaveModifyRecord(null, nameControl, ActionItem.Add, CategoryItem.NameList, gameType, Identifier);
                modifyRecord.Add(record);
            }
            else
            {
                NameControl oldData = this.QueryById(nameControl.ID);
                record = base.SaveModifyRecord(oldData, nameControl, ActionItem.Update, CategoryItem.NameList, gameType, Identifier);
                modifyRecord.Add(record);
                oldData.AppType = nameControl.AppType;
                oldData.Category = nameControl.Category;
                oldData.ChangeText = nameControl.ChangeText;
                oldData.GameType = nameControl.GameType;
                oldData.GTLangx = nameControl.GTLangx;
                oldData.Indexs = nameControl.Indexs;
                oldData.Langx = nameControl.Langx;
                oldData.SourceText = nameControl.SourceText;
                this.Update(oldData);
            }
            if (IsTennisTeamName(nameControl))
            {
                List<NameControl> name1 = this.QueryByCondition(p => p.SourceText.Contains("(") && p.SourceText.Contains(nameControl.SourceText) && p.GTLangx == "TN" && p.GameType == "Name").ToList();
                foreach (var item in name1)
                {
                    int index = item.SourceText.IndexOf('(');
                    item.ChangeText = string.Format("{0}{1}{2}", nameControl.ChangeText, " ", item.SourceText.Substring(index));
                }
                if (name1.Any())
                {
                    this.Update(name1);
                }
                List<NameControl> name2 = this.QueryByCondition(p => p.SourceText.Contains("/") && p.SourceText.Contains(nameControl.SourceText) && p.GTLangx == "TN" && p.GameType == "Name").ToList();
                foreach (var item in name2)
                {
                    int index1 = item.SourceText.IndexOf('/');
                    int index2 = item.SourceText.IndexOf(nameControl.SourceText);
                    int index3 = item.ChangeText.IndexOf('/');
                    if (index1 < index2)
                    {
                        item.ChangeText = string.Format("{0}{1}{2}", item.ChangeText.Substring(0, index3).Trim(), "/", nameControl.ChangeText);
                    }
                    else
                    {
                        item.ChangeText = string.Format("{0}{1}{2}", nameControl.ChangeText, "/", item.ChangeText.Substring(index3 + 1).Trim());
                    }
                }
                if (name2.Any())
                {
                    this.Update(name2);
                }
            }
            return this.Commit();
        }
        public int DeleteNameControl(int id)
        {
            NameControl control = this.QueryById(id);
            this.Delete(id);
            string gameType = "NameControl";
            string Identifier = MD5Password.GenerateId();
            ModifyRecord record = base.SaveModifyRecord(control, control, ActionItem.Delete, CategoryItem.NameList, gameType, Identifier);
            modifyRecord.Add(record);
            return this.Commit();
        }

        public string GetDataById(int id)
        {
            string name = "";
            NameControl namecontrol = base.QueryById(id);
            if(namecontrol!=null)
                name = namecontrol.ChangeText;
            return name;
        }
    }
}
