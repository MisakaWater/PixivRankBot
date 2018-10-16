using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PixivRankBot
{
    class tmpPath
    {
        public static string temp = Environment.GetEnvironmentVariable("TEMP");
    }

    class Other
    {
        /// <summary>
        /// 将传入参数转换为CQ代码
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public string CqCodeMsg(string Type, string Key, string Value, bool enter = false)
        {
            Console.WriteLine("CqCodeMsg()");
            //var TypeList = new List<string>();



            //var TypeIdList = new List<string>();
            //TypeIdList.Add("face");
            //TypeIdList.Add("emoji");
            //TypeIdList.Add("bface");
            //TypeIdList.Add("sface");

            //var TypeFileList = new List<string>();
            //TypeFileList.Add("image");
            //TypeFileList.Add("record");
            //TypeFileList.Add("record");

            //for (int i = 0; i < TypeList.Count; i++)
            //{

            //}

            string code = string.Empty;

            code = string.Format("{0}{1}{2}{3}{4}{5}{6}", "[CQ:", Type, ",", Key, "=", Value, "]");

            return code;
        }

        /// <summary>
        /// 解析HtmlCode P标签
        /// </summary>
        /// <param name="HtmlCode">传入Htmlcode</param>
        /// <returns></returns>
        public List<string> HtmlPParser(string HtmlCode)
        {
            Console.WriteLine("HtmlParser()");
            var ret = new List<string>();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(HtmlCode);
            var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//p");
            foreach (var node in htmlNodes)
            {
                ret.Add(node.InnerText);
            }
            return ret;
        }

        /// <summary>
        /// 解析HtmlCode src属性
        /// </summary>
        /// <param name="HtmlCode">传入Htmlcode</param>
        /// <returns></returns>
        public List<string> HtmlSrcParser(string HtmlCode)
        {
            Console.WriteLine("HtmlParser()");
            var ret = new List<string>();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(HtmlCode);
            var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//p/img");
            foreach (var node in htmlNodes)
            {
                ret.Add(node.Attributes["src"].Value);
            }
            return ret;
        }

        /// <summary>
        /// 拼接 pixiv.cat Url
        /// </summary>
        /// <param name="id">图片ID</param>
        /// <param name="format">图片格式，可选.jpg .png .gif</param>
        /// <returns></returns>
        public string PixivCat(string id, string format = ".jpg")
        {
            Console.WriteLine("PixivCat()");

            return string.Format("{0}{1}{2}", "https://pixiv.cat/", id, format);
        }

        /// <summary>
        /// 下载图片到指定文件夹内，返回下载后的本地文件地址
        /// </summary>
        /// <param name="url">需下载图片的Url地址</param>
        /// <param name="name">需下载的图片名字，空则为执行函数时的时间</param>
        /// <param name="path">需下载图片后保存的路径，默认为TMP文件夹下</param>
        /// <returns></returns>
        public string DownloadImg(string url, string name = "", string path = "")
        {
            Console.WriteLine("DownloadImg()");

            if (name == "")
            {
                name = string.Format("{0}{1}", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff"), ".jpg");
            }

            if (path == "")
            {
                path = string.Format("{0}{1}{2}", tmpPath.temp, @"\", name);
            }
            else
            {
                path = string.Format("{0}{1}{2}", path, @"\", name);
            }

            Uri uri = new Uri(url);

            WebClient wc = new WebClient();

            try
            {
                //wc.DownloadProgressChanged += (s, e) =>
                //{
                //    Console.WriteLine(e.ProgressPercentage);

                //};
                //wc.DownloadFileAsync(uri, path);
                wc.DownloadFile(uri, path);

                return name;
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex);
                return ex.ToString();
                throw;
            }
        }


        /// <summary>
        /// 删除指定文件
        /// </summary>
        /// <param name="name">文件名</param>
        /// <param name="path">路径</param>
        public void DelFile(string name = "", string path = "")
        {
            Console.WriteLine("DelFile()");
            string Dpath = string.Format("{0}{1}{2}", path, "\\", name.ToString());
            System.IO.File.Delete(Dpath);
        }


        /// <summary>
        /// 延时方法,时间误差貌似为0.002ms
        /// </summary>
        /// <param name="s">延时多少秒</param>
        public void Wait(int s)
        {
            Console.WriteLine("Wait("+s+")");
            DateTime t1, t2;
            long a;
            bool flg = true;
            t1 = DateTime.Now;
            long HowLong = s * 10000000;//10000000ticks 为 1s
            while (flg)
            {
                t2 = DateTime.Now;
                a = t2.Ticks - t1.Ticks;
                if (a > HowLong)
                {
                    flg = false;
                }
            }
        }

    }
}
