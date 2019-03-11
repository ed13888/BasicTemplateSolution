using System;
using System.Collections.Generic;
using System.IO;
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
            var guid = Request.Form["guid"];
            var msg = "上传失败";
            var imgUrl = "";
            if (string.IsNullOrEmpty(guid)) return Json(new { status = false, msg = msg, imgUrl = imgUrl });
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
                            string fileExt = "." + FileName.Split('.').Last();
                            string relativePath = $"/Upload/{guid}/";
                            string strPath = $"{Server.MapPath("/")}{relativePath}";
                            if (!Directory.Exists(strPath))
                            {
                                Directory.CreateDirectory(strPath);
                            }
                            var id = Guid.NewGuid().ToString("N");

                            imgUrl = relativePath + id + fileExt;//相对路径

                            var fileUrl = strPath + id + fileExt;//文件路径
                            PostedFile.SaveAs(fileUrl);
                        }
                    }
                }
                msg = "上传成功！";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Json(new { status = true, msg = msg, imgUrl = imgUrl });
        }
    }
}