using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECWorkflow.Models
{
    public class ECWorkflow
    {
        [DisplayName("產品料號")]
        [StringLength(12,MinimumLength =12, ErrorMessage = "料號須為12碼")]
        [Required(ErrorMessage = "產品料號是必填字段")]
        public string chModelNo { get; set; }     // 数据库字段: chModelNo，类型: varchar(12)

        [DisplayName("產品名稱")]
        [Required(ErrorMessage = "產品名稱是必填字段")]
        public string chModelName { get; set; }   // 数据库字段: chModelName，类型: nvarchar(100)

        [DisplayName("產測程式-流水號")]
        public string chTPNo { get; set; }        // 数据库字段: chTPNo，类型: varchar(12)

        [DisplayName("產測程式名稱")]
        [Required(ErrorMessage = "產測程式名稱是必填字段")]
        public string chTPName { get; set; }      // 数据库字段: chTPName，类型: nvarchar(100)
    }

    public class vECWorkflow : ECWorkflow
    {
        private List<ECWorkflow> _list = new List<ECWorkflow>();
        public List<ECWorkflow> List { get => (_list != null) ? _list: new List<ECWorkflow>(); set => _list = value; }
    }
}