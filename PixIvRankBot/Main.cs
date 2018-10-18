using System;
using System.Collections.Generic;

namespace PixivRankBot
{
    class Main
    {
        public bool InfoFlg = true;
        public void Start(string[] args)
        {
            Other other = new Other();


            //if (true)
            //{
            //    test test = new test();
            //    test.PostJson();
            //}

            Init init = new Init();
            CQ cq = new CQ();
            Api api = new Api();
            var StartInfoList = new List<string>();
            StartInfoList = init.AppInfo(args);//获取启动参数



            int id = -1;
            int total = -1;
            bool typeflg = true;
            bool ApiFlg = true;
            bool IdFlg = true;
            string path = string.Empty;
            string type = string.Empty;
            string ApiType = string.Empty;
            string RankType = string.Empty;



            if (StartInfoList.Count == 6)//没有启动参数则为true
            {
                InfoFlg = StartInfoList[0] == "args[] 为空";
            }
            if (InfoFlg)//没有 启动参数 则询问参数
            {
                Console.Write("Put Path ");
                path = Console.ReadLine();

                Console.WriteLine("Message Type:");
                Console.WriteLine("0.private");
                Console.WriteLine("1.group");
                Console.WriteLine("2.discuss");

                type = string.Empty;

                typeflg = true;
                while (typeflg)
                {
                    type = Console.ReadLine();
                    if (type != "0" && type != "1" && type != "2")
                    {
                        if (type != "private" && type != "group" && type != "discuss")
                        {
                            Console.WriteLine("输入消息类型错误，请输入对应数值或类型名...");
                        }
                    }
                    else
                    {
                        switch (type)
                        {

                            case "0":
                                type = "private";
                                break;

                            case "1":
                                type = "group";
                                break;

                            case "2":
                                type = "discuss";
                                break;

                        }
                        typeflg = false;
                    }
                }

                Console.Write("Put Id ");

                while (IdFlg)
                {


                    if (!int.TryParse(Console.ReadLine(), out id))
                    {
                        Console.WriteLine("请输入数值。");
                    }
                    else
                    {
                        IdFlg = false;
                    }

                }

                Console.WriteLine("Rank Type ");
                Console.WriteLine("参考:https://docs.rsshub.app/#pixiv");
                RankType = Console.ReadLine();

                Console.WriteLine("Api Type:");
                Console.WriteLine("0.ApiV1");
                Console.WriteLine("1.ApiV2");
                Console.WriteLine("2.HtmlApi");
                while (ApiFlg)
                {
                    ApiType = Console.ReadLine();
                    if (ApiType != "0" && ApiType != "1" && ApiType != "2")
                    {
                        if (ApiType != "ApiV1" && ApiType != "ApiV2" && ApiType != "HtmlApi")
                        {
                            Console.WriteLine("输入Api类型错误，请输入对应数值或类型名...");
                        }
                    }
                    else
                    {
                        switch (ApiType)
                        {

                            case "0":
                                ApiType = "ApiV1";
                                break;

                            case "1":
                                ApiType = "ApiV2";
                                break;

                            case "2":
                                ApiType = "HtmlApi";
                                break;

                        }
                        ApiFlg = false;
                    }
                }
            }
            else
            {
                path = StartInfoList[0];
                Console.WriteLine("Path=" + path);
                type = StartInfoList[1];
                Console.WriteLine("Type=" + type);
                try
                {
                    id = Convert.ToInt32(StartInfoList[2]);
                    Console.WriteLine("ID=" + id);
                }
                catch (NotFiniteNumberException ex)
                {
                    Console.WriteLine(ex);
                }
                RankType = StartInfoList[3];
                Console.WriteLine("RankType=" + RankType);

                try
                {
                    total = Convert.ToInt32(StartInfoList[4]);
                    Console.WriteLine("Total=" + total);
                }
                catch (NotFiniteNumberException ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }

                switch (StartInfoList[5])
                {
                    case "ApiV1":
                        ApiType = "ApiV1";
                        break;
                    case "ApiV2":
                        ApiType = "ApiV2";
                        break;
                    case "HtmlApi":
                        ApiType = "HtmlApi";
                        break;
                }
            }

            List<string> ApiIdList = new List<string>();
            Console.WriteLine("ApiType=" + ApiType);
            switch (ApiType)
            {
                case "ApiV1":
                    ApiIdList = api.ApiV1PixivRank(RankType);
                    break;
                case "ApiV2":
                    ApiIdList = api.ApiV2PixivRank(RankType);
                    break;
                case "HtmlApi":
                    ApiIdList = api.HtmlPixivRank(RankType);
                    break;
            }
            if (ApiIdList.Count > 1)//如果ApiV2返回不为空
            {
                bool totalflg1 = true;

                if (InfoFlg)
                {

                    Console.Write("Total ");
                    totalflg1 = false;
                    string put = string.Empty;
                    bool totalflg2 = true;
                    while (totalflg2)
                    {
                        put = Console.ReadLine();
                        if (put == "")
                        {
                            Console.WriteLine("输入的值为空，请输入一个确定的值");
                        }
                        else
                        {
                            totalflg2 = false;
                        }
                    }
                    total = Convert.ToInt32(put);
                    if (total > ApiIdList.Count)
                        Console.WriteLine("输入的值太大了，现在共发送" + ApiIdList.Count);
                    else
                        totalflg1 = true;
                }



                string name = string.Empty;
                string code = string.Empty;
                string url = string.Empty;
                var count = new List<string>();
                var Delname = new List<string>();
                count.Add("Img Start Date:" + DateTime.Now.ToString("yyyy-MM-dd"));
                if (!totalflg1)
                {
                    for (int i = 0; i < ApiIdList.Count; i++)
                    {
                        url = other.PixivCat(ApiIdList[i]);
                        Console.WriteLine(i + "\r\n" + url);
                        name = other.DownloadImg(url, path: path);
                        Delname.Add(name);
                        code = other.CqCodeMsg("image", "file", name);
                        count.Add("id=" + ApiIdList[i] + "");
                        count.Add(code);
                    }

                    Console.WriteLine("Img Start");
                    count.Add("END Count:" + ApiIdList.Count);
                    code = string.Concat(count.ToArray());

                    cq.HttpSendMsg(type, id, code, get: false);
                }
                else
                {
                    for (int i = 0; i < total; i++)
                    {
                        url = other.PixivCat(ApiIdList[i]);
                        Console.WriteLine(i + "\r\n" + url);
                        name = other.DownloadImg(url, path: path);
                        Delname.Add(name);
                        code = other.CqCodeMsg("image", "file", name);
                        count.Add("\\nid=" + ApiIdList[i] + "\\n");
                        count.Add(code);
                    }

                    Console.WriteLine("Img Start");
                    count.Add("\\nEND Count:" + total);
                    code = string.Concat(count.ToArray());

                    cq.HttpSendMsg(type, id, code, get: false);


                }


                Console.WriteLine("END");
                Console.WriteLine(DateTime.Now.ToString());

                other.Wait(10);
                foreach (var item in Delname)
                {
                    other.DelFile(item, path);
                }


            }
        }
    }

}
