using DBConn;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECWorkflow.Controllers
{
    public class ECWorkflowListController : Controller
    {
        // GET: ECWorkflowList
        [HttpPost]
        public JsonResult GetList(Models.ECWorkflowList model)
        {
            DBConnection sqlConnection = new DBConnection();
            string query = $"SELECT * FROM [ECWorkflow].[dbo].[v_ECWorkflowList] Where chTPNo = '{model.chTPNo}'";
            var list = sqlConnection.ExecuteQuery<Models.vmECWorlflowList>(query);
            var result = JsonConvert.SerializeObject(list);
            return Json(result);
        }

        public ActionResult List(Models.vmECWorlflowList model)
        {
            DBConnection sqlConnection = new DBConnection();
            string query = $"SELECT * FROM [ECWorkflow].[dbo].[v_ECWorkflowList]";
            var list = sqlConnection.ExecuteQuery<Models.vmECWorlflowList>(query);
            return View(list);
        }
    }
}