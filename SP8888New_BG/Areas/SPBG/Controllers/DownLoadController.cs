using IServices;
using Models.ViewModel;
using SP8888New_BG.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SP8888New_BG.Areas.SPBG.Controllers
{
    public class DownLoadController : BaseController
    {
        //
        // GET: /SPBG/DownLoad/
        public DownLoadController(IEmployeeService employeeService)
            : base(employeeService)
        {
        }
        public ActionResult Index(string fileName)
        {
            string filePath = string.Format("{0}{1}", Server.MapPath("~/rar/"), fileName);
            if (System.IO.File.Exists(filePath))
            return File(filePath, "application/save-as", fileName);
            ViewBag.fileName = fileName;
            ViewBag.navigation = new Navigation
            {
                Level = new List<string> { "檔案下載"},
                HaveButton = false
            };
            return View();
        }

        //public FilePathResult Download(int id)
        //{
        //    var fileinfo = db.FileStores.Find(id);
        //    return File(fileinfo.FileUrl, fileinfo.MimeType, fileinfo.FileName);
        //}
    }
}
