using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Colorful;
using System.Drawing;
using Console = Colorful.Console;

namespace PixivRankBot
{
    class Main
    {

        public bool InfoFlg = true;
        public string Path
        {
            get
            {
                CQ cq = new CQ();
                return cq.RunTimePath() + "data\\image";
            }
        }
        public string Type { get; set; }
        public int Id { get; set; }
        public string RankType { get; set; }
        public int Total { get; set; }
        public string ApiType { get; set; }


        public async void AsyncRun(string[] Info)
        {
            Console.WriteLine("AsyncRun()",Color.Yellow);
            await Task.Run(() =>
            {
                InitConfig(Info);
            });
        }

        public void InitConfig(string[] Config)
        {
            Console.WriteLine("InitConfig()", Color.Yellow);
            // Info[]
            // [0]=Type
            // [1]=Id
            // [2]=RankType
            // [3]=Total
            // [4]=ApiType
            Type = Config[0];
            Id = int.Parse(Config[1]);
            RankType = Config[2];
            Total = int.Parse(Config[3]);
            ApiType = Config[4];




            List<string> ApiIdList = new List<string>();
            Api api = new Api();
            //Cmd cmd = new Cmd();
            Console.WriteLine("Now Use The Config", Color.Yellow);
            //cmd.AppCfg(Path,Type,Id,RankType,Total,ApiType);
            if (Type == null && Id == 0 && RankType == null && Total == 0 && ApiType == null)
            {
                var idlist = ReadConfig();
                    Start(idlist);
                
            }
            else
            {
                switch (ApiType.ToLower())
                {
                    case "apiv1":
                        ApiIdList = api.ApiV1PixivRank(RankType);
                        break;
                    case "apiv2":
                        ApiIdList = api.ApiV2PixivRank(RankType);
                        break;
                    case "htmlapi":
                        ApiIdList = api.HtmlPixivRank(RankType);
                        break;
                }
                if (ApiIdList != null)
                    Start(ApiIdList);
                else
                    Console.WriteLine("网络较为缓慢请稍后重试。",Color.Red);
            }

            

        }



        public void Start(List<string> ApiIdList)
        {
            Console.WriteLine("Start()",Color.Yellow);
            Other other = new Other();
            CQ cq = new CQ();
            bool totalflg1 = true;
            if (Total == 0 && Total == -1)
            {
                totalflg1 = false;
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
                    Console.WriteLine(i + "\r\n" + url, Color.Yellow);
                    name = other.DownloadImg(url, path: Path);
                    Delname.Add(name);
                    code = other.CqCodeMsg("image", "file", name);
                    count.Add(code);
                    count.Add("\r\nid=" + ApiIdList[i] + "\r\n");
                }

                Console.WriteLine("Img Start", Color.Yellow);
                count.Add("END Count:" + ApiIdList.Count);
                code = string.Concat(count.ToArray());

                cq.HttpSendMsg(Type, Id, code, get: false);
            }
            else
            {


                for (int i = 0; i < Total; i++)
                {
                    url = other.PixivCat(ApiIdList[i]);
                    Console.WriteLine(i + "\r\n" + url);
                    name = other.DownloadImg(url, path: Path);
                    Delname.Add(name);
                    code = other.CqCodeMsg("image", "file", name);
                    count.Add("\r\n" + code);
                    count.Add("\r\nid=" + ApiIdList[i]);
                }

                Console.WriteLine("Img Start", Color.Yellow);
                count.Add("\nEND Count:" + Total);
                code = string.Concat(count.ToArray());

                cq.HttpSendMsg(Type, Id, code, get: false);


            }


            Console.WriteLine("END", Color.Yellow);
            Console.WriteLine(DateTime.Now.ToString(), Color.Yellow);

            other.Wait(10);
            foreach (var item in Delname)
            {
                other.DelFile(item, Path);
            }

            return;

        }





        public List<string> ReadConfig()
        {
            Console.WriteLine("ReadConfig()",Color.Yellow);
            List<string> ApiIdList = new List<string>();
            Console.WriteLine("Message Type:",Color.Yellow);
            Console.WriteLine("0.private", Color.Green);
            Console.WriteLine("1.group", Color.Green);
            Console.WriteLine("2.discuss", Color.Green);
            bool typeflg = true;
            while (typeflg)
            {
                Type = Console.ReadLine();
                if (Type != "0" && Type != "1" && Type != "2")
                {
                    if (Type != "private" && Type != "group" && Type != "discuss")
                    {
                        Console.WriteLine("输入消息类型错误，请输入对应数值或类型名...",Color.Red);
                    }
                }
                else
                {
                    switch (Type.ToLower())
                    {

                        case "0":
                            Type = "private";
                            break;

                        case "1":
                            Type = "group";
                            break;

                        case "2":
                            Type = "discuss";
                            break;

                    }
                    typeflg = false;
                }
            }

            Console.Write("Put Id ", Color.Yellow);
            bool IdFlg = true;
            while (IdFlg)
            {
                var tmp = -1;

                if (!int.TryParse(Console.ReadLine(), out tmp))
                {
                    Console.WriteLine("请输入数值。",Color.Red);
                }
                else
                {
                    Id = tmp;
                    IdFlg = false;
                }

            }

            Console.WriteLine("Rank Type ", Color.Yellow);
            Console.WriteLine("参考:https://docs.rsshub.app/#pixiv");
            RankType = Console.ReadLine();

            Console.WriteLine("Api Type:",Color.Green);
            Console.WriteLine("0.ApiV1", Color.Green);
            Console.WriteLine("1.ApiV2", Color.Green);
            Console.WriteLine("2.HtmlApi", Color.Green);
            bool ApiFlg = true;
            while (ApiFlg)
            {
                ApiType = Console.ReadLine();
                if (ApiType != "0" && ApiType != "1" && ApiType != "2")
                {
                    if (ApiType != "ApiV1" && ApiType != "ApiV2" && ApiType != "HtmlApi")
                    {
                        Console.WriteLine("输入Api类型错误，请输入对应数值或类型名...", Color.Red);
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



            Api api = new Api();
            Console.WriteLine("ApiType=" + ApiType);
            switch (ApiType.ToLower())
            {
                case "apiv1":
                    ApiIdList = api.ApiV1PixivRank(RankType);
                    break;
                case "apiv2":
                    ApiIdList = api.ApiV2PixivRank(RankType);
                    break;
                case "htmlapi":
                    ApiIdList = api.HtmlPixivRank(RankType);
                    break;
            }

            if (ApiIdList != null)//如果Api返回不为空
            {
                bool totalflg1 = true;
                Console.Write("Total ");
                totalflg1 = false;
                string put = string.Empty;
                bool totalflg2 = true;
                while (totalflg2)
                {
                    put = Console.ReadLine();
                    if (put == "")
                    {
                        Console.WriteLine("输入的值为空，请输入一个确定的值", Color.Red);
                    }
                    else
                    {
                        totalflg2 = false;
                    }
                }
                Total = Convert.ToInt32(put);
                if (Total > ApiIdList.Count)
                    Console.WriteLine("输入的值太大了，现在共发送" + ApiIdList.Count,Color.Red);
                else
                    totalflg1 = true;
            }
            return ApiIdList;
        }
    }
}
