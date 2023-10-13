using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ECWorkflow.ExtUni;

namespace ECWorkflow.Models
{
    public class ECWorkflowList
    {
        [DisplayName("申請日期")]
        public DateTime chApplyDate { get; set; }

        [DisplayName("流水號")]
        public string chTPNo { get; set; }

        [DisplayName("程式版本號")]
        public string chTPNo2 { get; set; }

        [DisplayName("程式名稱")]
        public string chTPName { get; set; }

        [DisplayName("建立者")]
        public string chCreater { get; set; }

        private int _intStatus = 0;
        [DisplayName("狀態")]
        public int intStatus { get => _intStatus; set => _intStatus = value; }

        [DisplayName("EC單號")]
        public string chECNo { get; set; }

        [DisplayName("EC建立日期")]
        public string chECCreateDate { get; set; }

        [DisplayName("EC完成日期")]
        public string chECFinishDate { get; set; }


        public enum StatusEnum
        {
            所有 = 0,
            申請版本料號 = 1,
            版本料號申請完成 = 2,
            發行軟體管制表 = 3,
            取消申請 = 4,
            申請EC單號 = 5,
            EC已完成 = 6
        }

    }



    public class vECWorkflowList: ECWorkflowList
    {
        [DisplayName("狀態")]
        public string chStatus { 
            get {

                string statusText = Enum.GetName(typeof(StatusEnum), intStatus);
                return statusText;
            } 
            set { }
        }

        [DisplayName("料號")]
        public string chModelNo { get; set; }

        [DisplayName("EC完成日期")]
        public string chModelName { get; set; }

        [DisplayName("產品料號")]
        public string chTPBasic { get; set; }

        [DisplayName("流水號+程式版本號")]
        public string chTPAll { get; set; }

        public string chApplyFirst { get; set; }

        public string chApplyEnd { get; set; }

        public string queryText { get; set; }

        public string chTPNo2PlusOne 
        {
            get { return (chTPNo2.pToInt() + 1).ToString(); }
        }
        
        public string chTPNameCombine
        {
            get {
                    string inputString = chTPNo2PlusOne;
                    double number = double.Parse(inputString) / 100;
                    string formattedString = number.ToString("F2"); // 保留两位小数
                    return $"Test {chModelName} {chTPBasic} {formattedString}"; 
               }
        }

        public List<vECWorkflowList> List { get; set; }


    }
}