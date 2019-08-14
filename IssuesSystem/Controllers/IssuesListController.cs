using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IssuesSystem.Controllers
{
    public class IssuesListController : Controller
    {
        // GET: IssuesList
        public ActionResult Index()
        {
            return View();
        }
    }
}