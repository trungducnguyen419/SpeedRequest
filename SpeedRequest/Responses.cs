namespace SpeedRequest
{
    public interface Responses
    {
        public bool Error { get; }
        public string StatusCode { get; }
        public string Timeout { get; }
        public string ErrorMessage { get; }
        public string Size { get; }
        public Cookies[] Cookies { get; }
        public Headers[] HeadersResponse { get; }
    }
}
