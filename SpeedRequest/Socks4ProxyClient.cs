using System.Net;
namespace SpeedRequest
{
    public class Socks4ProxyClient
    {
        public static Proxy Parse(string proxy)
        {
            return new Proxy() { Type = TypeProxy.SOCKS4, WebProxy = new WebProxy("socks4://" + proxy.Split(':')[0].Trim(), int.Parse(proxy.Split(':')[1].Trim())) };
        }
    }
}
