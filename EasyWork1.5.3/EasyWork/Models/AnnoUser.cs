using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyWork.Models
{
    public class AnnoUser
    {
        public int A_ID { get; set; }
        public string U_ID { get; set; }
        public DateTime A_Time { get; set; }
        public string A_Image { get; set; }
        public string A_Title { get; set; }
        public string A_Content { get; set; }
        public string Name { get; set; }
        public string Pwd { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        public string Tel { get; set; }
        public string Images { get; set; }
        public Nullable<int> D_ID { get; set; }
        public string IsManage { get; set; }
    }
}