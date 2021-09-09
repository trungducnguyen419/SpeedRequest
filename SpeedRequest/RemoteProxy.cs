using System.Net;
namespace SpeedRequest
{
    public struct RemoteProxy
    {
        public TypeProxy Type { get; set; }
        public WebProxy Proxy {  get; set; }
    }
}
