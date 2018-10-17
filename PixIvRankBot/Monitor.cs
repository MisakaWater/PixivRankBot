using System;
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
        /// <summary>
        /// 初始化监听器
        /// </summary>
        public async void MonitorInit()
        {
            Console.WriteLine("MonitorInit()");
            CQ cq = new CQ();

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
            string body = reader.ReadToEnd();
        }

        /// <summary>
        /// 读取消息Json
        /// </summary>
        /// <param name="json">传入json</param>
        /// <param name="key">Json中的Key</param>
        /// <returns>key对应的值</returns>
        string GetMsgType(string key,string Json)
        {
            JObject jObject = JObject.Parse(Json);
            return (string)jObject[key];
        }
    }
}
