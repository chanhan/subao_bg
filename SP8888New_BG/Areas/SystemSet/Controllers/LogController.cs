using IServices;
using Models;
using SP8888New_BG.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SP8888New_BG.Areas.SystemSet.Controllers
{
    public class LogController : BaseController
    {
        //
        // GET: /SystemSet/Log/

        private IMarqueeService _IMarqueeService;
        private INameControlService _INameControlService;
        private IIPAccessService _IIPAccessService;
        private ISetTypeValService _ISetTypeValService;
        public LogController(IMarqueeService marqueeService, INameControlService nameControlService, IIPAccessService ipaccessservice,ISetTypeValService settypevalservice, IEmployeeService employeeService)
            : base(employeeService)
        {
            _IMarqueeService = marqueeService;
            _INameControlService = nameControlService;
            _IIPAccessService = ipaccessservice;
            _ISetTypeValService = settypevalservice;
        }

        public ActionResult Index()
        {
            return View();
        }
        //讯息管理
        public ActionResult Message(IEnumerable<ModifyRecord> records)
        {
            List<Marquee> oldMessage = new List<Marquee>();
            List<Marquee> newMessage = new List<Marquee>();
            List<ModifyRecord> list = records.ToList();
            list.ForEach(p =>
            {
                Marquee old = _IMarqueeService.JsonDeserialize(p.OldData);
                Marquee New = _IMarqueeService.JsonDeserialize(p.NewData);
                if (old != null)
                {
                    oldMessage.Add(old);
                }
                if (New != null)
                {
                    newMessage.Add(New);
                }
            });
            return View(Tuple.Create(oldMessage, newMessage, list[0].ActionStatus));
        }


        //名称对应表
        public ActionResult Name(IEnumerable<ModifyRecord> records)
        {
            List<NameControl> oldName = new List<NameControl>();
            List<NameControl> newName = new List<NameControl>();
            List<ModifyRecord> list = records.ToList();
            list.ForEach(p =>
            {
                NameControl old = _INameControlService.JsonDeserialize(p.OldData);
                NameControl New = _INameControlService.JsonDeserialize(p.NewData);
                if (old != null)
                {
                    oldName.Add(old);
                }
                if (New != null)
                {
                    newName.Add(New);
                }
            });
            return View(Tuple.Create(oldName, newName, list[0].ActionStatus));
        }


        /// <summary>
        /// ip地址
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public ActionResult IpAddress(IEnumerable<ModifyRecord> records)
        {
            List<IPAccess> oldIpAddress = new List<IPAccess>();
            List<IPAccess> newIpAddress = new List<IPAccess>();
            List<ModifyRecord> list = records.ToList();
            list.ForEach(p =>
            {
                IPAccess old = _IIPAccessService.JsonDeserialize(p.OldData);
                IPAccess New = _IIPAccessService.JsonDeserialize(p.NewData);
                if (old != null)
                {
                    oldIpAddress.Add(old);
                }
                if (New != null)
                {
                    newIpAddress.Add(New);
                }
            });
            return View(Tuple.Create(oldIpAddress, newIpAddress, list[0].ActionStatus));
        }
        public ActionResult SetTypeVal(IEnumerable<ModifyRecord> records)
        {
            List<SetTypeVal> oldSetTypeVal = new List<SetTypeVal>();
            List<SetTypeVal> newSetTypeVal = new List<SetTypeVal>();
            List<ModifyRecord> list = records.ToList();
            list.ForEach(p =>
            {
                SetTypeVal old = _ISetTypeValService.JsonDeserialize(p.OldData);
                SetTypeVal New = _ISetTypeValService.JsonDeserialize(p.NewData);
                if (old != null)
                {
                    oldSetTypeVal.Add(old);
                }
                if (New != null)
                {
                    newSetTypeVal.Add(New);
                }
            });
            return View(Tuple.Create(oldSetTypeVal, newSetTypeVal, list[0].ActionStatus));
        }
    }
}
