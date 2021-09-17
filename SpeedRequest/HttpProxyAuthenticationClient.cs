using System;
using System.Net;
namespace SpeedRequest
{
    public class HttpProxyAuthenticationClient
    {
        public static Proxy Parse(string proxyAddress, int port, string username, string password)
        {
            return new Proxy() { Type = TypeProxy.HTTPAUTH, WebProxy = new WebProxy() { Address = new Uri(proxyAddress + ":" + port), Credentials = new NetworkCredential(username, password) } };
        }
    }
}
