using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IssuesSystem.Models;

namespace IssuesSystem.Controllers
{
    public class IssuesListController : Controller
    {
        // GET: IssuesList
        public ActionResult Index()
        {
            
            using (IssuesSystemDBEntities issuesDB = new IssuesSystemDBEntities())
            {
                List<Issue> issuesList = issuesDB.Issues.ToList<Issue>();
                
            }
            return View();
        }
    }
}