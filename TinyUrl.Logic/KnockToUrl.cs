using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace TinyUrl.Logic
{
    public static class KnockToUrl
    {
        public async static Task<bool> Knock(string urlString)
        {
            using (var client = new HttpClient() {Timeout = TimeSpan.FromSeconds(5)})
            {
                try
                {
                    var ans = await client.GetAsync(urlString);
                    return ans.StatusCode == HttpStatusCode.OK;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}