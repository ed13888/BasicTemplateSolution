using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBService.Util;

namespace WebCode.Controllers
{
    [RoutePrefix("Template")]
    public class TemplateController : BaseController
    {
        private static IDictionary<int, string> dic = new Dictionary<int, string>
        {
            { 0,"零"},
            { 1,"一"},
            { 2,"二"},
            { 3,"三"},
            { 4,"四"},
            { 5,"五"},
            { 6,"六"},
            { 7,"七"},
            { 8,"八"},
            { 9,"九"},
            { 10,"十"}
        };

        // GET: Template
        public ActionResult Index()
        {
            return View();
        }

        [Route("Design/{id:int?}.html")]
        public ActionResult Design(int id = 0)
        {
            ViewBag.Count = 4;
            ViewBag.Dic = dic;
            return View();
        }

        [HttpPost]
        public ActionResult Create()
        {
            return View();
        }
    }
}