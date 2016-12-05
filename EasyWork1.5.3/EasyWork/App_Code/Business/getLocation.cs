using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

public class getLocation
{
    public static string GetCurrentLocation(string longitude, string latitude)
    {
        string tagUrl = "http://restapi.amap.com/v3/geocode/regeo?output=xml&location=" + longitude + "," + latitude + "&key=c6cd6d0075637f20dd6c465d630e228e&radius=1000";
        string result = HttpHelper.Get(tagUrl);
        return getlocation(result);
    }
    private static string getlocation(string xml)
    {
        string loc = null;
        XmlTextReader reader = new XmlTextReader(xml, XmlNodeType.Document, null);
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element)
            {
                if (reader.Name == "formatted_address")
                {
                    //如果节点为MSGID，继续读下一个节点，即读取MSGID节中电话号码  
                    reader.Read();
                    //如果节点类型是节点的文本内容  
                    if (XmlNodeType.Text == reader.NodeType)
                    {
                        //读取电话号码  
                        loc += reader.Value;
                    }
                }

            }
        }
        return loc;

    }
}
