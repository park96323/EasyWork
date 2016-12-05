using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyWork.Models
{
    public class Messages
    {
        public class OAMessage
        {
            public OAMessage()
            {
                oa = new Messages.oa()
                {
                    body = new Messages.body()
                    {
                        rich = new Messages.rich(),
                        form=new List<FormItem>()
                    },
                    head = new Messages.head()
                };
            }
            public string touser { get; set; }
            public string toparty { get; set; }
            public string agentid { get; set; }
            /// <summary>
            /// 消息类型，此时固定为：oa
            /// </summary>
            public string msgtype { get { return "oa"; } }
            /// <summary>
            /// OA消息体内容
            /// </summary>
            public oa oa { get; set; }
        }
        public class oa
        {
            /// <summary>
            /// 客户端点击消息时跳转到的H5地址
            /// </summary>
            public string message_url { get; set; }
            /// <summary>
            /// PC端点击消息时跳转到的H5地址
            /// </summary>
            public string pc_message_url { get; set; }
            /// <summary>
            /// 消息头部内容
            /// </summary>
            public head head { get; set; }
            /// <summary>
            /// 消息体
            /// </summary>
            public body body { get; set; }

        }
        public class head
        {
            /// <summary>
            /// 消息头部的背景颜色。长度限制为8个英文字符，其中前2为表示透明度，后6位表示颜色值。不要添加0x
            /// </summary>
            public string bgcolor { get; set; }
            /// <summary>
            /// 消息的头部标题（仅适用于发送普通场景）
            /// </summary>
            public string text { get; set; }
        }
        public class body
        {
            /// <summary>
            /// 消息体的标题
            /// </summary>
            public string title { get; set; }
            /// <summary>
            /// 消息体的表单，最多显示6个，超过会被隐藏<key,value>
            /// </summary>
            public List<FormItem> form { get; set; }
            /// <summary>
            /// 单行富文本信息
            /// </summary>
            public rich rich { get; set; }
            /// <summary>
            /// 	消息体的内容，最多显示3行
            /// </summary>
            public string content { get; set; }
            /// <summary>
            /// 消息体中的图片media_id
            /// </summary>
            public string image { get; set; }
            /// <summary>
            /// 自定义的附件数目。此数字仅供显示，钉钉不作验证
            /// </summary>
            public string file_count { get; set; }
            /// <summary>
            /// 自定义的作者名字
            /// </summary>
            public string author { get; set; }
        }
        public class rich
        {
            /// <summary>
            /// 单行富文本信息的数目
            /// </summary>
            public string num { get; set; }
            /// <summary>
            /// 单行富文本信息的单位
            /// </summary>
            public string unit { get; set; }
        }
        public  class FormItem
        {
            public string key { get; set; }
            public string value { get; set; }
        }
        public class MessageCallBack
        {
            public string errcode { get; set; }
            public string errmsg { get; set; }
            public string invaliduser { get; set; }
            public string invalidparty { get; set; }
            public string messageId { get; set; }
        }
    }
}