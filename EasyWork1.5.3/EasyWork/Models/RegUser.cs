using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyWork.Models
{
    public class RegUser
    {
        public int R_ID { get; set; }
        public string Name { get; set; }
        public string U_ID { get; set; }
        public string R_Site { get; set; }
        public System.DateTime R_Time { get; set; }
        public string R_Remark { get; set; }
    }
}