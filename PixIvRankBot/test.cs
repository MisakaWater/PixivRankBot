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
        public void GetJObject()
        {
            JObject postObj = new JObject();
            postObj["message_type"] = "MessageType";
            postObj[""] = "Id";
            postObj["message"] = "Message";
            postObj["auto_escape"] = "AutoEscape.ToString()";
            var a =postObj.ToString();
            ;
        }


    }
}
