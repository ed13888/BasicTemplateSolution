﻿using Common.Entity.Business;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBService.Util;
using WEBService.WebBusiness;

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
            var list = BusinessBll.GetList(this);
            return View(list);
        }

        [Route("Design/{id:int?}.html")]
        public ActionResult Design(int id = 0)
        {
            var m = BusinessBll.GetById(this, id);
            ViewBag.Count = m.FSentenceCount;
            ViewBag.Dic = dic;
            return View();
        }

        [HttpPost]
        public ActionResult Create()
        {
            string json = "";
            for (int i = 0; i < Request.Form.Keys.Count; i++)
            {
                if (Request.Form.Keys[i].ToString().Substring(0, 1) != "_")
                {
                    json += Request.Form.Keys[i].ToString() + " : \"" + Request.Form[i].ToString() + "\",";
                }
            }
            json = $"{{{json}}}";

            var m = JsonConvert.DeserializeObject<CustomerTemplateInfoEntity>(json);
            //var val = BusinessBll.CreateTemplate(this, m);

            return View(m);
        }
    }
}