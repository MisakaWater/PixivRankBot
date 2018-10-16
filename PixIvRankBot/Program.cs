using System;
namespace PixivRankBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Other other = new Other();
            cmd cmd = new cmd();
            Main main = new Main();
            Init init = new Init();

            Api api = new Api();
            if (args != null)
            {
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
                    }
            }

        }
    }
}
