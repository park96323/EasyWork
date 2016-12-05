using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

public class WithoutLoginService
{

    private static string corpId { get { return Config.ECorpId; } }
    private static string corpSecret { get { return Config.ECorpSecret; } }
    /// <summary>
    /// 获取当前用户
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public static UserModel GetCurrentUser(string code)
    {
        //获取AccessToken
        string accessToken = EnterpriseBusiness.GetToken(corpId, corpSecret).access_token;
        UserModel user = EnterpriseBusiness.GetCurrentUser(accessToken, code);

        return user;
    }
}
