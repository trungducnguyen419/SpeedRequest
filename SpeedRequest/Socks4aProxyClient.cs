using System.Net;
namespace SpeedRequest
{
    public class Socks4aProxyClient
    {
        public static Proxy Parse(string proxy)
        {
            return new Proxy() { Type = TypeProxy.SOCKS4A, WebProxy = new WebProxy("socks4a://" + proxy.Split(':')[0].Trim(), int.Parse(proxy.Split(':')[1].Trim())) };
        }
    }
}
