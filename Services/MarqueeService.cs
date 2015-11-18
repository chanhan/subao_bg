
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
    public class MarqueeService : RepositoryBase<Marquee>, IMarqueeService
    {
        private readonly IModifyRecordService modifyRecord;
        public MarqueeService(IDatabaseFactory databaseFactory, IUser IUserService, IModifyRecordService IModifyRecordService)
            : base(databaseFactory, IUserService)
        {
            modifyRecord = IModifyRecordService;
        }

        public int UpDateMarquee(Marquee marquee)
        {
            marquee.ChgTime = DateTime.Now;
            string Identifier = MD5Password.GenerateId();
            ModifyRecord record = new ModifyRecord();
            string gameType = "Marquee";
            if (marquee.IsAdd)
            {
                this.Add(marquee);
                record = base.SaveModifyRecord(null, marquee, ActionItem.Add, CategoryItem.Message, gameType, Identifier);
            }
            else
            {
                Marquee oldMarquee = this.QueryById(marquee.ID);
                record = base.SaveModifyRecord(oldMarquee, marquee, ActionItem.Update, CategoryItem.Message, gameType, Identifier);

                oldMarquee.MessageCN = marquee.MessageCN;
                oldMarquee.MessageTW = marquee.MessageTW;
                oldMarquee.MessageUS = marquee.MessageUS;
                oldMarquee.AutoCloseYN = marquee.AutoCloseYN;
                oldMarquee.EffectiveTime = marquee.EffectiveTime;
                oldMarquee.EnableYN = marquee.EnableYN;
                oldMarquee.GameType = marquee.GameType;
                this.Update(oldMarquee);
            }
            modifyRecord.Add(record);
            return this.Commit();
        }
        public int autoClose(int id, string enableYN)
        {
            Marquee marquee = this.QueryById(id);
            marquee.EnableYN = enableYN;
            marquee.ChgTime = DateTime.Now;
            this.Update(marquee);
            return this.Commit();
        }

        public int DeleteMarquee(int id)
        {
            Marquee marquee = this.QueryById(id);
            string Identifier = MD5Password.GenerateId();
            ModifyRecord record = base.SaveModifyRecord(marquee, marquee, ActionItem.Delete, CategoryItem.Message, "Marquee", Identifier);
            this.Delete(id);
            modifyRecord.Add(record);
            return this.Commit();
        }
    }
}
