using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace PixivRankBot
{
    class test
    {
        public string HttpApiConfig(string Path, string FileName, string Key)
        {

            Path = string.Format("{0}{1}{2}", Path, "app\\io.github.richardchien.coolqhttpapi\\config\\", FileName);
            if (Directory.Exists(Path))//找cq配置文件目录
            {
                StreamReader file = File.OpenText(Path);
                JObject jObject = JObject.Parse(file.ReadToEnd());
                return (string)jObject[Key];

            }
            return null;//此处应用户输入正确的path
        }

    }
}
