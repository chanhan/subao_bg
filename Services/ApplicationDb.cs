using Models;
using System.Data.Entity;

//using Models.UserModels;
//using Services.SysServices;

namespace Services
{
    public class ApplicationDb : DbContext
    {
        //指定连接字符串
        public ApplicationDb()
            : base("Sport")
        {
        }
        public DbSet<BasketballSchedules> BasketballSchedules { get; set; }
        public DbSet<OSTeam> OSTeam { get; set; }
        public DbSet<BKOSAlliance> OSAlliance { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<FootballSchedules> SBSchedules { get; set; }
        public DbSet<AllianceNameList> AllianceNameList { get; set; }
        public DbSet<ModifyRecord> ModifyRecord { get; set; }
        public DbSet<OperationalRecord> OperationalRecord { get; set; }
        public DbSet<ScoreModifyRecord> ScoreModifyRecord { get; set; }
        public DbSet<NameControl> NameControl { get; set; }
        public DbSet<TennisSchedules> TennisSchedules { get; set; }
        public DbSet<TennisAlliance> TNAlliance { get; set; }
        public DbSet<BasketballAlliance> BasketballAlliance { get; set; }
        public DbSet<SourceType> SourceType { get; set; }
        public DbSet<SourceSettings> SourceSetting { get; set; }
        public DbSet<BasketballTeam> BasketballTeam { get; set; }
        public DbSet<BaseballSchedules> BaseballSchedules { get; set; }
        public DbSet<BaseballAlliance> BaseballAlliance { get; set; }
        public DbSet<BaseballTeam> BaseballTeam { get; set; }
        public DbSet<AFBSchedules> AFBSchedules { get; set; }
        public DbSet<AFBAlliance> AFBAlliance { get; set; }
        public DbSet<AFBTeam> AFBTeam { get; set; }
        public DbSet<IceHockeyAlliance> IceHockeyAlliance { get; set; }
        public DbSet<IceHockeyTeam> IceHockeyTeam { get; set; }
        public DbSet<IceHockeySchedules> IceHockeySchedules { get; set; }        
        public DbSet<Marquee> Marquee { get; set; }
        public DbSet<SetTypeVal> SetTypeVal { get; set; }
        public DbSet<ScrollingText> ScrollingText { get; set; }
        public DbSet<IPAccess> IPAccess { get; set; }
        public DbSet<FavoriteInfo> FavoriteInfo { get; set; }
        public virtual int Commit()
        {
            return base.SaveChanges();
        }
    }
}