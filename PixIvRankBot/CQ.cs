using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing;
using Colorful;
using Console = Colorful.Console;
using System.Collections.Generic;
using System.Linq;

namespace PixivRankBot
{
    class CQ
    {
        public string HostPost
        {
            get
            {
                if (Host == "0.0.0.0" || Host == "127.0.0.1")
                {
                    string host = "127.0.0.1";
                    return string.Format("{0}{1}{2}{3}{4}", "http://", host, ":", Port, "/");
                }
                else
                {
                    return string.Format("{0}{1}{2}{3}{4}", "http://", Host, ":", Port, "/");
                }

            }
        }

        public int Port
        {
            get => int.Parse(GetHttpApiConfig(RunTimePath(), Key: "port"));

            set => SetHttpApiConfig(RunTimePath(), Key: "port", Value: value.ToString());
        }

        public string Host
        {
            get => GetHttpApiConfig(RunTimePath(), Key: "host");
            set => SetHttpApiConfig(RunTimePath(), Key: "host", Value: value);
        }

        public string PostUrl
        {
            get => GetHttpApiConfig(RunTimePath(), Key: "post_url");
            set => SetHttpApiConfig(RunTimePath(), Key: "post_url", Value: value);
        }

        /// <summary>
        /// 酷Q登陆QQ的信息,获取速度较慢
        /// </summary>
        public JObject LogInInfo
        {
            get => LogInInfo;
            set => LogInInfo = GetLogInInfo();
        }

        /// <summary>
        /// 酷Q Config目录的第一个配置文件QQ号,可能获取到的不是登陆的QQ
        /// </summary>
        public int PathUserId
        {
            get => PathUserId;
            set
            {
                string Path = RunTimePath();
                Path = string.Format("{0}{1}", Path, "app\\io.github.richardchien.coolqhttpapi\\config\\");
                DirectoryInfo directoryInfo = new DirectoryInfo(Path);
                FileInfo[] files = directoryInfo.GetFiles();
                if (files != null)
                {
                    PathUserId = int.Parse(files[0].Name);
                }
            }
        }

        /// <summary>
        /// Http 发送消息
        /// </summary>
        /// <param name="MessageType">消息类型，支持 private、group、discuss，分别对应私聊、群组、讨论组</param>
        /// <param name="Id">对方ID，private为qq号，group为群号，discuss讨论组号</param>
        /// <param name="Message">要发送的内容</param>
        /// <param name="auto_escape">消息内容是否作为纯文本发送（即不解析 CQ 码），只在 message 字段是字符串时有效</param>
        /// <param name="get">发送的方法，true为get，false为post，post默认发送表单</param>//可能用json(大概率不会写json序列化| ω・´)
        /// <returns></returns>
        public string HttpSendMsg(string MessageType, int Id = -1, string Message = "", bool AutoEscape = false, bool get = true)
        {
            Console.WriteLine("SendMsg()",Color.Yellow);

            string ret = string.Empty;
            string IdType = string.Empty;
            if (Id != -1)
            {
                switch (MessageType)
                {
                    case "private":
                        IdType = "user_id";
                        break;

                    case "group":
                        IdType = "group_id";
                        break;

                    case "discuss":
                        IdType = "discuss_id";
                        break;
                }
            }
            else
            {
                ret = "pls put Id";
            }

            if (get)
            {
                if (!AutoEscape)
                {
                    Message = System.Web.HttpUtility.UrlEncode(Message, Encoding.UTF8);
                }
                string url;
                if (Host == "0.0.0.0" && Host == "127.0.0.1")
                {
                    url = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}", HostPost, "send_msg", "?", "message_type=", MessageType, "&", IdType, "=", Id, "&", "message", "=", Message, "&", "auto_escape", "=", AutoEscape.ToString());
                }
                else
                {
                    url = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}", HostPost, "send_msg", "?", "message_type=", MessageType, "&", IdType, "=", Id, "&", "message", "=", Message, "&", "auto_escape", "=", AutoEscape.ToString());
                }

                Uri uri = new Uri(url);
                WebClient wc = new WebClient();

                ret = wc.DownloadString(uri);
            }
            else
            {
                try
                {
                    JObject postObj = new JObject();
                    JArray jArray = new JArray();
                    postObj["message_type"] = MessageType;
                    postObj[IdType] = Id;
                    postObj["message"] = Message;

                    //TODO
                    //重写发送消息格式 彻底转换成数组表示消息的json
                    //JObject a1 = new JObject();
                    //a1["type"] = "image";
                    //JObject a11 = new JObject();
                    //a11["file"] = "1.jpg";
                    //a1["data"] = a11;

                    //JObject a2 = new JObject();
                    //a2["type"] = "text";
                    //JObject a22 = new JObject();
                    //a22["text"] = "\r\n";
                    //a2["data"] = a22;

                    //JObject a3 = new JObject();
                    //a3["type"] = "image";
                    //JObject a33 = new JObject();
                    //a33["file"] = "2.jpg";
                    //a3["data"] = a33;

                    //jArray.Add(a1);
                    //jArray.Add(a2);
                    //jArray.Add(a3);
                    //postObj["message"] = jArray;

                    //postObj["auto_escape"] = AutoEscape.ToString();

                    //var a = postObj.ToString();
                    //;
                    //string postData = "{ \"message_type\": \"" + MessageType + "\",\"" + IdType + "\": " + Id + ",\"message\":\"" + Message + "\",\"auto_escape\":\"" + AutoEscape.ToString() + "\"}";
                    byte[] data = Encoding.UTF8.GetBytes(postObj.ToString());
                    string api = string.Format("{0}{1}", HostPost, "send_msg");
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(api);
                    request.Method = "POST";
                    request.ContentType = "application/json; charset=UTF-8";
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";
                    request.ContentLength = data.Length;
                    Stream newStream = request.GetRequestStream();
                    newStream.Write(data, 0, data.Length);
                    newStream.Close();
                    HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
                    StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                    string content = reader.ReadToEnd();
                    Console.WriteLine("Seed:\r\n" + postObj,Color.Green);
                    Console.WriteLine("Return:\r\n" + content, Color.Green);
                }
                catch (WebException ex)
                {
                    Console.WriteLine(ex.ToString(),Color.Red);
                    throw;
                }


            }


            return ret;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="UserId">指定的QQ号</param>
        /// <param name="Cache">是否使用缓存，默认true</param>
        /// <returns>返回 QQ 号，昵称，性别，年龄，具体参考https://cqhttp.cc/docs/4.5/#/API?id=get_stranger_info-获取陌生人信息</returns>
        public JObject GetUserInfo(int UserId, bool Cache = true)
        {
            WebClient wc = new WebClient();
            string url = string.Format("{0}{1}{2}{3}{4}{5}", HostPost, "get_stranger_info?user_id=", UserId, "&", "no_cache=", Cache.ToString());
            Uri uri = new Uri(url);
            JObject jObject = new JObject();
            try
            {
                return JObject.Parse(wc.DownloadString(uri));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString(), Color.Red);
                return null;
            }
        }

        /// <summary>
        /// 获取酷Q登陆Q号的信息
        /// </summary>
        /// <returns></returns>
        public JObject GetLogInInfo()
        {
            WebClient wc = new WebClient();
            string url = string.Format("{0}{1}", HostPost, "/get_login_info");
            Uri uri = new Uri(url);
            JObject jObject = new JObject();
            try
            {
                return JObject.Parse(wc.DownloadString(uri));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString(), Color.Red);
                return null;
            }
        }

        /// <summary>
        /// 获取群管理员
        /// </summary>
        /// <param name="GroupId">群ID</param>
        /// <returns>List<int>,管理员的QQ号</returns>
        public List<int> GetGroupAdmin(int GroupId)
        {
            WebClient wc = new WebClient();
            string GroupListUrl = string.Format("{0}{1}{2}", HostPost, "get_group_member_list?group_id=",GroupId);
            Uri uri = new Uri(GroupListUrl);
            JObject jObject = new JObject();
            var AdminList = new List<int>();
            try
            {
                var GroupList = new List<int>();
                var GroupUserJObj =JObject.Parse(wc.DownloadString(uri));
                for (int i = 0; i < GroupUserJObj["data"].Count(); i++)
                {
                    if ((string)GroupUserJObj["data"][i]["role"] == "admin")
                    {
                        AdminList.Add((int)GroupUserJObj["data"][i]["user_id"]);
                    }
                }
                return AdminList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString(), Color.Red);
                return null;
            }
        }

        /// <summary>
        /// 依据酷Q进程寻找酷Q根目录
        /// </summary>
        /// <returns>酷Q根目录</returns>
        public string RunTimePath()
        {
            var app = Process.GetProcesses();
            Process item = null;
            for (int i = 0; i < app.Length; i++)
            {
                item = app[i];
                if (item.MainWindowTitle.IndexOf("酷Q") != -1 && item.MainModule.FileVersionInfo.Comments.IndexOf("酷Q") != -1)
                {
                    return item.MainModule.FileName.Replace(item.MainModule.ModuleName, "");
                }
            }
            return null;
        }

        public string GetHttpApiConfig(string Path, string FileName = "", string Key = "port")
        {

            Path = string.Format("{0}{1}", Path, "app\\io.github.richardchien.coolqhttpapi\\config\\");

            if (FileName == "")
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(Path);
                FileInfo[] files = directoryInfo.GetFiles();
                Path = Path + files[0].Name;
            }
            else
            {
                Path = Path + FileName;
            }

            if (File.Exists(Path))//找cq配置文件
            {
                StreamReader file = File.OpenText(Path);
                JObject jObject = JObject.Parse(file.ReadToEnd());
                file.Close();
                return (string)jObject[Key];

            }
            return null;//此处应用户输入正确的path
        }

        /// <summary>
        /// 设置CQHttpApi配置文件
        /// </summary>
        /// <param name="Path">Cq根目录</param>
        /// <param name="FileName">文件名，默认为目录下第一个文件</param>
        /// <param name="Key">修改的Key</param>
        /// <param name="Value">修改的值</param>
        /// <returns></returns>
        public string SetHttpApiConfig(string Path, string FileName = "", string Key = "port", string Value = "")
        {

            Path = string.Format("{0}{1}", Path, "app\\io.github.richardchien.coolqhttpapi\\config\\");

            if (FileName == "")
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(Path);
                FileInfo[] files = directoryInfo.GetFiles();
                Path = Path + files[0].Name;
            }
            else
            {
                Path = Path + FileName;
            }
            string OutStr = string.Empty;
            if (File.Exists(Path))//找cq配置文件
            {
                StreamReader file = File.OpenText(Path);
                JObject jObject = JObject.Parse(file.ReadToEnd());
                file.Close();//先关闭不然 File.WriteAllText(Path, OutStr); 会报文件被占用
                jObject[Key] = Value;
                OutStr = JsonConvert.SerializeObject(jObject, Formatting.Indented);
                File.WriteAllText(Path, OutStr);
                return (string)jObject[Key];

            }
            return null;//此处应用户输入正确的path
        }


        //public string Help()
        //{
        //    return "0.Path=>酷Q的根目录下 data\\image,自动获取酷Q运行目录" + "\r\n" +
        //          "1.Type=>消息类型，private 为 qq 用户，group 为群，discuss 讨论组" + "\r\n" +
        //          "2.ID=>发送对象的ID，private为 qq 号，group为群号，discuss 讨论组号" + "\r\n" +
        //          "3.RankType=>排行榜类型，参考 https://docs.rsshub.app/#pixiv" + "\r\n" +
        //          "4.Total=>总数" + "\r\n" +
        //          "5.ApiType=>ApiV1,ApiV2,HtmlApi";
        //}

        
    }
}
