using System;

namespace PixivRankBot
{
    class Program
    {
        static void Main(string[] args)
        {
            CQ cq = new CQ();
            test test = new test();
            test.GetJObject();

            Other other = new Other();
            cmd cmd = new cmd();
            Main main = new Main();
            Init init = new Init();


            Api api = new Api();
            Monitor monitor = new Monitor();
            if (args != null)
            {
                monitor.MonitorInit();

                main.Start(args);
            }

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
                    }
            }

        }
    }
}
