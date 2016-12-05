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
    
    public partial class Userinfo
    {
        public Userinfo()
        {
            this.Announcement = new HashSet<Announcement>();
            this.IsRead = new HashSet<IsRead>();
            this.Registration = new HashSet<Registration>();
            this.Reimburse = new HashSet<Reimburse>();
            this.Reimburse1 = new HashSet<Reimburse>();
            this.Subscribe = new HashSet<Subscribe>();
            this.Travelinfo = new HashSet<Travelinfo>();
            this.Travelinfo1 = new HashSet<Travelinfo>();
            this.Vacateinfo = new HashSet<Vacateinfo>();
            this.Vacateinfo1 = new HashSet<Vacateinfo>();
        }
    
        public string U_ID { get; set; }
        public string Name { get; set; }
        public string Pwd { get; set; }
        public string Sex { get; set; }
        public Nullable<int> Age { get; set; }
        public string Tel { get; set; }
        public string Images { get; set; }
        public Nullable<int> D_ID { get; set; }
        public string IsManage { get; set; }
    
        public virtual ICollection<Announcement> Announcement { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<IsRead> IsRead { get; set; }
        public virtual ICollection<Registration> Registration { get; set; }
        public virtual ICollection<Reimburse> Reimburse { get; set; }
        public virtual ICollection<Reimburse> Reimburse1 { get; set; }
        public virtual ICollection<Subscribe> Subscribe { get; set; }
        public virtual ICollection<Travelinfo> Travelinfo { get; set; }
        public virtual ICollection<Travelinfo> Travelinfo1 { get; set; }
        public virtual ICollection<Vacateinfo> Vacateinfo { get; set; }
        public virtual ICollection<Vacateinfo> Vacateinfo1 { get; set; }
    }
}
