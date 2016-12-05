using EasyWork.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyWork.Controllers
{
    public class MeetingRoomController : Controller
    {

        //定义默认用户编号
        private CompanyDBEntities db = new CompanyDBEntities();

        //显示已预约的会议
        public ActionResult Index()
        {
            string sql = "select * from Userinfo s1 inner join Subscribe s2 on s1.U_ID=s2.A_U_ID inner join MeetingRoom s3 on s2.M_ID=s3.M_ID order by s2.M_Date  desc,s2.M_StartTime asc";
            List<UserRoomSub> urs = db.Database.SqlQuery<UserRoomSub>(sql).ToList();
            ViewData["UserRoomSub"] = urs;
            //获取当前用户的信息
            if (Session["User"] != null && Session["is_sys"] != null)
            {
                Dictionary<string, string> user = new Dictionary<string, string>() {
                    { "UserID",Session["User"].ToString()},
                    { "is_sys",Session["is_sys"].ToString()}
                };
                ViewBag.User = user;
            }

            return View();
        }
        //查询会议室
        [HttpPost]
        public ActionResult SelMeet(string sel)
        {
            List<UserRoomSub> urs = db.Database.SqlQuery<UserRoomSub>("select * from Userinfo s1 inner join Subscribe s2 on s1.U_ID=s2.A_U_ID inner join MeetingRoom s3 on s2.M_ID=s3.M_ID where M_Title like '%" + sel + "%'").ToList();
            ViewData["UserRoomSub"] = urs;
            return Json(ViewData["UserRoomSub"]);
        }
        //显示添加会议室的页面
        public ActionResult AddRoom()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddRoom(string M_Site, string M_Floor, string M_Name, string M_PeopleNum, bool M_IsWIFI, bool M_Projection)
        {
            bool falg = db.Database.ExecuteSqlCommand("insert into MeetingRoom values('" + M_Name + "','" + M_Floor + "','" + M_Site + "','" + M_PeopleNum + "','" + M_IsWIFI + "','" + M_Projection + "')") > 0 ? true : false;
            if (falg)
            {
                //发送OA消息至客户端内部
                Messages.OAMessage o = new Messages.OAMessage();
                o.touser = "@all";
                o.agentid = "26281717";
                o.oa.body.title = "有新的会议室可用";
                List<Messages.FormItem> ls = new List<Messages.FormItem>();
                ls.Add(new Messages.FormItem() { key = "会议室：", value = M_Name });
                ls.Add(new Messages.FormItem() { key = "楼层：", value = M_Floor });
                ls.Add(new Messages.FormItem() { key = "地点：", value = M_Site });
                ls.Add(new Messages.FormItem() { key = "可容纳人数：", value = M_PeopleNum });
                ls.Add(new Messages.FormItem() { key = "WIFI：", value = M_IsWIFI == true ? "支持" : "不支持" });
                ls.Add(new Messages.FormItem() { key = "投影仪：", value = M_Projection == true ? "支持" : "不支持" });
                o.oa.body.form = ls;
                o.oa.head.bgcolor = "FF09bce9";
                o.oa.message_url = Config.WebUrl + "/withoutlogin/login?agentid=26281717&device=Mobile";
                o.oa.pc_message_url = Config.WebUrl + "/withoutlogin/login?agentid=26281717&device=PC";
                Messages.MessageCallBack MessageCallBack = EnterpriseBusiness.SendMessage(o);
                Helper.WriteLog("预约会议室消息ID" + MessageCallBack.messageId);
                return Json("ok");
            }
            else
            {
                return Json("no");
            }
        }

        //显示预约会议室页面
        public ActionResult Subscribe(int room_id = 0)
        {
            List<MeetingRoom> all = db.Database.SqlQuery<MeetingRoom>("select * from MeetingRoom").ToList();
            ViewData["all"] = all;
            //把当前用户的编号传到页面
            ViewData["UserID"] = Session["User"].ToString();
            return View(room_id);
        }
        //预约会议室
        [HttpPost]
        public ActionResult Subscribe(string M_ID, string M_Date, string M_StartTime, string M_EndTime, string M_Title, string A_U_ID)
        {
            bool falg = db.Database.ExecuteSqlCommand("insert into Subscribe values(" + M_ID + ",'" + DateTime.Parse(M_Date) + "','" + M_StartTime + "','" + M_EndTime + "','" + M_Title + "','" + A_U_ID + "')") > 0 ? true : false;
            if (falg)
            {
                //发送OA消息至客户端内部
                Messages.OAMessage o = new Messages.OAMessage();
                o.touser = "@all";
                o.agentid = "26281717";
                o.oa.body.title = "会议通知";
                int m_ID = int.Parse(M_ID);
                List<Messages.FormItem> ls = new List<Messages.FormItem>();
                ls.Add(new Messages.FormItem() { key = "会议通知主题：", value = M_Title });
                ls.Add(new Messages.FormItem() { key = "会议室：", value = (from m in db.MeetingRoom where m.M_ID == m_ID select m).First().M_Name });
                ls.Add(new Messages.FormItem() { key = "地点：", value = (from m in db.MeetingRoom where m.M_ID == m_ID select m).First().M_Site });
                ls.Add(new Messages.FormItem() { key = "时间：", value = M_Date + " " + M_StartTime + "~" + M_EndTime });
                ls.Add(new Messages.FormItem() { key = "发起人：", value = (from u in db.Userinfo where u.U_ID == A_U_ID select u).First().Name });
                o.oa.body.form = ls;
                o.oa.head.bgcolor = "FF09bce9";
                o.oa.body.content = "您有一个会议需要出席,请及时调整行程,若不便出席，请联系会议组织方";
                o.oa.message_url = Config.WebUrl + "/withoutlogin/login?agentid=26281717&device=Mobile";
                o.oa.pc_message_url = Config.WebUrl + "/withoutlogin/login?agentid=26281717&device=PC"; 
                Messages.MessageCallBack MessageCallBack = EnterpriseBusiness.SendMessage(o);
                Helper.WriteLog("预约会议室消息ID" + MessageCallBack.messageId);
                return Json("ok");
            }
            else
            {
                return Json("no");
            }
        }
        //显示会议室信息
        public ActionResult RoomInfo()
        {
            //查询全部
            List<MeetingRoom> all = db.Database.SqlQuery<MeetingRoom>("select * from MeetingRoom").ToList();
            ViewData["all"] = all;
            //查询有wifi
            List<MeetingRoom> wifi = db.Database.SqlQuery<MeetingRoom>("select * from MeetingRoom where M_IsWIFI='true'").ToList();
            ViewData["wifi"] = wifi;
            //查询有投影仪
            List<MeetingRoom> pro = db.Database.SqlQuery<MeetingRoom>("select * from MeetingRoom where M_Projection='true'").ToList();
            ViewData["pro"] = pro;
            return View();
        }
    }
}
