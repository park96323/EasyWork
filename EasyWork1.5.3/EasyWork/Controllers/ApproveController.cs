using EasyWork.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Providers.Entities;

namespace EasyWork.Controllers
{
    /// <summary>
    /// 审批微应用控制器
    /// </summary>
    public class ApproveController : Controller
    {
        //定义默认用户编号
        private string UserID = "0443103313939774"; //王天
        CompanyDBEntities db = new CompanyDBEntities();

        //首页界面显示
        public ActionResult Index()
        {
            string is_sys = "Ture";
            if (Session["User"] != null || Session["is_sys"] != null)
            {
                UserID = Session["User"].ToString();
                is_sys = Session["is_sys"].ToString();
            }

            //未审批    
            //未审批请假
            SqlParameter[] para1 = new SqlParameter[] { new SqlParameter("@U_ID", UserID), new SqlParameter("@IsApprove", false) };
            List<Vacateinfo> v1 = db.Database.SqlQuery<Vacateinfo>(" proc_IsVacate @U_ID,@IsApprove", para1).ToList();
            ViewData["IsVacate"] = v1;
            //查询请假发起人名称
            List<Userinfo> list = new List<Userinfo>();
            foreach (Vacateinfo item in v1)
            {
                Userinfo u1 = db.Database.SqlQuery<Userinfo>("select s1.U_ID,Name,Pwd,Sex,Age,Tel,Images,D_ID,IsManage from userinfo s1 inner join Vacateinfo s2 on s1.u_id=s2.u_id where s1.u_id='" + item.U_ID + "' and V_ID='" + item.V_ID + "'").First();
                list.Add(u1);
            }

            ViewData["IsVacatename"] = list;
            //未审批出差
            SqlParameter[] para2 = new SqlParameter[] { new SqlParameter("@U_ID", UserID), new SqlParameter("@IsApprove", false) };
            List<Travelinfo> t = db.Database.SqlQuery<Travelinfo>("proc_IsTravel @U_ID,@IsApprove", para2).ToList();
            ViewData["IsTravel"] = t;
            //查询出差发起人名称
            List<Userinfo> Travelname = new List<Userinfo>();
            foreach (Travelinfo item in t)
            {
                Userinfo u2 = db.Database.SqlQuery<Userinfo>("select * from userinfo where u_id='" + item.U_ID + "'").First();
                Travelname.Add(u2);
            }

            ViewData["IsTravelname"] = Travelname;
            //未审批报销
            SqlParameter[] para3 = new SqlParameter[] { new SqlParameter("@U_ID", UserID), new SqlParameter("@IsApprove", false) };
            List<Reimburse> r = db.Database.SqlQuery<Reimburse>("proc_IsReimburse @U_ID,@IsApprove", para3).ToList();
            ViewData["IsReimburse"] = r;
            //查询审批发起人名称
            List<Userinfo> Reimbursename = new List<Userinfo>();
            foreach (Reimburse item in r)
            {
                Userinfo u3 = db.Database.SqlQuery<Userinfo>("select * from userinfo where u_id='" + item.U_ID + "'").First();
                Reimbursename.Add(u3);
            }
            ViewData["IsReimbursename"] = Reimbursename;
            //未审批数量
            ViewData["count"] = v1.Count + t.Count + r.Count;

            //已审批
            //已审批请假
            SqlParameter[] para4 = new SqlParameter[] { new SqlParameter("@U_ID", UserID), new SqlParameter("@IsApprove", 1) };
            List<Vacateinfo> v2 = db.Database.SqlQuery<Vacateinfo>("proc_AlreadyVacate @U_ID,@IsApprove", para4).ToList();
            ViewData["AlreadyVacate"] = v2;
            //已审批出差
            SqlParameter[] para5 = new SqlParameter[] { new SqlParameter("@U_ID", UserID), new SqlParameter("@IsApprove", 1) };
            List<Travelinfo> t2 = db.Database.SqlQuery<Travelinfo>("proc_AlreadyTravel @U_ID,@IsApprove", para5).ToList();
            ViewData["AlreadyTravel"] = t2;
            //查询出差审批人名称
            //已审批报销
            SqlParameter[] para6 = new SqlParameter[] { new SqlParameter("@U_ID", UserID), new SqlParameter("@IsApprove", 1) };
            List<Reimburse> r2 = db.Database.SqlQuery<Reimburse>("proc_AlreadyReimburse @U_ID,@IsApprove", para6).ToList();
            ViewData["AlreadyReimburse"] = r2;
            //我发起的审批
            //我发起的请假
            SqlParameter[] para7 = new SqlParameter[] { new SqlParameter("@U_ID", UserID) };
            List<Vacateinfo> v3 = db.Database.SqlQuery<Vacateinfo>("proc_MyVacate @U_ID", para7).ToList();
            ViewData["MyVacate"] = v3;
            //我发起的出差
            SqlParameter[] para8 = new SqlParameter[] { new SqlParameter("@U_ID", UserID) };
            List<Travelinfo> t3 = db.Database.SqlQuery<Travelinfo>("proc_MyTravel @U_ID", para8).ToList();
            ViewData["MyTravel"] = t3;
            //我发起的报销
            SqlParameter[] para9 = new SqlParameter[] { new SqlParameter("@U_ID", UserID) };
            List<Reimburse> r3 = db.Database.SqlQuery<Reimburse>("proc_MyReimburse @U_ID", para9).ToList();
            ViewData["MyReimburse"] = r3;
            return View();
        }


        //显示发起审批的页面
        public ActionResult AddApprove(string type)
        {
            ViewData["type"] = type;
            string AppPath = Request.RawUrl.ToString(); //目前为了应付url获取到的端口为本地端口问题，后期服务器该用url
            ViewBag.Config = GetConfig.getConfig(Session["appId"].ToString(), AppPath);
            return View();
        }
        [HttpPost]
        //添加请假审批
        public ActionResult AddVacateinfo(string V_Type, string V_StartTime, string V_EndTime, string V_Days, string V_Remark)
        {
            string is_sys = "Ture";
            if (Session["User"] != null || Session["is_sys"] != null)
            {
                UserID = Session["User"].ToString();
                is_sys = Session["is_sys"].ToString();
            }

            bool falg = db.Database.ExecuteSqlCommand("insert into Vacateinfo values('" + UserID + "','" + V_Type + "','" + V_StartTime + "','" + V_EndTime + "','" + V_Days + "','" + V_Remark + "','04362343542545','3',default)") > 0 ? true : false;
            if (falg)
            {
                //发送OA消息至客户端内部
                Messages.OAMessage o = new Messages.OAMessage();
                o.touser = "04362343542545";
                o.agentid = "26281659";
                o.oa.head.bgcolor = "FF0069f9";
                o.oa.body.title = (from u in db.Userinfo where u.U_ID == UserID select u).First().Name + "的请假需要您审批";
                List<Messages.FormItem> ls = new List<Messages.FormItem>();
                ls.Add(new Messages.FormItem() { key = "请假类型：", value = V_Type });
                ls.Add(new Messages.FormItem() { key = "开始时间：", value = V_StartTime });
                ls.Add(new Messages.FormItem() { key = "结束时间：", value = V_EndTime });
                o.oa.body.form = ls;
                o.oa.message_url = Config.WebUrl + "/withoutlogin/login?agentid=26281659&device=Mobile";
                o.oa.pc_message_url = Config.WebUrl + "/withoutlogin/login?agentid=26281659&device=PC";
                Messages.MessageCallBack MessageCallBack = EnterpriseBusiness.SendMessage(o);
                Helper.WriteLog("请假消息ID" + MessageCallBack.messageId);
                return Json("ok");
            }
            else
            {
                return Json("no");
            }
        }
        [HttpPost]
        //添加出差审批
        public ActionResult AddTravelinfo(string T_Site, string T_StartTime, string T_EndTime, string T_Days, string T_Remark)
        {
            string is_sys = "Ture";
            if (Session["User"] != null || Session["is_sys"] != null)
            {
                UserID = Session["User"].ToString();
                is_sys = Session["is_sys"].ToString();
            }

            bool falg = db.Database.ExecuteSqlCommand("insert into Travelinfo values('" + UserID + "','" + T_Site + "','" + T_StartTime + "','" + T_EndTime + "','" + T_Days + "','" + T_Remark + "','04362343542545','3',default)") > 0 ? true : false;
            if (falg)
            {
                //发送OA消息至客户端内部
                Messages.OAMessage o = new Messages.OAMessage();
                o.touser = "04362343542545";
                o.agentid = "26281659";
                o.oa.head.bgcolor = "FF0069f9";
                o.oa.body.title = (from u in db.Userinfo where u.U_ID == UserID select u).First().Name + "的出差需要您审批";
                List<Messages.FormItem> ls = new List<Messages.FormItem>();
                ls.Add(new Messages.FormItem() { key = "出差地点：", value = T_Site });
                ls.Add(new Messages.FormItem() { key = "开始时间：", value = T_StartTime });
                ls.Add(new Messages.FormItem() { key = "结束时间：", value = T_EndTime });
                o.oa.body.form = ls;
                o.oa.message_url = Config.WebUrl + "/withoutlogin/login?agentid=26281659&device=Mobile";
                o.oa.pc_message_url = Config.WebUrl + "/withoutlogin/login?agentid=26281659&device=PC";
                Messages.MessageCallBack MessageCallBack = EnterpriseBusiness.SendMessage(o);
                Helper.WriteLog("出差消息ID" + MessageCallBack.messageId);
                return Json("ok");
            }
            else
            {
                return Json("no");
            }
        }
        [HttpPost]
        //添加报销审批
        public ActionResult AddReimburse(string R_Money, string R_Type, string R_Remark)
        {
            string is_sys = "Ture";
            if (Session["User"] != null || Session["is_sys"] != null)
            {
                UserID = Session["User"].ToString();
                is_sys = Session["is_sys"].ToString();
            }

            bool falg = db.Database.ExecuteSqlCommand("insert into Reimburse values('" + UserID + "','" + R_Money + "','" + R_Type + "','" + R_Remark + "','04362343542545','3',default)") > 0 ? true : false;
            if (falg)
            {
                //发送OA消息至客户端内部
                Messages.OAMessage o = new Messages.OAMessage();
                o.touser = "04362343542545";
                o.agentid = "26281659";
                o.oa.head.bgcolor = "FF0069f9";
                o.oa.body.title = (from u in db.Userinfo where u.U_ID == UserID select u).First().Name + "的报销需要您审批";
                List<Messages.FormItem> ls = new List<Messages.FormItem>();
                ls.Add(new Messages.FormItem() { key = "报销类型：", value = R_Type });
                ls.Add(new Messages.FormItem() { key = "用途：", value = R_Remark });
                o.oa.body.form = ls;
                o.oa.body.rich.num = R_Money;
                o.oa.body.rich.unit = "元";
                o.oa.message_url = Config.WebUrl + "/withoutlogin/login?agentid=26281659&device=Mobile";
                o.oa.pc_message_url = Config.WebUrl + "/withoutlogin/login?agentid=26281659&device=PC";
                Messages.MessageCallBack MessageCallBack = EnterpriseBusiness.SendMessage(o);
                Helper.WriteLog("报销消息ID" + MessageCallBack.messageId);
                return Json("ok");
            }
            else
            {
                return Json("no");
            }
        }
        //审批确认
        //[HttpPost]
        public ActionResult ApproveSure(string type, int id, string isok)
        {
            //请假
            if (type == "1")
            {
                ReturnState(type, id, isok);
                SqlParameter[] para = new SqlParameter[] { new SqlParameter("@V_ID", id), new SqlParameter("@IsAdmission", isok), };
                int i = db.Database.ExecuteSqlCommand("proc_IsPassVacate @V_ID,@IsAdmission", para);
                return Json("ok");
            }


            //出差
            else if (type == "2")
            {
                ReturnState(type, id, isok);
                SqlParameter[] para = new SqlParameter[] { new SqlParameter("@T_ID", id), new SqlParameter("@IsAdmission", isok), };
                int i = db.Database.ExecuteSqlCommand("proc_IsPassTravel @T_ID,@IsAdmission", para);
                return Json("ok");
            }
            //报销
            else
            {
                ReturnState(type, id, isok);
                SqlParameter[] para = new SqlParameter[] { new SqlParameter("@R_ID", id), new SqlParameter("@IsAdmission", isok), };
                int i = db.Database.ExecuteSqlCommand("proc_IsPassReimburse @R_ID,@IsAdmission", para);
                return Json("ok");
            }


        }
        //反馈审批结果
        private void ReturnState(string type, int id, string isok)
        {
            //发送OA消息至客户端内部
            Messages.OAMessage o = new Messages.OAMessage();
            
            o.agentid = "26281659";
            o.oa.head.bgcolor = "FF0069f9";
            o.oa.body.title = "审批已被处理";
            List<Messages.FormItem> ls = new List<Messages.FormItem>();
            if (type == "1")
            {
                o.touser = (from v in db.Vacateinfo where v.V_ID == id select v).First().U_ID;
                ls.Add(new Messages.FormItem() { key = "请假类型：", value = (from v in db.Vacateinfo where v.V_ID == id select v).First().V_Type });
                ls.Add(new Messages.FormItem() { key = "开始时间：", value = (from v in db.Vacateinfo where v.V_ID == id select v).First().V_StartTime.ToLocalTime().ToString() });
                ls.Add(new Messages.FormItem() { key = "结束时间：", value = (from v in db.Vacateinfo where v.V_ID == id select v).First().V_EndTime.ToLocalTime().ToString() });
            }
            else if (type == "2")
            {
                o.touser = (from t in db.Travelinfo where t.T_ID == id select t).First().U_ID;
                ls.Add(new Messages.FormItem() { key = "出差地点：", value = (from t in db.Travelinfo where t.T_ID == id select t).First().T_Site });
                ls.Add(new Messages.FormItem() { key = "开始时间：", value = (from t in db.Travelinfo where t.T_ID == id select t).First().T_StartTime.ToLocalTime().ToString() });
                ls.Add(new Messages.FormItem() { key = "结束时间：", value = (from t in db.Travelinfo where t.T_ID == id select t).First().T_EndTime.ToLocalTime().ToString() });
            }
            else
            {
                o.touser = (from r in db.Reimburse where r.R_ID == id select r).First().U_ID;
                ls.Add(new Messages.FormItem() { key = "报销类型：", value = (from r in db.Reimburse where r.R_ID==id select r).First().R_Type });
                ls.Add(new Messages.FormItem() { key = "用途：", value = (from r in db.Reimburse where r.R_ID == id select r).First().R_Remark });
                ls.Add(new Messages.FormItem() { key = "金额：", value = (from r in db.Reimburse where r.R_ID == id select r).First().R_Money+"元"});
            }
            o.oa.body.form = ls;
            o.oa.body.rich.num = isok == "1" ? "已通过" : "已拒绝";
            o.oa.message_url = Config.WebUrl + "/withoutlogin/login?agentid=26281659&device=Mobile";
            o.oa.pc_message_url = Config.WebUrl + "/withoutlogin/login?agentid=26281659&device=PC";
            Messages.MessageCallBack MessageCallBack = EnterpriseBusiness.SendMessage(o);
            Helper.WriteLog("审批消息ID" + MessageCallBack.messageId);
        }
        //钉钉API测试
        [HttpPost]
        public ActionResult test(string start, string end)
        {
            bool result = false;
            DateTime t1 = DateTime.Parse(start);
            DateTime t2 = DateTime.Parse(end);

            result = t1 < t2 ? true : false;
            return Json(result.ToString());
        }

    }
}
