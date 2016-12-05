using EasyWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace EasyWork.Controllers
{

    /// <summary>
    /// 签到微应用
    /// </summary>
    public class SignController : Controller
    {

        //定义默认用户编号
        private string UserID = "0443103313939774"; //王天
        private CompanyDBEntities db = new CompanyDBEntities();

        //签到首页
        public ActionResult Index()
        {
            string AppPath = Request.RawUrl.ToString(); //目前为了应付url获取到的端口为本地端口问题，后期服务器该用url
            Helper.WriteLog("URL:" + AppPath);
            //获取用户信息
            string UserID = Session["User"].ToString();
          ViewBag.U_Name= (from u in db.Userinfo
                          where u.U_ID == UserID
                          select u).First().Name;
            DateTime time1 = DateTime.Parse(DateTime.Now.ToShortDateString());
            DateTime time2 = DateTime.Parse(DateTime.Now.AddDays(1).ToShortDateString());
            ViewBag.SignCount = (from u in db.Registration
                                 where u.U_ID == UserID && u.R_Time >= time1 && u.R_Time < time2
                                 select u).Count();
            //获取权限配置
            ViewBag.Config = GetConfig.getConfig(Session["appId"].ToString(), AppPath);
            return View();
        }

        public ActionResult Content(string site)
        {
            ViewData["site"] = site;
            return View();
        }
        //签到内容页
        public ActionResult Content1(string time, string site, string remark)
        {
            string is_sys = "Ture";
            if (Session["User"] != null || Session["is_sys"] != null)
            {
                UserID = Session["User"].ToString();
                is_sys = Session["is_sys"].ToString();
            }
            bool b = db.Database.ExecuteSqlCommand(" insert into Registration values('" + UserID + "','" + site + "','" + time + "','" + remark + "')") > 0 ? true : false;

            if (b)
            {
                return this.Json("ok");
            }
            else
            {
                return this.Json("no");
            }
        }


        [HttpPost]
        public string GetCurrentLocation(string longitude, string latitude)
        {
            return getLocation.GetCurrentLocation(longitude, latitude);
        }

    }
}
