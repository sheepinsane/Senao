using Senao_EE.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Senao_EE.Controllers
{
    public class WorkOvertimeController : Controller
    {
        private DateTime tmFrom = new DateTime((DateTime.Now.Month == 1 ? DateTime.Now.Year - 1 : DateTime.Now.Year),
                (DateTime.Now.Month == 1 ? 12 : DateTime.Now.Month - 1), 26);
        private DateTime tmTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 25).AddDays(1);
        public ActionResult List()
        {
            //初始化
            DateTime woTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime woFrom = woTo.AddDays(-7);

            v_WorkOvertimeViewModel model = new v_WorkOvertimeViewModel()
            {
                DateFrom = woFrom.ToString("yyyy/MM/dd"),
                DateTo = woTo.ToString("yyyy/MM/dd"),
                Department = GetDepartmentList(),
                v_workOvertime = GetWorkOvertime(woFrom, woTo, 1, string.Empty)//1:工程處
            };

            Session["v_workOvertime"] = model.v_workOvertime;

            return View(model);
        }

        [HttpPost]
        public ActionResult List(v_WorkOvertimeViewModel model)
        {
            //1.驗證使用者輸入資訊
            //2.查詢加班紀錄
            model.Department = GetDepartmentList();
            model.v_workOvertime = new List<v_WorkOvertime>();

            bool isSuccess = true;

            if (!string.IsNullOrEmpty(model.EmployeeID) && GetEmployee(model.EmployeeID) == null)
            {
                ModelState.AddModelError("", "【" + model.EmployeeID + "】查無此工號");
                isSuccess = false;
            }

            DateTime woFrom, woTo;
            if (!DateTime.TryParseExact(model.DateFrom.Trim(), "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out woFrom))
            {
                ModelState.AddModelError("", model.DateFrom.Trim() + "加班日期(起)格式有誤");
                isSuccess = false;
            }

            if (!DateTime.TryParseExact(model.DateTo.Trim(), "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out woTo))
            {
                ModelState.AddModelError("", model.DateTo.Trim() + "加班日期(訖)格式有誤");
                isSuccess = false;
            }

            if (woFrom > woTo)
            {
                ModelState.AddModelError("", "加班日期(起)須早於或等於加班日期(訖)");
                isSuccess = false;
            }

            if (isSuccess)
            {
                //加班紀錄查詢清單
                model.v_workOvertime = GetWorkOvertime(woFrom, woTo, model.DepartmentSN, model.EmployeeID);
                Session["v_workOvertime"] = model.v_workOvertime;
            }

            return View(model);
        }

        public ActionResult MonthlyReport()
        {
            MonthlyReportViewModel model = new MonthlyReportViewModel()
            {
                Department = GetDepartmentList(),
                DateBetween = tmFrom.ToString("yyyy/MM/dd") + " ~ " + tmTo.AddDays(-1).ToString("yyyy/MM/dd"),
                MonthlyReport = GetMonthlyReport(tmFrom, tmTo, 1)//1:工程處
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult MonthlyReport(MonthlyReportViewModel model)
        {
            model.Department = GetDepartmentList();
            model.DateBetween = tmFrom.ToString("yyyy/MM/dd") + " ~ " + tmTo.AddDays(-1).ToString("yyyy/MM/dd");
            model.MonthlyReport = GetMonthlyReport(tmFrom, tmTo, model.DepartmentSN);

            return View(model);
        }

        public FileContentResult ExportCSV()
        {
            var csv = new StringBuilder();
            csv.AppendLine("部門,工號,姓名,開始時間,結束時間,加班時數,加班原因,週別,新增日期");


            var allLines = (from item in ((List<v_WorkOvertime>)Session["v_WorkOvertime"])
                            select new object[] 
                            { 
                                item.DepartmentName, 
                                item.EmployeeID, 
                                item.EmployeeName, 
                                string.Format("\"{0}\"",item.DateFrom +" "+ item.TimeFrom), 
                                string.Format("\"{0}\"",item.DateTo +" "+ item.TimeTo), 
                                string.Format("\"{0}\"",item.Hours + "小時"), 
                                string.Format("\"{0}\"", item.Reason),                             
                                string.Format("\"{0}\"", new TaiwanCalendar().GetWeekOfYear(Convert.ToDateTime(item.DateFrom), 
                                System.Globalization.CalendarWeekRule.FirstDay, System.DayOfWeek.Sunday).ToString()),    
                                string.Format("\"{0}\"",item.InsertTime), 
                            }).ToList();

            allLines.ForEach(line =>
            {
                csv.AppendLine(string.Join(",", line));
            });

            Encoding encode = Encoding.GetEncoding("Big5");

            return File(encode.GetBytes(csv.ToString()), "text/csv", string.Format("加班紀錄{0}.csv", DateTime.Now.ToString("yyyyMMddHHmm")));
        }

        public ActionResult Edit(int WorkOvertimeSN = 0)
        {
            //初始化
            if (WorkOvertimeSN == 0)
            {
                ViewBag.Action = "新增";
                WorkOvertimeViewModel model = new WorkOvertimeViewModel()
                {
                    DateFrom = DateTime.Now.ToString("yyyy/MM/dd"),
                    DateTo = DateTime.Now.ToString("yyyy/MM/dd"),
                    TimeFrom = "1800",
                    TimeTo = "1930"
                };
                return View(model);
            }
            else
            {
                ViewBag.Action = "修改";
                Senao_EEEntities db = new Senao_EEEntities();

                v_WorkOvertime wo = GetWorkOvertime(WorkOvertimeSN);

                WorkOvertimeViewModel model = new WorkOvertimeViewModel()
                {
                    WorkOvertimeSN = WorkOvertimeSN,
                    EmployeeID = wo.EmployeeID,
                    DateFrom = wo.DateFrom,
                    DateTo = wo.DateTo,
                    TimeFrom = wo.TimeFrom.Replace(":", ""),
                    TimeTo = wo.TimeTo.Replace(":", ""),
                    Reason = wo.Reason
                };
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Edit(WorkOvertimeViewModel model)
        {
            //1.驗證使用者輸入資訊
            //2.儲存加班紀錄

            bool isSuccess = true;

            Employee employee = GetEmployee(model.EmployeeID);
            if (employee == null)
            {
                ModelState.AddModelError("", "【" + model.EmployeeID + "】查無此工號");
                isSuccess = false;
            }

            string strFrom = string.Format("{0} {1}", model.DateFrom.Trim(), model.TimeFrom.Trim());
            string strTo = string.Format("{0} {1}", model.DateTo.Trim(), model.TimeTo.Trim());
            DateTime woFrom, woTo;
            if (!DateTime.TryParseExact(strFrom, "yyyy/MM/dd HHmm", CultureInfo.InvariantCulture, DateTimeStyles.None, out woFrom))
            {
                ModelState.AddModelError("", strFrom + "加班開始時間格式有誤");
                isSuccess = false;
            }
            if (!DateTime.TryParseExact(strTo, "yyyy/MM/dd HHmm", CultureInfo.InvariantCulture, DateTimeStyles.None, out woTo))
            {
                ModelState.AddModelError("", strTo + "加班結束時間格式有誤");
                isSuccess = false;
            }
            if (woFrom >= woTo)
            {
                ModelState.AddModelError("", "加班開始時間須早於加班結束時間");
                isSuccess = false;
            }

            if (isSuccess)
            {
                Senao_EEEntities db = new Senao_EEEntities();

                if (model.WorkOvertimeSN == 0)
                {
                    //新增
                    WorkOvertime wo = new WorkOvertime()//Employee = employee,
                    {
                        EmployeeSN = employee.EmployeeSN,
                        TimeFrom = woFrom,
                        TimeTo = woTo,
                        Reason = model.Reason,
                        UpdateTime = DateTime.Now,
                        InsertTime = DateTime.Now
                    };

                    db.WorkOvertime.Add(wo);
                }
                else
                {
                    //修改
                    WorkOvertime wo = db.WorkOvertime.Find(model.WorkOvertimeSN);
                    wo.EmployeeSN = employee.EmployeeSN;
                    wo.TimeFrom = woFrom;
                    wo.TimeTo = woTo;
                    wo.Reason = model.Reason;
                    wo.UpdateTime = DateTime.Now;
                }

                db.SaveChanges();

                //確認本月累計加班時數
                double? totalHours = (from v in db.v_WorkOvertime
                                      where v.EmployeeID == model.EmployeeID
                                      && (v.OrgFrom >= tmFrom && v.OrgFrom <= tmTo)
                                      select v.Hours).ToList().Sum();
                if (totalHours > 50)
                {
                    TempData["message"] = "工號 " + model.EmployeeID + " 本月加班時數已累計【 " + totalHours + " 小時】，請留意!";
                }

                return RedirectToAction("List", "WorkOvertime");
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult Delete(int WorkOvertimeSN = 0)
        {
            //刪除加班紀錄

            Senao_EEEntities db = new Senao_EEEntities();

            WorkOvertime workOvertime = db.WorkOvertime.Find(WorkOvertimeSN);
            db.WorkOvertime.Remove(workOvertime);
            db.SaveChanges();

            return RedirectToAction("List", "WorkOvertime");
        }

        private List<v_WorkOvertime> GetWorkOvertime(DateTime woFrom, DateTime woTo, int DepartmentSN, string EmployeeID)
        {
            woTo = woTo.AddDays(1);
            Senao_EEEntities db = new Senao_EEEntities();

            //DepartmentSN=1:工程處
            var workOvertime = from view in db.v_WorkOvertime
                               where (view.OrgFrom >= woFrom && view.OrgFrom <= woTo)
                               && (DepartmentSN == 1 || view.DepartmentSN == DepartmentSN)
                               && (string.IsNullOrEmpty(EmployeeID) || view.EmployeeID == EmployeeID.Trim())
                               orderby view.UpdateTime descending
                               select view;

            return workOvertime.ToList();
        }

        private v_WorkOvertime GetWorkOvertime(int WorkOvertimeSN)
        {
            Senao_EEEntities db = new Senao_EEEntities();

            var workOvertime = from view in db.v_WorkOvertime
                               where view.WorkOvertimeSN == WorkOvertimeSN
                               select view;

            return workOvertime.FirstOrDefault();
        }

        private List<SelectListItem> GetDepartmentList()
        {
            Senao_EEEntities db = new Senao_EEEntities();

            //部門下拉選單
            var department = from table in db.Department select table;
            SelectList departmentList = new SelectList(department, "DepartmentSN", "Name");

            return departmentList.ToList();
        }

        private Employee GetEmployee(string EmployeeID)
        {
            Senao_EEEntities db = new Senao_EEEntities();

            var employee = from table in db.Employee where table.ID == EmployeeID.Trim() select table;

            return employee.FirstOrDefault();
        }

        private List<sp_MonthlyReport_Result> GetMonthlyReport(DateTime woFrom, DateTime woTo, int DepartmentSN)
        {
            Senao_EEEntities db = new Senao_EEEntities();

            //DepartmentSN=1:工程處
            var monthlyReport = from sp in db.sp_MonthlyReport(woFrom.ToString("yyyy-MM-dd"),
                                    woTo.ToString("yyyy-MM-dd"), DepartmentSN)
                                select sp;

            return monthlyReport.ToList();
        }
    }
}