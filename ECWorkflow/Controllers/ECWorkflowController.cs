using DBConn;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECWorkflow.ExtUni;

namespace ECWorkflow.Controllers
{
    public class ECWorkflowController : Controller
    {
        // GET: ECWorkflow
        public ActionResult List()
        {
            DBConnection sqlConnection = new DBConnection();
            string query = "SELECT TOP (1000) * FROM [ECWorkflow].[dbo].[ECWorkflow]";
            var result = sqlConnection.ExecuteQuery<Models.ECWorkflow>(query);
            return View(result);
        }

        public ActionResult Edit(Models.ECWorkflow Model) 
        {
            if (Model.chModelNo == null)
                return View();
            else
            {
                return View(SetViewModel(Model));
            }
        }
        [HttpGet]
        public ActionResult Delete(string chTPNo)
        {
            if (chTPNo == string.Empty)
                return View("Edit");
            else
            {
                return View("Edit", DelViewModel(chTPNo));
            }
        }

        public Models.vECWorkflow SetViewModel(Models.ECWorkflow Model) 
        {
            var tempList = TempData["message"] as Models.vECWorkflow;
            if (tempList == null)
                tempList = new Models.vECWorkflow();
            Model.chTPNo = DateTime.Now.pTimestampString();
            tempList.List.Add(Model);
            TempData["message"] = tempList;
            return tempList;
        }

        public Models.vECWorkflow DelViewModel(string chTPNo)
        {
            var tempList = TempData["message"] as Models.vECWorkflow;
            if (tempList == null)
                tempList = new Models.vECWorkflow();
            var item = tempList.List.Where(x => x.chTPNo == chTPNo).FirstOrDefault();
            tempList.List.Remove((Models.ECWorkflow)item);
            TempData["message"] = tempList;
            tempList.chTPNo = string.Empty;
            return tempList;
        }
        public ActionResult Search(string queryText)
        {
            DBConnection sqlConnection = new DBConnection();
            string query = $"SELECT TOP (1000) * FROM [ECWorkflow].[dbo].[ECWorkflow] " +
                $"Where [chModelNo] like '%{queryText}%'" +
                $"Or [chModelName] like '%{queryText}%'" +
                $"Or [chTPNo] like '%{queryText}%'" +
                $"Or [chTPName] like '%{queryText}%'";

            var result = sqlConnection.ExecuteQuery<Models.ECWorkflow>(query);
            
            return View("List", result);
        }
    }
} 