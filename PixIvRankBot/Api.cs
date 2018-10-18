using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml;
using HtmlAgilityPack;

namespace PixivRankBot
{
    class Api
    {
        /// <summary>
        /// 通过API V1 获取Pixiv的排行榜的图片ID
        /// </summary>
        /// <param name="mode">排行榜类型，类型有:参考https://docs.rsshub.app/#pixiv</param>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<string> ApiV1PixivRank(string content = "illust", string mode = "daily")//, string date = "")
        {
            Console.WriteLine("ApiV1PixivRank()");

            var ret = new List<string>();

            //string url = string.Format("{0}{1}", "https://www.pixiv.net/ranking.php?mode=", mode);

            string url = string.Format("{0}{1}{2}{3}{4}{5}", "https://api.imjad.cn/pixiv/v1/?type=rank&", "content=", content, "&", "mode=", mode);

            Uri uri = new Uri(url);

            WebClient wc = new WebClient();

            string json;

            try
            {
                json = wc.DownloadString(url);
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex);
                ret.Add(ex.ToString());
                return ret;
                throw;
            }

            JObject js = JObject.Parse(json);

            foreach (var itme in js)
            {
                if (itme.Key.ToString() == "response")
                {
                    var t = itme.Value;
                }

            }

            //var works = response["works"];


            //try
            //{
            //    results = JsonConvert.DeserializeObject<List<TestTag>>(strJson);

            //}
            //catch(ArgumentException ex)
            //{
            //    ret.Add(ex.ToString());
            //    return ret;
            //    throw;
            //}
            //catch (NullReferenceException ex)
            //{
            //    ret.Add(ex.ToString());
            //    return ret;
            //    throw;
            //}
            //for (int i = 0; i < results.Count; i++)
            //{
            //    ret.Add(results[i].First.First.ToString());
            //}

            return ret;
        }



        /// <summary>
        /// 通过RssHub.app获取Pixiv图片信息
        /// </summary>
        /// <param name="mode">排行榜类型</param>
        /// <param name="date">日期, 取值形如 2018-4-25</param>
        /// <returns></returns>
        public List<List<string>> RssPixivRank(string mode = "day", string date = "")
        {
            Console.WriteLine("RssPixivRank()");

            if (date == "")
            {
                date = DateTime.Now.ToString("yyyy-MM-dd");
            }

            string url = string.Format("{0}{1}{2}", "https://rsshub.app/pixiv/ranking/", mode, "/", date);

            var ret = new List<List<string>>();
            var error = new List<string>();
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(url);
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex);
                error.Add(ex.ToString());
                ret.Add(error);
                return ret;
                throw;
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine(ex);
                error.Add(ex.ToString());
                ret.Add(error);
                return ret;
                throw;
            }

            XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xml.NameTable);
            XmlNodeList titleNode = xml.SelectNodes("/rss/channel");
            var info = new List<string>();
            foreach (XmlElement item in titleNode)
            {
                info.Add(item.GetElementsByTagName("description")[0].InnerText);
            }

            var ItemTitle = new List<string>();
            var ItemLink = new List<string>();
            var ItemDescription = new List<string>();
            XmlNodeList nodelist = xml.SelectNodes("/rss/channel/item");
            foreach (XmlElement item in nodelist)
            {
                ItemTitle.Add(item.GetElementsByTagName("title")[0].InnerText);
                ItemDescription.Add(item.GetElementsByTagName("description")[0].InnerText);
                ItemLink.Add(item.GetElementsByTagName("link")[0].InnerText);
            }

            if (ItemTitle.Count == ItemLink.Count)
            {
                if (ItemDescription.Count == ItemLink.Count)
                {
                    ret.Add(info);
                    ret.Add(ItemTitle);
                    ret.Add(ItemLink);
                    ret.Add(ItemDescription);
                    return ret;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }






        /// <summary>
        /// 通过解析Pixiv.net的Html获取图片ID
        /// </summary>
        /// <param name="mode">排行榜类型</param>
        /// <param name="content">图片类型</param>
        /// <returns></returns>
        public List<string> HtmlPixivRank(string mode = "day",string content = "illust")
        {
            Console.WriteLine("HtmlPixivRank()");
            if (mode == "day")
            {
                mode = "daily";
            }

            var ret = new List<string>();
            int total = 30;
            WebClient wc = new WebClient();
            string url = string.Format("{0}{1}{2}{3}", "https://www.pixiv.net/ranking.php?mode=", mode, "&content=",content);
            //获取插图类型的Pixiv排行榜
            string html = string.Empty;
            try
            {
                html = wc.DownloadString(url);
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex);
                ret.Add(ex.ToString());
                return null;
            }
            catch (StackOverflowException ex)
            {
                Console.WriteLine(ex);
                ret.Add(ex.ToString());
                return null;
            }
            catch (HttpListenerException ex)
            {
                Console.WriteLine(ex);
                ret.Add(ex.ToString());
                return null;
            }


            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            int itemTotal;
            string ImgName;
            HtmlNode section, ranking, count;
            HtmlNode rootnode = htmlDoc.DocumentNode;
            for (int i = 1; i < total + 1; i++)
            {
                try
                {
                    count = null;
                    //section = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"wrapper\"]/div[1]/div/div[2]/div[1]/section[" + i + "]");
                    section = htmlDoc.DocumentNode.SelectSingleNode("//section[@id=" + i + "]");

                    //section = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"wrapper\"]");
                    ranking = section.SelectSingleNode("./div[@class='ranking-image-item']");
                    if (ranking.InnerHtml.IndexOf("page-count") != -1)
                    {
                        count = ranking.SelectSingleNode("./a/div[@class='page-count']");
                    }
                    if (count != null)
                    {
                        itemTotal = Convert.ToInt32(count.InnerText);
                    }
                    else
                    {
                        itemTotal = 0;
                    }

                    ImgName = section.Attributes[10].Value;//获取图片名
                    if (itemTotal > 1)//如果是图片集则为true
                    {
                        for (int i1 = 1; i1 < itemTotal + 1; i1++)
                        {
                            ImgName = string.Format("{0}{1}{2}", section.Attributes[10].Value, "-", i1);
                            ret.Add(ImgName);
                        }


                    }
                    else
                    {
                        ret.Add(ImgName);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }

            }


            return ret;
        }


        /// <summary>
        /// 通过API V2 获取Pixiv的排行榜的图片ID
        /// </summary>
        /// <param name="mode">排行榜类型，类型有:参考https://docs.rsshub.app/#pixiv</param>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<string> ApiV2PixivRank(string mode = "day")//, string date = "")
        {
            Console.WriteLine("ApiV2PixivRank()");

            var ret = new List<string>();

            //string url = string.Format("{0}{1}", "https://www.pixiv.net/ranking.php?mode=", mode);

            string url = string.Format("{0}{1}", "https://api.imjad.cn/pixiv/v2/?type=rank&mode=", mode);

            Uri uri = new Uri(url);

            WebClient wc = new WebClient();

            string json;

            try
            {
                json = wc.DownloadString(url);
            }
            catch (WebException ex)
            {
                ret.Add(ex.ToString());
                Console.WriteLine(ex);
                return ret;
                throw;
            }

            IList<JToken> results;

            JObject js = JObject.Parse(json);
            try
            {
                results = js["illusts"].Children().ToList();
            }
            catch (NullReferenceException ex)
            {
                ret.Add(ex.ToString());
                Console.WriteLine(ex);
                return ret;
                throw;
            }

            string name = string.Empty;

            for (int i = 0; i < results.Count; i++)
            {
                if (results[i]["type"].ToString() == "illust")
                {
                    if (results[i]["meta_pages"].Count() > 0)
                    {
                        for (int i1 = 1; i1 < results[i]["meta_pages"].Count() + 1; i1++)
                        {
                            name = string.Format("{0}{1}{2}", results[i].First.First.ToString(), "-", i1);
                            ret.Add(name);
                        }
                    }
                    else
                    {
                        name = results[i].First.First.ToString();
                        ret.Add(name);
                    }
                }
            }

            return ret;
        }

    }
}
