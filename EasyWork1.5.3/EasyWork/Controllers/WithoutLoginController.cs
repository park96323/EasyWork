using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyWork.Controllers
{
    /// <summary>
    /// 免登控制器，主要负责权限验证配置以及用户信息的获取
    /// </summary>
    public class WithoutLoginController : Controller
    {
        /// <summary>
        /// 移动端免登及权限验证配置信息
        /// </summary>
        /// <param name="AgentID">应用ID</param>
        /// <returns></returns>
        public ActionResult Login(string AgentID, string device)
        {
            //将当前应用的ID保存到Session中，方便其他页面调用
            Session["appId"] = AgentID;
            Session["Device"] = device;
            string AppPath = Request.RawUrl.ToString(); //目前为了应付url获取到的端口为本地端口问题，后期服务器该用url
            Helper.WriteLog("URL:" + AppPath);
            ViewBag.Config = GetConfig.getConfig(Session["appId"].ToString(), AppPath);
            return View();
        }
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <param name="Code">免登授权码</param>
        /// <returns></returns>
        [HttpPost]
        public void GetUserInfo(string Code)
        {
            UserModel user = WithoutLoginService.GetCurrentUser(Code);
            Session.Timeout = 9999;
            Session["is_sys"] = user.is_sys;
            Session["User"] = user.userid;
            Helper.WriteLog("User:" + user.userid);
        }

    }
}
