using System.Collections.Generic;
namespace SpeedRequest
{
    internal class RemoteRequests : Requests
    {
        
        public string UserAgent { get; set; }
        public string Accept { get; set; }
        public string Connection { get; set; }
        public string Host { get; set; }
        public string Referer { get; set; }
        public bool AllowAutoRedirect { get; set; }
        public bool IgnoreProtocolErrors { get; set; }
        public int Timeout { get; set; }
        public int ContinueTimeout { get; set; }
        public int ReadWriteTimeout { get; set; }
        public bool KeepAlive { get; set; }
        public RemoteProxy Proxy { get; set; }
        public Headers[] Headers { get; set; }
        public void AddHeaders(string name, string value)
        {
            List<Headers> headers = new List<Headers>();
            foreach (Headers header in Headers)
            {
                headers.Add(header);
            }
            headers.Add(new Headers() { Name = name, Value = value });
            Headers = headers.ToArray();
        }
    }
}
