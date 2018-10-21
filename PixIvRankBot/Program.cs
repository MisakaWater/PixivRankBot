using System;

namespace PixivRankBot
{
    class Program
    {
        static void Main(string[] args)
        {
            CQ cq = new CQ();
            Other other = new Other();

            Main main = new Main();
            Init init = new Init();
            Api api = new Api();
            Monitor monitor = new Monitor();
            monitor.MonitorInit();

            Cmd.InitRead(args);
            while (true)
            {
                Cmd.ReadSelect(Console.ReadLine());
            }

        }
    }
}
