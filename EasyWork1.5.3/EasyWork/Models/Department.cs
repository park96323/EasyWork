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
    
    public partial class Department
    {
        public Department()
        {
            this.Userinfo = new HashSet<Userinfo>();
        }
    
        public int D_ID { get; set; }
        public string D_Name { get; set; }
    
        public virtual ICollection<Userinfo> Userinfo { get; set; }
    }
}
