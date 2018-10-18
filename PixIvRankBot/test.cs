using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace PixivRankBot
{
    class test
    {
        /// <summary>
        /// Gets the j object.
        /// </summary>
        /// <returns></returns>
        public JObject GetJObject()
        {
            var obj = new JObject { { "Name", "Mark" } };
            return obj;
        }


    }
}
