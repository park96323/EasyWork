using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

/// <summary>
/// 获取dd.config参数
/// </summary>
public class GetConfig
{
    private static string corpId { get { return Config.ECorpId; } }
    private static string corpSecret { get { return Config.ECorpSecret; } }
    private static string agentId { get { return Config.EAgentID; } }
    public static string webUrl { get { return Config.WebUrl; } }
    /// <summary>
    /// 生成签名代码
    /// </summary>
    /// <param name="nonceStr">八位的随机号，用于签名</param>
    /// <param name="timeStamp">时间戳的随机数</param>
    /// <param name="appUrl">当前应用的地址</param>
    /// <returns>返回签名</returns>
    private static string getSignature(string nonceStr, string timeStamp, string appUrl)
    {
        ////获取AccessToken
        //string accessToken = EnterpriseBusiness.GetToken(corpId, corpSecret).access_token;
        //获取jsapi_ticket
        string accessToken = System.Web.HttpContext.Current.Application["Corp_AccessToken"].ToString();
        string jsApiTicket = EnterpriseBusiness.GetTickets(accessToken);
        Helper.WriteLog("nonceStr：" + nonceStr);
        Helper.WriteLog("timestamp:" + timeStamp);
        Helper.WriteLog("AppUrl:" + appUrl);
        Helper.WriteLog("accessToken:" + accessToken);
        Helper.WriteLog("jsApiTicket:" + jsApiTicket);
        //生成签名
        string str = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", jsApiTicket, nonceStr, timeStamp, appUrl);
        //Helper.WriteLog("signature not sha1:" + str);
        string signature = DES.SHA1Encrypt(str);
        Helper.WriteLog("signature sha1:" + signature);
        return signature;
    }
    /// <summary>
    /// 获取配置文件
    /// </summary>
    /// <returns></returns>
    public static Dictionary<string, string> getConfig(string appId, string path)
    {
        //生成时间戳和八位随机数
        string timeStamp = Helper.timeStamp();
        string nonceStr = Helper.randNonce();

        Dictionary<string, string> json = new Dictionary<string, string>() {
            {"appId",appId},
            {"corpId",corpId},
            {"timeStamp",timeStamp},
            {"nonceStr",nonceStr},
            {"signature",getSignature(nonceStr,timeStamp,webUrl + path)}
            };
        return json;
    }
}
