using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Linq;

namespace Senao_EE.Models
{
    public class WorkingRecordModel
    {

    }

    public class v_WrokingRecordViewModel 
    {
        [Display(Name = "年度")]
        public int Year { get; set; }

        [Display(Name = "週別")]
        public int Week { get; set; }

        [Display(Name = "部門")]
        public List<SelectListItem> Department { get; set; }

        [Display(Name = "部門")]
        public int DepartmentSN { get; set; }
    }
}