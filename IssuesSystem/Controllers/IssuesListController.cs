using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using IssuesSystem.Models;

namespace IssuesSystem.Controllers
{
    public class IssueListToSend
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        public string DateCreated { get; set; }
    }
    

public class IssuesListController : Controller
    {
        

        // GET: IssuesList
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CollectIssuesList()
        {
            Dictionary<int, string> States =
                new Dictionary<int, string>
                {
                    {1, "Открыта"},
                    {2, "Решена"},
                    {3, "Возврат"},
                    {4, "Закрыта"}


                };
            using (IssuesSystemDBEntities issuesDB = new IssuesSystemDBEntities())
            {
                List<Issue> issuesList = issuesDB.Issues.ToList<Issue>();
                
                List< IssueListToSend> issueListToSends  = new List<IssueListToSend>();
                foreach (var issue in issuesList)
                {
                    string s = "";
                    if (issue.State!=null)
                    {
                        States.TryGetValue((int)issue.State, out s);
                    }
                    
                    

                    issueListToSends.Add(
                        new IssueListToSend()
                        {
                            Id = issue.Id,
                            //State = issue.State,
                            State = s,
                            Description = issue.Description,
                            DateCreated = issue.DateCreated.ToString()
                        }
                        );
                }
                return Json(new { data = issueListToSends }, JsonRequestBehavior.AllowGet);
        }
        }

        [HttpGet]
        public ActionResult AddOrEdit(int Id=0)
        {
            return View(new Issue());
        }

        [HttpPost]
        public ActionResult AddOrEdit(Issue issue)
        {
            using (IssuesSystemDBEntities IssuesDB = new IssuesSystemDBEntities())
            {
                IssuesDB.Issues.Add(issue);
                IssuesDB.SaveChanges();
                return Json(new {success = true, message = "Saved Issue Successfully"}, JsonRequestBehavior.AllowGet);
            }
            return View();
        }
    }
}