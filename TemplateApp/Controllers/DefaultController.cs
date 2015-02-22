using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TemplateApp.Controllers
{
    public class DefaultController : Controller
    {
        public ActionResult Weather()
        {
            SaveRequest();
            return View();        
        }

        private void SaveRequest()
        {
        }

        public ActionResult Walkscore()
        {
            SaveRequest();
            return View();
        }

        public ActionResult Family()
        {
            SaveRequest();
            return View();
        }

        public ActionResult Entrepreneur()
        {
            SaveRequest();
            return View();
        }
        public ActionResult Book()
        {
            SaveRequest();
            return View();
        }

        public ActionResult Often()
        {
            SaveRequest();
            return View();
        }

        public ActionResult Activities()
        {
            SaveRequest();
            return View();
        }

        public ActionResult Opportunities()
        {
            SaveRequest();
            return View();
        }


        public ActionResult Student()
        {
            SaveRequest();
            return View();
        }

        public ActionResult Industry()
        {
            SaveRequest();
            return View();
        }
        // GET: Default
        public ActionResult Index()
        {
            SaveRequest();
            return View();
        }

        public ActionResult Result()
        {
            SaveRequest();
            return View();
        }
        public ActionResult Results()
        {
            SaveRequest();
            return View();
        }
    }
}