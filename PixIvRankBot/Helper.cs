using System;
using System.Collections.Generic;
using System.Text;

namespace PixivRankBot
{
    class Helper
    {
        /// <summary>
        /// 设置帮助
        /// </summary>
        /// <returns></returns>
        public string CQSetHelp()
        {
            return "0.Path=>酷Q的根目录下 data\\image,自动获取酷Q运行目录" + "\r\n" +
                  "1.Type=>消息类型，private 为 qq 用户，group 为群，discuss 讨论组" + "\r\n" +
                  "2.ID=>发送对象的ID，private为 qq 号，group为群号，discuss 讨论组号" + "\r\n" +
                  "3.RankType=>排行榜类型，参考 https://docs.rsshub.app/#pixiv" + "\r\n" +
                  "4.Total=>总数" + "\r\n" +
                  "5.ApiType=>ApiV1,ApiV2,HtmlApi";
        }

        public string[] SetInfo()
        {
            //Cmd cmd = new Cmd();
            //var a = cmd.Info;
            return null;
        }

        public void Help()
        {
            Console.WriteLine(@"0.Path=>酷Q的根目录下 data\image,自动获取酷Q运行目录");
            Console.WriteLine("1.Type=>消息类型，private 为 qq 用户，group 为群，discuss 讨论组");
            Console.WriteLine("2.ID=>发送对象的ID，private为 qq 号，group为群号，discuss 讨论组号");
            Console.WriteLine("3.RankType=>排行榜类型，参考 https://docs.rsshub.app/#pixiv");
            Console.WriteLine("4.Total=>总数");
            Console.WriteLine("5.ApiType=>ApiV1,ApiV2,HtmlApi");
        }


        public void Status()
        {

        }

        public void AppCfg(string[] Config)
        {
            try
            {
                Console.WriteLine("Path:" + "运行时自动获取酷Q目录");
                Console.WriteLine("Type:" + Config[0]);
                Console.WriteLine("Id:" + Config[1]);
                Console.WriteLine("RankType:" + Config[2]);
                Console.WriteLine("Total:" + Config[3]);
                Console.WriteLine("ApiType:" + Config[4]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }
        }
        public void AppCfg(string Path, string Type, int Id, string RankType, int Total, string ApiType)
        {
            try
            {
                Console.WriteLine("Path:" + Path);
                Console.WriteLine("Type:" + Type);
                Console.WriteLine("Id:" + Id);
                Console.WriteLine("RankType:" + RankType);
                Console.WriteLine("Total:" + Total);
                Console.WriteLine("ApiType:" + ApiType);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }
        }

    }
}
