using System.Net;
namespace SpeedRequest
{
    public class Socks4ProxyClient
    {
        public static RemoteProxy Parse(string proxy)
        {
            return new RemoteProxy() { Type = TypeProxy.SOCKS4, Proxy = new WebProxy("socks4://" + proxy.Split(':')[0].Trim(), int.Parse(proxy.Split(':')[1].Trim())) };
        }
    }
}
