using System;

namespace PixivRankBot
{
    class Program
    {

        static void Main(string[] args)
        {
            CQ cq = new CQ();
            Other other = new Other();
            cmd cmd = new cmd();
            Main main = new Main();
            Init init = new Init();
            Api api = new Api();
            Monitor monitor = new Monitor();
            //if (args != null)
            //{
            //    monitor.MonitorInit();

            //    main.Start(args);
            //}

            monitor.MonitorInit();

            string Put;
            while (true)
            {
                switch (Console.ReadLine().ToLower())
                {

                    case "exit":
                        return;

                    case "help":
                        cmd.Help();
                        break;

                    case "status":
                        cmd.Status();
                        break;
                    case "start":
                        main.Init(args);
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




                    case "type":
                        Console.Write("(输入:q退出)Put Type ");
                        Put = Console.ReadLine();
                        if (Put != "" && Put != ":q")
                        {
                            main.Type = Console.ReadLine();
                            Console.WriteLine("Now Type= " + main.Type);
                        }
                        break;
                    case "Id":
                        Console.Write("(输入:q退出)Put Id ");
                        Put = Console.ReadLine();
                        if (Put != "" && Put != ":q")
                        {
                            main.Id = Console.ReadLine();
                            Console.WriteLine("Now Id= " + main.Id);
                        }
                        break;

                    case "RankType":
                        Console.Write("(输入:q退出)Put RankType ");
                        Put = Console.ReadLine();
                        if (Put != "" && Put != ":q")
                        {
                            main.RankType = Console.ReadLine();
                            Console.WriteLine("Now RankType= " + main.RankType);
                        }
                        break;
                    case "Total":
                        Console.Write("(输入:q退出)Put Total ");
                        Put = Console.ReadLine();
                        if (Put != "" && Put != ":q")
                        {
                            main.Total = Console.ReadLine();
                            Console.WriteLine("Now Total= " + main.Total);
                        }
                        break;
                    case "Api":
                        Console.Write("(输入:q退出)Put Api ");
                        Put = Console.ReadLine();
                        if (Put != "" && Put != ":q")
                        {
                            main.Api = Console.ReadLine();
                            Console.WriteLine("Now Api= " + main.Api);
                        }
                        break;

                    case "path":
                        Console.ReadLine();
                        break;
                }
            }

        }
    }
}
