using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace PixivRankBot
{
    class Monitor
    {
        CQ cq = new CQ();
        /// <summary>
        /// 初始化监听器
        /// </summary>
        public async void MonitorInit()
        {
            Console.WriteLine("MonitorInit()");


            string Path = cq.RunTimePath();
            string post_url;
            if (Path == null)
            {
                Console.WriteLine("没有启动酷Q!请启动!");
                return;
            }
            else
            {
                post_url = cq.GetHttpApiConfig(Path, Key: "post_url");
            }

            if (post_url == null)
            {
                Console.WriteLine("请设置HttpApi的Post_Url，配置文件位于:CQ根目录\\app\\io.github.richardchien.coolqhttpapi\\config\\登陆的QQ号.json");
                return;
            }
            else
            {
                await Task.Run(() => {
                    MonitorPost(post_url);
                });
                
            }
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

            //接收POST参数
            Stream stream = ctx.Request.InputStream;
            StreamReader reader = new System.IO.StreamReader(stream, Encoding.UTF8);
            var json = reader.ReadToEnd();
            ParsePost(json,true);
        }

        /// <summary>
        /// 读取消息Json
        /// </summary>
        /// <param name="json">传入json</param>
        /// <param name="key">Json中的Key</param>
        /// <returns>key对应的值</returns>
        string ParsePost(string Json,bool Write, string key="")
        { 
            JObject jObject = JObject.Parse(Json);

            if (Write)
            {
                switch (jObject["post_type"].ToString())
                {
                    case "message":
                        WriteMessage(jObject);
                        break;
                }
                
            }

            if (key == "")
            {
                return null;
            }
            return (string)jObject[key];
        }


        void WriteMessage(JObject jObject)
        {
            var DefForegroundColor = Console.ForegroundColor;
            var nickname  = cq.GetUserInfo((int)jObject["user_id"])["data"]["nickname"];

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("[");
            Console.Write(jObject["message_type"]);
            if (jObject["sub_type"] != null)//子类型
            {
                Console.Write("(");
                Console.Write(jObject["sub_type"]);
                Console.Write(")");
            }
            Console.Write("]");//[message_type(sub_type)]

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(nickname);
            Console.Write("<");
            Console.Write(jObject["user_id"]);
            Console.Write(">");//昵称<QQ号>

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("=>");
            Console.Write(jObject["message"]);
            Console.Write("\r\n");
            //[message_type(sub_type)]昵称<QQ号>=>消息

            Console.ForegroundColor = DefForegroundColor;


        }

        public List<int> GetGroupAdmin()
        {
            return null;
        }
    }
}
