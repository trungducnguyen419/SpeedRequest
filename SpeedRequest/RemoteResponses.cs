namespace SpeedRequest
{
    internal class RemoteResponses : Responses
    {
        private string statusCode { get; set; }
        private bool error { get; set; }
        private string timeout { get; set; }
        private string errormessage { get; set; }
        private string size { get; set; }
        private Cookies[] cookies { get; set; }
        private Headers[] headersresponse { get; set; }
        public RemoteResponses(string statusCode, bool error, string timeout, string errormessage, string size, Cookies[] cookies, Headers[] headersresponse)
        {
            this.statusCode = statusCode;
            this.error = error;
            this.timeout = timeout;
            this.errormessage = errormessage;
            this.headersresponse = headersresponse;
            this.cookies = cookies;
        }
        public bool Error { get { return error; } }
        public string StatusCode { get { return statusCode; } }
        public string Timeout { get { return timeout; } }
        public string ErrorMessage { get { return errormessage; } }
        public string Size { get { return size; } }
        public Cookies[] Cookies { get { return cookies; } }
        public Headers[] HeadersResponse { get { return headersresponse; } }
    }
}
