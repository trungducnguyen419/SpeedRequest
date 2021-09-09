using System.Net;
namespace SpeedRequest
{
    public class HttpProxyClient
    {
        public static RemoteProxy Parse(string proxy)
        {
            return new RemoteProxy() { Type = TypeProxy.HTTP, Proxy = new WebProxy(proxy.Split(':')[0].Trim(), int.Parse(proxy.Split(':')[1].Trim())) };
        }
    }
}
