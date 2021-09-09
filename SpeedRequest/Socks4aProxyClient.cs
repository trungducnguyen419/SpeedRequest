using System.Net;
namespace SpeedRequest
{
    public class Socks4aProxyClient
    {
        public static RemoteProxy Parse(string proxy)
        {
            return new RemoteProxy() { Type = TypeProxy.SOCKS4A, Proxy = new WebProxy("socks4a://" + proxy.Split(':')[0].Trim(), int.Parse(proxy.Split(':')[1].Trim())) };
        }
    }
}
