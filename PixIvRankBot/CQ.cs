using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            Console.WriteLine("SendMsg()");

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
                    Message = System.Web.HttpUtility.UrlEncode(Message, System.Text.Encoding.UTF8);
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
                    postObj["message_type"] = MessageType;
                    postObj[IdType] = Id;
                    postObj["message"] = Message;
                    postObj["auto_escape"]= AutoEscape.ToString()


                    string postData = "{ \"message_type\": \"" + MessageType + "\",\"" + IdType + "\": " + Id + ",\"message\":\"" + Message + "\",\"auto_escape\":\"" + AutoEscape.ToString() + "\"}";
                    byte[] data = Encoding.UTF8.GetBytes(postData);
                    string api = string.Format("{0}{1}",HostPost,"/send_msg");
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
                    Console.WriteLine(content);
                    Console.WriteLine(postData);
                }
                catch (WebException ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }


            }


            return ret;
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
                    return item.MainModule.FileName.Replace(item.MainModule.ModuleName,"");
                }
            }
            return null;
        }

        public string GetHttpApiConfig(string Path, string FileName="", string Key="port")
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
        public string SetHttpApiConfig(string Path, string FileName = "", string Key = "port",string Value="")
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

    }
}
