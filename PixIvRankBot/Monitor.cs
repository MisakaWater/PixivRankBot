using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Colorful;
using System.Drawing;
using Console = Colorful.Console;



namespace PixivRankBot
{
    class Monitor
    {
        CQ cq = new CQ();
        Helper helper = new Helper();
        //Cmd cmd = new Cmd();

        /// <summary>
        /// 初始化监听器
        /// </summary>
        public async void MonitorInit()
        {
            Colorful.Console.WriteLine("MonitorInit()", System.Drawing.Color.Yellow);
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

            if (post_url == "")
            {
                Console.WriteLine("请设置HttpApi的Post_Url，配置文件位于:CQ根目录\\app\\io.github.richardchien.coolqhttpapi\\config\\登陆的QQ号.json");
                return;
            }
            else
            {
                await Task.Run(() =>
                {
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
            Colorful.Console.WriteLine("MonitorPost()",System.Drawing.Color.Yellow);
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
            var a =ParsePostMsg(json);
            
            var b = a.IndexOf("post_type");
            Cmd.ReadSelect(ParsePostMsg(json));
            ParsePostMsg(json,true);
        }



        //void ReadSelect(List<string> Read)
        //{
        //    ///ret[0] = ["font"]);
        //    ///ret[1] = ["message"]);
        //    ///ret[2] = ["message_id"]);
        //    ///ret[3] = ["message_type"]);
        //    ///ret[4] = ["post_type"]);
        //    ///ret[5] = ["raw_message"]);
        //    ///ret[6] = ["self_id"]);
        //    ///ret[7] = ["sub_type"]);
        //    ///ret[8] = ["time"]);
        //    ///ret[9] = ["user_id"]);

        //    if (Read != null){

        //        if (Read[Read.IndexOf("message_type")+1]== "private") {//私聊
        //            switch (Read[Read.IndexOf("message") + 1])
        //            {
        //                case "help":
        //                    cq.HttpSendMsg(Read[Read.IndexOf("message_type") + 1], int.Parse(Read[Read.IndexOf("user_id") + 1]), helper.CQSetHelp(), get: false);
        //                    break;
        //            }

        //            if (Read[Read.IndexOf("message") + 1].Contains("type=>"))
        //            {
        //                cmd.Info[0]=Read[Read.IndexOf("message") + 1].Replace("type=>","");
        //                cq.HttpSendMsg(Read[Read.IndexOf("message_type") + 1], int.Parse(Read[Read.IndexOf("user_id") + 1]), "Now Type=>"+ cmd.Info[0], get: false);
        //            }

                    
        //        }
        //        if (Read[Read.IndexOf("message_type") + 1] == "group")//群聊
        //        {
        //            cq.HttpSendMsg(Read[Read.IndexOf("message_type") + 1], int.Parse(Read[Read.IndexOf("group_id") + 1]), helper.CQSetHelp(), get: false);
        //        }






        //        switch (Read[1].ToLower())
        //    {
        //        case "help":

        //            break;
        //    }
        //    }
        //}


        /// <summary>
        /// 读取消息Json
        /// </summary>
        /// <param name="Json">传入json</param>
        /// <param name="Write">是否输出到控制台</param>
        /// <param name="key">Json中的Key</param>
        /// <returns>key对应的值</returns>
        string ParsePostMsg(string Json,bool Write, string key="")
        {
            JObject jObject;
            try
            {
                jObject = JObject.Parse(Json);
            }catch(Exception ex)
            {
                
                Console.WriteLine(ex,Color.Red);
                return null;
            }


            if (Write)
            {
                switch (jObject["post_type"].ToString())
                {
                    case "message":
                        WriteMessage(jObject.ToString());
                        break;
                }
                
            }

            if (key == "")
            {
                return null;
            }
            return (string)jObject[key];
        }

        /// <summary>
        /// 读取消息Json
        /// </summary>
        /// <param name="Json">传入json</param>
        /// <param name="Write">是否输出到控制台</param>
        /// <param name="key">Json中的Key</param>
        /// <returns>返回所有信息
        ///ret[0] = ["font"]);
        ///ret[1] = ["message"]);
        ///ret[2] = ["message_id"]);
        ///ret[3] = ["message_type"]);
        ///ret[4] = ["post_type"]);
        ///ret[5] = ["raw_message"]);
        ///ret[6] = ["self_id"]);
        ///ret[7] = ["sub_type"]);
        ///ret[8] = ["time"]);
        ///ret[9] = ["user_id"]);
        /// </returns>
        List<string> ParsePostMsg(string Json)
        {
            var ret = new List<string>();
            JObject jObject;
            try
            {
                jObject = JObject.Parse(Json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex, Color.Red);
                return null;
            }
            if ((string)jObject["post_type"]== "message") {


                foreach (var item in jObject)
                {
                    ret.Add(item.Key);
                    ret.Add((string)item.Value);
                }







                //switch ((string)jObject["message_type"])
                //{
                //    case "private":
                //        ret.Add("font");
                //        ret.Add((string)jObject["font"]);
                //        ret.Add("message");
                //        ret.Add((string)jObject["message"]);
                //        ret.Add("message_id");
                //        ret.Add((string)jObject["message_id"]);
                //        ret.Add("message_id");
                //        ret.Add((string)jObject["message_type"]);
                //        ret.Add("post_type");
                //        ret.Add((string)jObject["post_type"]);
                //        ret.Add("raw_message");
                //        ret.Add((string)jObject["raw_message"]);
                //        ret.Add("self_id");
                //        ret.Add((string)jObject["self_id"]);
                //        ret.Add("sub_type");
                //        ret.Add((string)jObject["sub_type"]);
                //        ret.Add("time");
                //        ret.Add((string)jObject["time"]);
                //        ret.Add("user_id");
                //        ret.Add((string)jObject["user_id"]);
                //        break;

                //    case "group":
                //        ret.Add("anonymous");
                //        ret.Add((string)jObject["anonymous"]);
                //        ret.Add("font");
                //        ret.Add((string)jObject["font"]);
                //        ret.Add("group_id");
                //        ret.Add((string)jObject["group_id"]);
                //        ret.Add("message");
                //        ret.Add((string)jObject["message"]);
                //        ret.Add("message_id");
                //        ret.Add((string)jObject["message_id"]);
                //        ret.Add("message_id");
                //        ret.Add((string)jObject["message_type"]);
                //        ret.Add("post_type");
                //        ret.Add((string)jObject["post_type"]);
                //        ret.Add("raw_message");
                //        ret.Add((string)jObject["raw_message"]);
                //        ret.Add("self_id");
                //        ret.Add((string)jObject["self_id"]);
                //        ret.Add("sub_type");
                //        ret.Add((string)jObject["sub_type"]);
                //        ret.Add("time");
                //        ret.Add((string)jObject["time"]);
                //        ret.Add("user_id");
                //        ret.Add((string)jObject["user_id"]);
                //        break;

                //}
                
                return ret;
            }
            else
            {
                ret.Clear();
                ret.Add("!Msg");
                return ret;
            }
        }




            void WriteMessage(string Json)
            {
            JObject jObject = JObject.Parse(Json);

            var DefForegroundColor = Console.ForegroundColor;
            string nickname=string.Empty;
            try
            {
                nickname = cq.GetUserInfo((int)jObject["user_id"])["data"]["nickname"].ToString();
            }catch(NullReferenceException ex)
            {
                Colorful.Console.WriteLine(ex.ToString(),System.Drawing.Color.Red);
            }
            

            Console.ForegroundColor = Color.Blue;
            Console.Write("[");
            Console.Write(jObject["message_type"]);
            if (jObject["sub_type"] != null)//子类型
            {
                Console.Write("(");
                Console.Write(jObject["sub_type"]);
                Console.Write(")");
            }
            Console.Write("]");//[message_type(sub_type)]

            Console.ForegroundColor = Color.Red;
            Console.Write(nickname);
            Console.Write("<");
            Console.Write(jObject["user_id"]);
            Console.Write(">");//昵称<QQ号>

            Console.ForegroundColor = Color.Yellow;
            Console.Write("=>");
            Console.Write(jObject["message"]);
            Console.Write("\r\n");
            //[message_type(sub_type)]昵称<QQ号>=>消息

            Console.ForegroundColor = DefForegroundColor;


        }


    }
}
