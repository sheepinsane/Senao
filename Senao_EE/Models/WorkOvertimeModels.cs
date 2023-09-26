using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Linq;

namespace Senao_EE.Models
{
    public class WorkOvertimeViewModel
    {
        [Required]
        [Display(Name = "工號")]
        public string EmployeeID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [RegularExpression(@"[2][0-1][0-9][0-9]/[0-1][0-9]/[0-3][0-9]", ErrorMessage = "開始日期輸入格式須為yyyy/MM/dd")]
        [Display(Name = "開始日期")]
        public string DateFrom { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [RegularExpression(@"[2][0-1][0-9][0-9]/[0-1][0-9]/[0-3][0-9]", ErrorMessage = "結束日期輸入格式須為yyyy/MM/dd")]
        [Display(Name = "結束日期")]
        public string DateTo { get; set; }

        [Required]
        [RegularExpression(@"[0-2][0-9][0-5][0-9]", ErrorMessage = "開始時間輸入格式須為HHmm")]
        [Display(Name = "開始時間")]
        public string TimeFrom { get; set; }

        [Required]
        [RegularExpression(@"[0-2][0-9][0-5][0-9]", ErrorMessage = "結束時間輸入格式須為HHmm")]
        [Display(Name = "結束時間")]
        public string TimeTo { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "上限 {1} 個字元")]
        [Display(Name = "加班原因")]
        public string Reason { get; set; }

        public int WorkOvertimeSN { get; set; }
    }

    public class v_WorkOvertimeViewModel
    {
        [Display(Name = "工號")]
        public string EmployeeID { get; set; }

        [Display(Name = "部門")]
        public List<SelectListItem> Department { get; set; }

        [Display(Name = "部門")]
        public int DepartmentSN { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [RegularExpression(@"[2][0-1][0-9][0-9]/[0-1][0-9]/[0-3][0-9]", ErrorMessage = "加班日期(起)輸入格式須為yyyy/MM/dd")]
        [Display(Name = "加班日期(起)")]
        public string DateFrom { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [RegularExpression(@"[2][0-1][0-9][0-9]/[0-1][0-9]/[0-3][0-9]", ErrorMessage = "加班日期(訖)輸入格式須為yyyy/MM/dd")]
        [Display(Name = "加班日期(訖)")]
        public string DateTo { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [RegularExpression(@"[2][0-1][0-9][0-9]/[0-1][0-9]/[0-3][0-9]", ErrorMessage = "加班日期(訖)輸入格式須為yyyy/MM/dd")]
        [Display(Name = "填寫日期")]
        public string UpdateTime { get; set; }


        [Required]
        [DataType(DataType.Date)]
        [RegularExpression(@"[2][0-1][0-9][0-9]/[0-1][0-9]/[0-3][0-9]", ErrorMessage = "加班日期(訖)輸入格式須為yyyy/MM/dd")]
        [Display(Name = "新增日期")]
        public string InsertTime { get; set; }

        public List<v_WorkOvertime> v_workOvertime { get; set; }
    }

    public class MonthlyReportViewModel
    {
        [Display(Name = "日期區間")]
        public string DateBetween { get; set; }

        [Display(Name = "部門")]
        public List<SelectListItem> Department { get; set; }

        [Display(Name = "部門")]
        public int DepartmentSN { get; set; }

        public List<sp_MonthlyReport_Result> MonthlyReport { get; set; }
    }
}