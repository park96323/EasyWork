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
    
    public partial class MeetingRoom
    {
        public MeetingRoom()
        {
            this.Subscribe = new HashSet<Subscribe>();
        }
    
        public int M_ID { get; set; }
        public string M_Site { get; set; }
        public string M_Floor { get; set; }
        public string M_Name { get; set; }
        public int M_PeopleNum { get; set; }
        public Nullable<bool> M_IsWIFI { get; set; }
        public Nullable<bool> M_Projection { get; set; }
    
        public virtual ICollection<Subscribe> Subscribe { get; set; }
    }
}
