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
        /// [0]=Path
        /// [1]=Type
        /// [2]=Id
        /// [3]=RankType
        /// [4]=Total
        /// [5]=ApiType
        public List<string> AppInfo(string[] args)
        {
            Console.WriteLine("Init.Info()");
            cmd cmd = new cmd();
            var ret = new List<string>();
            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    switch (args[i])//判断是否为"-"的参数选项
                    {
                        case "-h":
                            cmd.Help();
                            ret.Add("args[] 为空");
                            break;
                        case "-help":
                            cmd.Help();
                            ret.Add("args[] 为空");
                            break;


                        case "-path":
                            ret.Add(args[i + 1]);
                            break;
                        case "-Path":
                            ret.Add(args[i + 1]);
                            break;


                        case "-type":
                            ret.Add(args[i + 1]);
                            break;
                        case "-Type":
                            ret.Add(args[i + 1]);
                            break;


                        case "-id":
                            ret.Add(args[i + 1]);
                            break;
                        case "-Id":
                            ret.Add(args[i + 1]);
                            break;
                        case "-ID":
                            ret.Add(args[i + 1]);
                            break;


                        case "-ranktype":
                            ret.Add(args[i + 1]);
                            break;
                        case "-Ranktype":
                            ret.Add(args[i + 1]);
                            break;
                        case "-RankType":
                            ret.Add(args[i + 1]);
                            break;
                        case "-rankType":
                            ret.Add(args[i + 1]);
                            break;


                        case "-total":
                            ret.Add(args[i + 1]);
                            break;
                        case "-Total":
                            ret.Add(args[i + 1]);
                            break;

                        case "-api":
                            ret.Add(args[i + 1]);
                            break;
                        case "-Api":
                            ret.Add(args[i + 1]);
                            break;
                    }
                }
            }
            else
            {
                ret.Add("args[] 为空");
            }


            return ret;
        }



    }
}
