//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace EasyWork.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reimburse
    {
        public int R_ID { get; set; }
        public string U_ID { get; set; }
        public decimal R_Money { get; set; }
        public string R_Type { get; set; }
        public string R_Remark { get; set; }
        public string A_U_ID { get; set; }
        public string IsAdmission { get; set; }
        public bool IsApprove { get; set; }
    
        public virtual Userinfo Userinfo { get; set; }
        public virtual Userinfo Userinfo1 { get; set; }
    }
}