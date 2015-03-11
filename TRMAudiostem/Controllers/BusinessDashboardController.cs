using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TRMAudiostem.Controllers
{
    public class BusinessDashboardController : Controller
    {
        //
        // GET: /BusinessDashboard/

        public ActionResult Index()
        {
            return PartialView();
        }

        public PartialViewResult _Artists()
        {
            return PartialView(new List<Artist>());
        }

        public PartialViewResult _Activity()
        {
            return PartialView();
        }
    }
}
