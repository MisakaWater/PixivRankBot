using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Console = Colorful.Console;

namespace PixivRankBot
{
    static class Cmd
    {
        static public string[] Info { get; set; }
        static public void InitRead(string[] args)
        {
            Console.WriteLine("InitRead()", Color.Yellow);
            Init init = new Init();
            Monitor monitor = new Monitor();
            string[] StartAppInfo = init.StartAppInfo(args); 
            Info = StartAppInfo;
            //await Task.Run(() =>
            //{
            //    ReadSelect();
            //    monitor.MonitorInit();
            //});
        }

        /// <summary>
        /// 用户输入运行对应的方法
        /// </summary>
        /// <param name="Read">传参</param>
        static public void ReadSelect(string Read = "")
        {
            CQ cq = new CQ();
            Main main = new Main();
            Monitor monitor = new Monitor();
            Helper helper = new Helper();

            string Put;
            switch (Read.ToLower())
            {

                case "exit":
                    return;

                case "help":
                    helper.Help();
                    break;

                case "status":
                    helper.AppCfg(Info);
                    break;
                case "start":
                    main.AsyncRun(Info);
                    break;

                case "monitor":
                    monitor.MonitorInit();
                    break;

                case "set start time":
                    break;

                case "clear":
                    Console.Clear();
                    break;

                case "":
                    break;


                // Info[]
                // [0]=Type
                // [1]=Id
                // [2]=RankType
                // [3]=Total
                // [4]=ApiType
                case "type":
                    Console.Write("(输入:q退出)Put Type=>");
                    Put = Console.ReadLine();
                    if (Put != "" && Put != ":q")
                    {
                        Info[0] = Put;
                        Console.WriteLine("Now Type= " + Info[0]);
                    }
                    break;
                case "id":
                    Console.Write("(输入:q退出)Put Id=>");
                    Put = Console.ReadLine();
                    if (Put != "" && Put != ":q")
                    {

                        Info[1] = Put;
                        Console.WriteLine("Now Id= " + Info[1]);
                    }
                    break;

                case "ranktype":
                    Console.Write("(输入:q退出)Put RankType=>");
                    Put = Console.ReadLine();
                    if (Put != "" && Put != ":q")
                    {
                        Info[2] = Put;
                        Console.WriteLine("Now RankType= " + Info[2]);
                    }
                    break;
                case "total":
                    Console.Write("(输入:q退出)Put Total=>");
                    Put = Console.ReadLine();
                    if (Put != "" && Put != ":q")
                    {
                        Info[3] = Put;
                        Console.WriteLine("Now Total= " + Info[3]);
                    }
                    break;
                case "apitype":
                    Console.Write("(输入:q退出)Put Api=>");
                    Put = Console.ReadLine();
                    if (Put != "" && Put != ":q")
                    {
                        Info[4] = Put;
                        Console.WriteLine("Now Api= " + Info[4]);
                    }
                    break;

                case "path":
                    Console.ReadLine();
                    break;

            }
        }

        static public void ReadSelect(List<string> Read)
        {
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
            ///
            CQ cq = new CQ();
            Main main = new Main();
            Helper helper = new Helper();

            if (Read != null)
            {

                if (Read[Read.IndexOf("message_type") + 1] == "private")//私聊
                {
                    switch (Read[Read.IndexOf("message") + 1])
                    {
                        case "help":
                            cq.HttpSendMsg(Read[Read.IndexOf("message_type") + 1], int.Parse(Read[Read.IndexOf("user_id") + 1]), helper.CQSetHelp(), get: false);
                            break;
                    }

                    if (Read[Read.IndexOf("message") + 1].Contains("type=>"))
                    {
                        Info[0] = Read[Read.IndexOf("message") + 1].Replace("type=>", "");
                        cq.HttpSendMsg(Read[Read.IndexOf("message_type") + 1], int.Parse(Read[Read.IndexOf("user_id") + 1]), "Now Type=>" + Info[0], get: false);
                        ;
                    }


                }
                if (Read[Read.IndexOf("message_type") + 1] == "group")//群聊
                {
                    cq.HttpSendMsg(Read[Read.IndexOf("message_type") + 1], int.Parse(Read[Read.IndexOf("group_id") + 1]), helper.CQSetHelp(), get: false);
                }
            }
        }
    }
}
