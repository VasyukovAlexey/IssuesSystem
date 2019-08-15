﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
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
        

        
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult CollectIssuesList()
        {
                            //Перечень статусов заявок
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
                    //Формируем особый, уличный, перечень заявок для отображения в таблице, взяв за основу перечень из базы
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
                            State = s,
                            Description = issue.Description,
                            DateCreated = issue.DateCreated.ToString()
                        }
                        );
                }
                return Json(new { data = issueListToSends }, JsonRequestBehavior.AllowGet);
        }
        }

        [HttpGet]               //Отправляем новый пустой объект заявки
        public ActionResult AddIssue(int Id=0)
        {
            return View(new Issue());
        }

        [HttpPost]                          //Получаем заполненный объект заявки
        public ActionResult AddIssue(Issue issue)
        {
            using (IssuesSystemDBEntities IssuesDB = new IssuesSystemDBEntities())
            {
                            //Создаём новую заявку в базе
                issue.State = 1;
                issue.DateCreated=DateTime.Now;
                IssuesDB.Issues.Add(issue);
                IssuesDB.SaveChanges();
                                        
                                            //Создаём запись об изменении статуса заявки
                using (SqlCommand comm = new SqlCommand(string.Empty,new SqlConnection(IssuesDB.Database.Connection.ConnectionString)))
                {

                    comm.CommandText = ($"insert into IssueStateChanges (IdIssue,State,DateChanged) values ({issue.Id},{issue.State},@date_operation)");
                    comm.Parameters.Add(
                        "@date_operation", SqlDbType.DateTime, 80).Value = issue.DateCreated;
                    comm.Connection.Open();
                    comm.ExecuteNonQuery();
                }
            }

            
            
            return Json(new { success = true, message = "Saved Issue Successfully" }, JsonRequestBehavior.AllowGet);
            //return View();
        }
    }
}