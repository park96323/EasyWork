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
    
    public partial class Registration
    {
        public int R_ID { get; set; }
        public string U_ID { get; set; }
        public string R_Site { get; set; }
        public System.DateTime R_Time { get; set; }
        public string R_Remark { get; set; }
    
        public virtual Userinfo Userinfo { get; set; }
    }
}