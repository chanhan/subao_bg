using System.Web.Optimization;

namespace SP8888New_BG
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            //            "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            //公共js
            bundles.Add(new ScriptBundle("~/bundles/public").Include(
                  "~/Scripts/public.js"
                  ));
            //jquery.form.min.js
             bundles.Add(new ScriptBundle("~/bundles/jqueryform").Include(
                  "~/Scripts/jquery.form.js"
                  ));
            //datetimepicker
            bundles.Add(new ScriptBundle("~/bundles/datetimepicker").Include(
                 "~/Scripts/datetimepicker/jquery.datetimepicker.js"
                 ));

            //inputmask
            bundles.Add(new ScriptBundle("~/bundles/inputmask").Include(
                     "~/Scripts/jquery.inputmask/jquery.inputmask.js",
                     "~/Scripts/jquery.inputmask/jquery.inputmask.numeric.extensions.js",
                     "~/Scripts/jquery.inputmask/jquery.inputmask.date.extensions.js"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/ScoreEdit").Include("~/Scripts/ScoreEdit.js"));

            //// 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            //// 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //公共css
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Css/main.css"));
            //表格的css
            bundles.Add(new StyleBundle("~/Content/list_table").Include("~/Css/list_table.css"));

            //datetimepicker css
            bundles.Add(new StyleBundle("~/Content/datetimepicker").Include("~/Css/datetimepicker/jquery.datetimepicker.css"));        
            
            //美棒 篮球 美足 冰球  公共css
            bundles.Add(new StyleBundle("~/Content/updatescore").Include("~/Css/updatescore.css"));//修改分数
            bundles.Add(new StyleBundle("~/Content/scoremodifyrecord").Include("~/Css/scoremodifyrecord.css"));//分数修改记录
            bundles.Add(new StyleBundle("~/Content/schedule").Include("~/Css/schedule.css"));//赛事管理
            bundles.Add(new StyleBundle("~/Content/editschedule").Include("~/Css/editschedule.css"));//新增修改赛事
            bundles.Add(new StyleBundle("~/Content/alliance").Include("~/Css/alliance.css"));//联盟管理
            bundles.Add(new StyleBundle("~/Content/updatealliance").Include("~/Css/updatealliance.css"));//新增修改联盟
            bundles.Add(new StyleBundle("~/Content/team").Include("~/Css/team.css"));//队伍管理
            bundles.Add(new StyleBundle("~/Content/updateteam").Include("~/Css/updateteam.css"));//新增修改队伍
     
            //奥逊篮球
            bundles.Add(new StyleBundle("~/Content/bkosalliance").Include("~/Css/bkos/bkosalliance.css"));
            bundles.Add(new StyleBundle("~/Content/bkosteam").Include("~/Css/bkos/bkosteam.css"));
            bundles.Add(new StyleBundle("~/Content/bkosscore").Include("~/Css/bkos/score.css"));
            bundles.Add(new StyleBundle("~/Content/bkos").Include("~/Css/bkos/bkos.css"));
            bundles.Add(new StyleBundle("~/Content/bkosmain").Include("~/Css/bkos/bkosmain.css"));

            //足球
            bundles.Add(new StyleBundle("~/Content/sb").Include("~/Css/football/sb.css"));
            bundles.Add(new StyleBundle("~/Content/score").Include("~/Css/football/score.css"));

            //网球
            bundles.Add(new StyleBundle("~/Content/tennis").Include("~/Css/tennis/tennis.css"));
            bundles.Add(new StyleBundle("~/Content/tennismain").Include("~/Css/tennis/tennismain.css"));
            bundles.Add(new StyleBundle("~/Content/tennisalliance").Include("~/Css/tennis/tennisalliance.css"));
            
            //棒球
            bundles.Add(new StyleBundle("~/Content/baseballscore").Include("~/Css/baseball/score.css"));
            bundles.Add(new StyleBundle("~/Content/baseballschedule").Include("~/Css/baseball/schedule.css"));
            bundles.Add(new StyleBundle("~/Content/sp8888ctrl").Include("~/Css/baseball/sp8888ctrl.css"));


            //冰球
            bundles.Add(new StyleBundle("~/Content/icehockeyschedule").Include("~/Css/IceHockey/schedule.css"));

            //系统设定
            bundles.Add(new StyleBundle("~/Content/marquee").Include("~/Css/systemset/marquee.css"));
            bundles.Add(new StyleBundle("~/Content/namecontrol").Include("~/Css/systemset/namecontrol.css"));
            bundles.Add(new StyleBundle("~/Content/commercial").Include("~/Css/systemset/commercial.css"));
            bundles.Add(new StyleBundle("~/Content/IPAccess").Include("~/Css/systemset/IPAccess.css"));
            bundles.Add(new StyleBundle("~/Content/scrollingtext").Include("~/Css/systemset/scrollingtext.css"));
            bundles.Add(new StyleBundle("~/Content/SetTypeVal").Include("~/Css/systemset/SetTypeVal.css"));
            
            //员工管理
            bundles.Add(new StyleBundle("~/Content/employee").Include("~/Css/employee/employee.css"));

            //操盘列表
            bundles.Add(new StyleBundle("~/Content/actionlist").Include("~/Css/actionlist/actionlist.css"));
            
            //操作记录
            bundles.Add(new StyleBundle("~/Content/loglist").Include("~/Css/QT/loglist.css"));

            //bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
            //            "~/Content/themes/base/jquery.ui.core.css",
            //            "~/Content/themes/base/jquery.ui.resizable.css",
            //            "~/Content/themes/base/jquery.ui.selectable.css",
            //            "~/Content/themes/base/jquery.ui.accordion.css",
            //            "~/Content/themes/base/jquery.ui.autocomplete.css",
            //            "~/Content/themes/base/jquery.ui.button.css",
            //            "~/Content/themes/base/jquery.ui.dialog.css",
            //            "~/Content/themes/base/jquery.ui.slider.css",
            //            "~/Content/themes/base/jquery.ui.tabs.css",
            //            "~/Content/themes/base/jquery.ui.datepicker.css",
            //            "~/Content/themes/base/jquery.ui.progressbar.css",
            //            "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}