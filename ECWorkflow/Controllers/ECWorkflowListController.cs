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
            string query = $"SELECT * FROM[dbo].[v_ECWorkflowList] Where chTPNo = '{model.chTPNo}'";
            var list = sqlConnection.ExecuteQuery<Models.vECWorkflowList>(query);
            var result = JsonConvert.SerializeObject(list);
            return Json(result);
        }

        public ActionResult List(Models.vECWorkflowList model)
        {
            var list = GetvECWorkflowList(model);
            return View(list);
        }

        public ActionResult Edit(Models.vECWorkflowList model)
        {
            if (model == null || model.chTPNo == null)
                return View();

            //model.chTPNo = "FA7000361";
            DBConnection sqlConnection = new DBConnection();

            string query = $"SELECT TOP 1 * FROM[dbo].[v_ECWorkflowList] Where chTPNo = '{model.chTPNo}' Order by chTPNo2 Desc";
            var lastModel = sqlConnection.ExecuteQuery<Models.vECWorkflowList>(query).FirstOrDefault();

            //lastModel is null 去撈基本檔案做新的新增資料
            if (lastModel == null)
            {
                sqlConnection = new DBConnection();
                query = $"SELECT TOP (1000) * FROM[dbo].[ECWorkflow] " +
                        $"Where [chTPNo] = '{model.chTPNo}'";

                var result = sqlConnection.ExecuteQuery<Models.ECWorkflow>(query);
                if (result.Any())
                {
                    var modelFirst = result.FirstOrDefault();
                    lastModel = new Models.vECWorkflowList();
                    lastModel.chTPNo2 = "199";//+1預設變成200
                    lastModel.intStatus = (int)Models.ECWorkflowList.StatusEnum.申請EC單號;
                    lastModel.chModelName = modelFirst.chModelName;
                    lastModel.chModelNo = modelFirst.chModelNo;
                    lastModel.chTPBasic = modelFirst.chTPName;
                    lastModel.chTPNo = modelFirst.chTPNo;
                }
            }

            query = $"SELECT * FROM[dbo].[v_ECWorkflowList] Where chTPNo = '{model.chTPNo}' Order by chTPNo2 ";
            var list = sqlConnection.ExecuteQuery<Models.vECWorkflowList>(query);

            lastModel.List = new List<Models.vECWorkflowList>();
            if (list.Any())
                lastModel.List.AddRange(list);

            return View(lastModel);
        }

        public ActionResult Save(Models.vECWorkflowList model)
        {
            if (model == null || model.chTPNo == null)
                return View("List");


            DBConnection sqlConnection = new DBConnection();
            //新增資料到系統中
            Models.ECWorkflowList detail = model;
            detail.intStatus = (int)Models.ECWorkflowList.StatusEnum.申請EC單號;
            sqlConnection.InsertEntity(detail, "ECWorkflowList");

            string query = $"SELECT TOP 1 * FROM[dbo].[v_ECWorkflowList] Where chTPNo = '{model.chTPNo}' Order by chTPNo2 Desc";
            var lastModel = sqlConnection.ExecuteQuery<Models.vECWorkflowList>(query).FirstOrDefault();

            query = $"SELECT * FROM[dbo].[v_ECWorkflowList] Where chTPNo = '{model.chTPNo}' Order by chTPNo2 ";
            var list = sqlConnection.ExecuteQuery<Models.vECWorkflowList>(query);

            lastModel.List = new List<Models.vECWorkflowList>();
            if (list.Any())
                lastModel.List.AddRange(list);

            return View("Edit", lastModel);

        }

        public ActionResult Delete(Models.vECWorkflowList model)
        {
            DBConnection sqlConnection = new DBConnection();
            string SQL = $"Update ECWorkflowList Set intStatus =  {(int)Models.ECWorkflowList.StatusEnum.取消申請} Where (chTPNo + chTPNo2) = ('{model.chTPAll}')";
            sqlConnection.Update(SQL);

            var list = GetvECWorkflowList(model);
            return View("List",list);
        }

        public ActionResult StatusChangeList(Models.vECWorkflowList model)
        {
            model.List = GetvECWorkflowList(model).ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangeStatusToData(string Result,string status)
        {
            // 將JSON數據轉換為.NET對象（例如List<string>）
            DBConnection sqlConnection = new DBConnection();
            List<string> selectedValues = JsonConvert.DeserializeObject<List<string>>(Result);
            string result = string.Join("','", selectedValues);
            string SQL = $"Update ECWorkflowList Set intStatus =  {status} Where (chTPNo + chTPNo2) in ('{result}')";
            sqlConnection.Update(SQL);
            return Json(new { message = "Success" });
        }

        public IEnumerable<Models.vECWorkflowList> GetvECWorkflowList(Models.vECWorkflowList model)
        {

            DBConnection sqlConnection = new DBConnection();
            string query = $"SELECT * FROM[dbo].[v_ECWorkflowList]" +
                $"Where ([chTPAll] like '%{model.queryText}%' " +
                $"Or [chTPName] like '%{model.queryText}%' " +
                $"Or [chECNo] like '%{model.queryText}%' " +
                $"Or [chCreater] like '%{model.queryText}%' " +
                $"Or [chModelNo] like '%{model.queryText}%') ";
            if (model.intStatus != (int)Models.ECWorkflowList.StatusEnum.所有)
                query += $"And [intStatus] = '{model.intStatus}'";
            if (model.chApplyFirst != string.Empty && model.chApplyFirst != null)
                query += $"And [chApplyDate] >= '{model.chApplyFirst}'";
            if (model.chApplyEnd != string.Empty && model.chApplyEnd != null)
                query += $"And [chApplyDate] <= '{model.chApplyEnd}'";


            var list = sqlConnection.ExecuteQuery<Models.vECWorkflowList>(query);
            if (list == null)
                list = new List<Models.vECWorkflowList>();

            return list;
        }
    }
}