using System.Net;
namespace SpeedRequest
{
    public class HttpProxyClient
    {
        public static Proxy Parse(string proxy)
        {
            return new Proxy() { Type = TypeProxy.HTTP, WebProxy = new WebProxy(proxy.Split(':')[0].Trim(), int.Parse(proxy.Split(':')[1].Trim())) };
        }
    }
}
