//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Senao_EE
{
    using System;
    using System.Collections.Generic;
    
    public partial class v_ProduceChangeLog
    {
        public int ProduceChangeLogSN { get; set; }
        public string PartNumbar { get; set; }
        public string Name { get; set; }
        public System.DateTime ReleaseDate { get; set; }
        public string Version { get; set; }
        public string Note { get; set; }
        public Nullable<int> TE_EmployeeSN { get; set; }
        public Nullable<int> IE_EmployeeSN { get; set; }
        public Nullable<int> PE_EmployeeSN { get; set; }
        public string TE_Note { get; set; }
        public string IE_Note { get; set; }
        public string PE_Note { get; set; }
        public Nullable<System.DateTime> TE_FinishDate { get; set; }
        public Nullable<System.DateTime> IE_FinishDate { get; set; }
        public Nullable<System.DateTime> PE_FinishDate { get; set; }
        public string TE_Name { get; set; }
        public string IE_Name { get; set; }
        public string PE_Name { get; set; }
    }
}