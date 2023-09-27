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
        public JsonResult GetList()
        {
            DBConnection sqlConnection = new DBConnection();
            string query = "SELECT TOP (20) * FROM [ECWorkflow].[dbo].[ECWorkflow]";
            var list = sqlConnection.ExecuteQuery<Models.ECWorkflow>(query);

            var result = JsonConvert.SerializeObject(list);
            return Json(result);
        }

    }
}