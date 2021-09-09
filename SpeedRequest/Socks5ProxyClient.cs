using System.Net;
namespace SpeedRequest
{
    public class Socks5ProxyClient
    {
        public static RemoteProxy Parse(string proxy)
        {
            return new RemoteProxy() { Type = TypeProxy.SOCKS5, Proxy = new WebProxy("socks5://" + proxy.Split(':')[0].Trim(), int.Parse(proxy.Split(':')[1].Trim())) };
        }
    }
}
