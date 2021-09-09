using System;
using System.Net;
namespace SpeedRequest
{
    public class HttpProxyAuthenticationClient
    {
        public static RemoteProxy Parse(string proxyAddress, int port, string username, string password)
        {
            return new RemoteProxy() { Type = TypeProxy.HTTPAUTH, Proxy = new WebProxy() { Address = new Uri(proxyAddress + ":" + port), Credentials = new NetworkCredential(username, password) } };
        }
    }
}
