using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBService.Util;

namespace WebCode.Controllers
{
    public class FileUploadController : BaseController
    {
        // GET: File
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upload()
        {
            var msg = "";
            try
            {
                var files = Request.Files;
                if (files != null && files.Count > 0)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        var PostedFile = files[i];
                        if (PostedFile.ContentLength > 0)
                        {
                            string FileName = PostedFile.FileName;//文件名自行处理

                            string strPath = Server.MapPath("/") + "Upload\\";
                            PostedFile.SaveAs(strPath + FileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Json(new { msg = msg });
        }
    }
}