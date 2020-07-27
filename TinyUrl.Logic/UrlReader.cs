namespace TinyUrl.Logic
{
    public static class UrlReader
    {
        public static string ReadUrl(string url)
        {
            if (url.Substring(0,8).ToLower() == "https://" || url.Substring(0,7).ToLower() == "http://" )
                return url;
            if (url.Substring(0, 4).ToLower() == "www.")
                return "https://" + url.Substring(4);
            return "https://" +url;
        }
        
    }
}