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
    
    public partial class Subscribe
    {
        public int S_ID { get; set; }
        public Nullable<int> M_ID { get; set; }
        public System.DateTime M_Date { get; set; }
        public string M_StartTime { get; set; }
        public string M_EndTime { get; set; }
        public string M_Title { get; set; }
        public string A_U_ID { get; set; }
    
        public virtual MeetingRoom MeetingRoom { get; set; }
        public virtual Userinfo Userinfo { get; set; }
    }
}
