using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TinyUrl.Logic
{
    public static class TinyCreator
    {
        public static string CreateTinyUrl(string url)
        {
            var rand =new Random();
            var bytes = new byte[6];
            rand.NextBytes(bytes);
            return Base64UrlEncoder.Encode(bytes);
        }

        public static string CreateTinyUrlByRandomChar(string url)
        {
            var rand = new Random();
            var tinyUrl = new StringBuilder(8);
            for (var i = 0; i < tinyUrl.Capacity; i++)
                tinyUrl.Append((char) rand.Next(58, 64)); // 48=='0' , 122 = 'z' , 97 = 'a'
            return tinyUrl.ToString();
        }

        public static string CreateTinyUrFromInteger(string url)
        {
            var rand = new Random();
            int r = rand.Next(10000000, 19999999);
            return r.ToString();
        }

        //not Unique
        public static string CreateTinyUrlByHash(string url)
        {
            var md5 = MD5.Create();

            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(url));
            var str = Convert.ToBase64String(hash);

            var tinyUrl = new StringBuilder(8);

            for (var i = 0; i < str.Length; i = i + 3)
                tinyUrl.Append(str[i]);

            return tinyUrl.ToString();
        }
    }
}