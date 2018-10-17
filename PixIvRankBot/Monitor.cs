using Newtonsoft.Json.Linq;
using PixivRankBot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;

namespace PixIvRankBot
{
    class Monitor
    {
        /// <summary>
        /// 初始化监听器
        /// </summary>
        void MonitorInit()
        {
            Console.WriteLine("MonitorInit()");
            CQ cq = new CQ();
            string post_url = cq.GetHttpApiConfig(cq.RunTimePath(), Key: "post_url");
            MonitorPost(post_url);
        }
        /// <summary>
        /// 启动监视器
        /// </summary>
        /// <param name="Url">监听的Url地址</param>
        void MonitorPost(string Url)
        {
            Console.WriteLine("MonitorPost()");
            HttpListener listerner = new HttpListener();
            while (true)
            {
                try
                {
                    listerner.AuthenticationSchemes = AuthenticationSchemes.Anonymous;//指定身份验证 Anonymous匿名访问
                    listerner.Prefixes.Add(Url);
                    listerner.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    break;
                }
                while (true)
                {
                    //等待请求连接
                    //没有请求则GetContext处于阻塞状态
                    HttpListenerContext ctx = listerner.GetContext();
                    ThreadPool.QueueUserWorkItem(new WaitCallback(TaskProc), ctx);
                }
            }
        }

        void TaskProc(object o)
        {
            HttpListenerContext ctx = (HttpListenerContext)o;

            ctx.Response.StatusCode = 200;//设置返回给客服端http状态代码

            //接收Get参数
            string type = ctx.Request.QueryString["type"];
            string userId = ctx.Request.QueryString["userId"];
            string password = ctx.Request.QueryString["password"];
            string filename = Path.GetFileName(ctx.Request.RawUrl);
            string userName = HttpUtility.ParseQueryString(filename).Get("userName");//避免中文乱码
            //进行处理
            Console.WriteLine("收到数据:" + userName);

            //接收POST参数
            Stream stream = ctx.Request.InputStream;
            System.IO.StreamReader reader = new System.IO.StreamReader(stream, Encoding.UTF8);
            String body = reader.ReadToEnd();
            Console.WriteLine("收到POST数据:" + HttpUtility.UrlDecode(body));
            Console.WriteLine("解析:" + HttpUtility.ParseQueryString(body).Get("userName"));

        }

        //https://cqhttp.cc/docs/4.5/#/Post?id=上报和回复中的数据类型

        /// <summary>
        /// 获取消息类型
        /// </summary>
        /// <param name="json">传入json</param>
        /// <returns>消息类型 </returns>
        string GetMsgType(string json)
        {
            JObject jObject = JObject.Parse(json);
            return (string)jObject["message_type"];
        }
    }
}
