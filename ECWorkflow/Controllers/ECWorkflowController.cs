﻿using DBConn;
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
            string query = "SELECT TOP (1000) * FROM ECWorkflow";
            var result = sqlConnection.ExecuteQuery<Models.ECWorkflow>(query);
            return View(result);
        }

        public ActionResult Edit(Models.ECWorkflow Model) 
        {
            if (Model.chModelNo == null)
            {
                TempData["message"] = null;
                return View();
            }
            else
            {
                return View(SetViewModel(Model));
            }
        }

        public ActionResult Update(Models.ECWorkflow Model)
        {
            return View(Model);
        }

        public ActionResult UpdateName(Models.ECWorkflow Model)
        {
            DBConnection sqlConnection = new DBConnection();
            string SQL = $"Update ECWorkflow Set chModelName =  '{Model.chModelName}' Where (chModelNo) in ('{Model.chModelNo}')";
            sqlConnection.Update(SQL);

            sqlConnection = new DBConnection();
            string query = "SELECT TOP (1000) * FROM ECWorkflow";
            var result = sqlConnection.ExecuteQuery<Models.ECWorkflow>(query);
            return View("List",result);
        }

        public ActionResult Save()
        {
            DBConnection sqlConnection = new DBConnection();
            string query = "SELECT TOP (1000) * FROM [ECWorkflow]";
            var  item =  GetViewModel();
            foreach (Models.ECWorkflow row in item.List) 
            {
                sqlConnection.ExecuteInsert($@"INSERT INTO [ECWorkflow]
                                            ([chModelName],[chTPName],[chModelNo],[chTPNo])
                                            VALUES('{row.chModelName}','{row.chTPName}','{row.chModelNo}','A12345678')");
            }

            
            var result = sqlConnection.ExecuteQuery<Models.ECWorkflow>(query);

            return View("List", result);
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
            tempList.List.Add(Model);
            TempData["message"] = tempList;

            return tempList;
        }

        public Models.vECWorkflow GetViewModel()
        {
            var tempList = TempData["message"] as Models.vECWorkflow;
            if (tempList == null)
                tempList = new Models.vECWorkflow();
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
            return tempList;
        }
        public ActionResult Search(string queryText)
        {
            DBConnection sqlConnection = new DBConnection();
            string query = $"SELECT TOP (1000) * FROM [dbo].[ECWorkflow] " +
                $"Where [chModelNo] like '%{queryText}%'" +
                $"Or [chModelName] like '%{queryText}%'" +
                $"Or [chTPNo] like '%{queryText}%'" +
                $"Or [chTPName] like '%{queryText}%'";

            var result = sqlConnection.ExecuteQuery<Models.ECWorkflow>(query);
            
            return View("List", result);
        }
    }
} 