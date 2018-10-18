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

            

            while (true)
            {
                switch (Console.ReadLine())
                {
                    case "exit":
                        return;

                    case "help":
                        cmd.Help();
                        break;
                    case "path":
                        Console.ReadLine();
                        break;
                    case "status":
                        cmd.Status();
                        break;
                    case "start":
                        main.Start(args);
                        break;

                    case "monitor":
                        monitor.MonitorInit();
                        break;

                    case "set start time":
                        break;

                    case "":
                        break;
                }
            }

        }
    }
}
