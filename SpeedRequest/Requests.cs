namespace SpeedRequest
{
    public interface Requests
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
        public void AddHeaders(string name, string value);
    }
}
