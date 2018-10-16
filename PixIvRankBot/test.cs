using System;
using System.IO;
using System.Net;
using System.Text;

namespace PixivRankBot
{
    class test
    {
        public static string PostJson()
        {
            try
            {
                string postData = "{ \"user_id\": 719980538,\"message\": \"你\\r\\n好\"}";
                
                byte[] data = Encoding.UTF8.GetBytes(postData);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:5700/send_private_msg");
                request.Method = "POST";
                request.ContentType = "application/json; charset=UTF-8";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";
                request.ContentLength = data.Length;
                Stream newStream = request.GetRequestStream();

                newStream.Write(data, 0, data.Length);
                newStream.Close();

                HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string content = reader.ReadToEnd();
                Console.WriteLine(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            return null;
        }

    }
}
