using System.Net;
namespace SpeedRequest
{
    public class Socks5ProxyClient
    {
        public static Proxy Parse(string proxy)
        {
            return new Proxy() { Type = TypeProxy.SOCKS5, WebProxy = new WebProxy("socks5://" + proxy.Split(':')[0].Trim(), int.Parse(proxy.Split(':')[1].Trim())) };
        }
    }
}
