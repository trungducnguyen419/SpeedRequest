using System.Net;
namespace SpeedRequest
{
    public struct Proxy
    {
        public TypeProxy Type { get; set; }
        public WebProxy WebProxy {  get; set; }
    }
}
