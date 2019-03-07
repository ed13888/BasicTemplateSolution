using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebCode.Controllers
{
    [RoutePrefix("Template")]
    public class TemplateController : Controller
    {
        // GET: Template
        public ActionResult Index()
        {
            return View();
        }

        [Route("Design/{id:int}.html")]
        public ActionResult Design(int id)
        {
            return View();
        }
    }
}