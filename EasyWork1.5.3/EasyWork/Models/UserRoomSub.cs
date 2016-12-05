using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyWork.Models
{
    public class UserRoomSub
    {
        public string U_ID { get; set; }
        public string Name { get; set; }
        public string Pwd { get; set; }
        public string Sex { get; set; }
        public Nullable<int> Age { get; set; }
        public string Tel { get; set; }
        public string Images { get; set; }
        public Nullable<int> D_ID { get; set; }
        public string IsManage { get; set; }
        public int S_ID { get; set; }
        public Nullable<int> M_ID { get; set; }
        public System.DateTime M_Date { get; set; }
        public string M_StartTime { get; set; }
        public string M_EndTime { get; set; }
        public string M_Title { get; set; }
        public string A_U_ID { get; set; }
        public int R_ID { get; set; }
        public string R_Site { get; set; }
        public System.DateTime R_Time { get; set; }
        public string R_Remark { get; set; }
      
        public string M_Site { get; set; }
        public string M_Floor { get; set; }
        public string M_Name { get; set; }
        public int M_PeopleNum { get; set; }
        public Nullable<bool> M_IsWIFI { get; set; }
        public Nullable<bool> M_Projection { get; set; }
    
    
    }
}