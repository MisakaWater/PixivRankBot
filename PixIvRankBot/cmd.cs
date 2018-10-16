using System;
using System.Collections.Generic;
using System.Text;

namespace PixivRankBot
{
    class cmd
    {
        public void Help()
        {
            Console.WriteLine(@"0.Path :酷Q的根目录下 data\image");
            Console.WriteLine("1.Type :消息类型，private 为 qq 用户，group 为群，discuss 讨论组");
            Console.WriteLine("2.ID :发送对象的ID，private为 qq 号，group为群号，discuss 讨论组号");
            Console.WriteLine("3.RankType :排行榜类型，参考 https://docs.rsshub.app/#pixiv");
            Console.WriteLine("4.Total :总数");
        }
    }
}
