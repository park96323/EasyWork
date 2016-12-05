using EasyWork.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyWork.Controllers
{
    /// <summary>
    /// 公告微应用控制器
    /// </summary>
    public class NoticeController : Controller
    {

        //定义默认用户编号
        private string UserID = "";
        private CompanyDBEntities db = new CompanyDBEntities();

        //展示已读与未读公告列表
        public ActionResult Index()
        {
            string AppPath = Request.RawUrl.ToString(); //目前为了应付url获取到的端口为本地端口问题，后期服务器该用url
            ViewBag.Config = GetConfig.getConfig(Session["appId"].ToString(), AppPath);
            string is_sys = "Ture";
            if (Session["User"] != null || Session["is_sys"] != null)
            {
                UserID = Session["User"].ToString();
                is_sys = Session["is_sys"].ToString();
            }

            SqlParameter[] para = new SqlParameter[]{
                new SqlParameter("@id",UserID)
            };
            List<AnnoUser> result = db.Database.SqlQuery<AnnoUser>(" exec proc_SelNoread @id", para).ToList();
            return View(result);
        }
        //查询已读
        [HttpPost]
        public string Index1()
        {
            string is_sys = "Ture";
            if (Session["User"] != null || Session["is_sys"] != null)
            {
                UserID = Session["User"].ToString();
                is_sys = Session["is_sys"].ToString();
            }

            SqlParameter[] para = new SqlParameter[]{
                new SqlParameter("@id",UserID)
            };

            List<AnnoUser> result = db.Database.SqlQuery<AnnoUser>(" exec proc_Selread @id", para).ToList();

            return JsonConvert.SerializeObject(result); ;
        }
        //查询全部
        public string Index2()
        {
            string is_sys = "Ture";
            if (Session["User"] != null || Session["is_sys"] != null)
            {
                UserID = Session["User"].ToString();
                is_sys = Session["is_sys"].ToString();
            }
            List<AnnoUser> result = db.Database.SqlQuery<AnnoUser>("select * from Announcement s1 inner join userinfo s2 on s1.u_id=s2.u_id").ToList();

            return JsonConvert.SerializeObject(result);
        }

        //显示某一条公告的详细信息
        public ActionResult Details(int id, int i)
        {
            string is_sys = "Ture";
            if (Session["User"] != null || Session["is_sys"] != null)
            {
                UserID = Session["User"].ToString();
                is_sys = Session["is_sys"].ToString();
            }
            ViewData["is_sys"] = is_sys;
            IsRead r = null;
            try
            {
                r = (from ir in db.IsRead
                     where ir.U_ID == UserID && ir.A_ID == id
                     select ir).First();
            }
            catch (Exception)
            {

            }
            if (r == null)
            {
                bool s = db.Database.ExecuteSqlCommand("insert into IsRead values('" + UserID + "','" + id + "');") > 0 ? true : false;
            }

            IEnumerable<AnnoUser> result = from a in db.Announcement
                                           join u in db.Userinfo on a.U_ID equals u.U_ID
                                           where a.A_ID == id
                                           select new AnnoUser { Name = u.Name, A_Title = a.A_Title, A_Time = (DateTime)a.A_Time, A_Content = a.A_Content, A_Image = a.A_Image, A_ID = a.A_ID };
            Session["Aid"] = result.First().A_ID;
            Session["i"] = i;

            return View(result);
        }

        //上一页
        public string Previous()
        {
            string is_sys = "Ture";
            if (Session["User"] != null || Session["is_sys"] != null)
            {
                UserID = Session["User"].ToString();
                is_sys = Session["is_sys"].ToString();
            }
            string Aid = Session["Aid"].ToString();
            string id = "";
            int i = int.Parse(Session["i"].ToString());
            if (i == 1)
            {
                IEnumerable<IsRead> read = db.Database.SqlQuery<IsRead>("select top 1 * from IsRead where a_id < '" + Aid + "' and u_id='" + UserID + "' order by  a_id desc").ToList();
                try
                {
                    id = read.First().A_ID.ToString();
                }
                catch (Exception)
                {

                    id = Aid;
                }
            }
            else if (i == 2)
            {
                IEnumerable<Announcement> read = db.Database.SqlQuery<Announcement>("select top 1 * from Announcement where a_id < '" + Aid + "'order by  a_id desc").ToList();
                try
                {
                    id = read.First().A_ID.ToString();
                }
                catch (Exception)
                {

                    id = Aid;
                }
            }
            if (id == "") { id = Aid; };
            return id + "," + i.ToString();
        }
        //下一页
        public string Previous1()
        {
            string is_sys = "Ture";
            if (Session["User"] != null || Session["is_sys"] != null)
            {
                UserID = Session["User"].ToString();
                is_sys = Session["is_sys"].ToString();
            }
            string Aid = Session["Aid"].ToString();
            string id = "";
            int i = int.Parse(Session["i"].ToString());
            if (i == 1)
            {
                IEnumerable<IsRead> read = db.Database.SqlQuery<IsRead>("select top 1 * from IsRead where a_id > '" + Aid + "' and u_id='" + UserID + "' order by  a_id asc").ToList();
                try
                {
                    id = read.First().A_ID.ToString();
                }
                catch (Exception)
                {

                    id = Aid;
                }
            }
            else if (i == 2)
            {
                IEnumerable<Announcement> read = db.Database.SqlQuery<Announcement>("select top 1 * from Announcement where a_id > '" + Aid + "'order by  a_id asc").ToList();
                try
                {
                    id = read.First().A_ID.ToString();
                }
                catch (Exception)
                {

                    id = Aid;
                }
            }
            if (id == "") { id = Aid; };
            return id + "," + i.ToString();
        }

        //显示添加公告页面
        public ActionResult AddNotice()
        {
            return View();
        }

        //添加公告
        [HttpPost]
        public ActionResult AddAnnou(string title, string content)
        {
            string is_sys = "Ture";
            if (Session["User"] != null || Session["is_sys"] != null)
            {
                UserID = Session["User"].ToString();
                is_sys = Session["is_sys"].ToString();
            }
            bool falg = db.Database.ExecuteSqlCommand("insert into Announcement values('" + UserID + "',default,'','" + title + "','" + content + "')") > 0 ? true : false;
            Messages.OAMessage o = new Messages.OAMessage();
            o.touser = "@all";
            o.agentid = "24664016";
            o.oa.body.author = (from u in db.Userinfo
                                where u.U_ID == UserID
                                select u).First().Name;
            o.oa.body.title = title;
            o.oa.head.bgcolor = "FF7ee114";
            o.oa.body.content = content;
            o.oa.message_url = Config.WebUrl + "/withoutlogin/login?agentid=24664016&device=Mobile";
            o.oa.pc_message_url = Config.WebUrl + "/withoutlogin/login?agentid=24664016&device=PC";
            Messages.MessageCallBack MessageCallBack = EnterpriseBusiness.SendMessage(o);
            Helper.WriteLog("公告消息ID" + MessageCallBack.messageId);
            return Json(falg);
        }
        //删除公告
        public ActionResult Delete()
        {
            db.Database.ExecuteSqlCommand("delete from isread where A_ID='" + Session["Aid"] + "' ");
            db.Database.ExecuteSqlCommand("delete from Announcement where A_ID='" + Session["Aid"] + "' ");
            return Json("ok");
        }

    }
}
