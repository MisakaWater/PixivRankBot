using System;
using System.Collections.Generic;

namespace PixivRankBot
{
    class Init
    {
        /// <summary>
        /// 获取应用参数
        /// </summary>
        /// <param name="args">参数数组</param>
        /// 返回时
        /// //[0]=Path
        /// [0]=Type
        /// [1]=Id
        /// [2]=RankType
        /// [3]=Total
        /// [4]=ApiType
        public string[] StartAppInfo(string[] args)
        {
            Colorful.Console.WriteLine("Init.Info()",System.Drawing.Color.Yellow);
            //Cmd cmd = new Cmd();
            Main main = new Main();
            Helper helper = new Helper();
            string[] Strret = new string[5];
            
            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    switch (args[i].ToLower())//判断是否为"-"的参数选项
                    {
                        case "-h":
                            helper.Help();
                            //ret.Add("args[] 为空");
                            break;
                        case "-help":
                            helper.Help();
                            //ret.Add("args[] 为空");
                            break;


                        //case "-path":
                        //    //ret.Add(args[i + 1]);
                        //    Strret[0] = args[i + 1];
                        //    break;


                        case "-type":
                            //ret.Add(args[i + 1]);
                            Strret[0] = args[i + 1];
                            
                            //main.Type = args[i + 1];
                            break;



                        case "-id":
                            //ret.Add(args[i + 1]);
                            Strret[1] = args[i + 1];
                            //main.Id= int.Parse(args[i + 1]);
                            break;



                        case "-ranktype":
                            //ret.Add(args[i + 1]);
                            Strret[2] = args[i + 1];
                            //main.RankType= args[i + 1];
                            break;



                        case "-total":
                            //ret.Add(args[i + 1]);
                            Strret[3] = args[i + 1];
                           // main.Total= int.Parse(args[i + 1]);
                            break;


                        case "-api":
                            //ret.Add(args[i + 1]);
                            Strret[4] = args[i + 1];
                            //main.ApiType= args[i + 1];
                            break;

                    }
                }
            }
            else
            {
                //ret.Add("args[] 为空");
                Strret[0] = "args[] 为空";
            }

            Console.WriteLine("Start Parameter:");
            helper.AppCfg(Strret);
            Console.WriteLine("=====END=====");

            return Strret;
        }



    }
}
