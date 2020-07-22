using System;

namespace TinyUrl.Logic
{
    public static class TinyCreator
    {
        public static string CreateTinyUrl(string url)
        {
            var rand =new Random();
            int r = rand.Next(10000000, 19999999);
            return r.ToString();
        }
    }
}