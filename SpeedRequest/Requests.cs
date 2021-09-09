using System.Collections.Generic;
namespace SpeedRequest
{
    public interface Requests
    {
        public string UserAgent { get; set; }
        public string Accept { get; set; }
        public string Connection { get; set; }
        public string Host { get; set; }
        public string Referer { get; set; }
        public List<Headers> Headers { get; set; }
    }
}
