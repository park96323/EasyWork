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
    
    public partial class Announcement
    {
        public Announcement()
        {
            this.IsRead = new HashSet<IsRead>();
        }
    
        public int A_ID { get; set; }
        public string U_ID { get; set; }
        public Nullable<System.DateTime> A_Time { get; set; }
        public string A_Image { get; set; }
        public string A_Title { get; set; }
        public string A_Content { get; set; }
    
        public virtual Userinfo Userinfo { get; set; }
        public virtual ICollection<IsRead> IsRead { get; set; }
    }
}
