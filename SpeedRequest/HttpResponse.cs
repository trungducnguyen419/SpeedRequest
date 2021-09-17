using System;
using System.IO;
using System.Net;
namespace SpeedRequest
{
    public class HttpResponse
    {
        private string statusCode { get; set; }
        private bool error { get; set; }
        private string timeout { get; set; }
        private string errormessage { get; set; }
        private string size { get; set; }
        private Cookies[] cookies { get; set; }
        private Headers[] headersresponse { get; set; }
        private string tostring { get; set; }
        private HttpWebResponse httpWebResponse { get; set; }
        public HttpResponse(string statusCode, bool error, string timeout, string errormessage, string size, Cookies[] cookies, Headers[] headersresponse, string tostring, HttpWebResponse httpWebResponse)
        {
            this.statusCode = statusCode;
            this.error = error;
            this.timeout = timeout;
            this.errormessage = errormessage;
            this.headersresponse = headersresponse;
            this.cookies = cookies;
            this.tostring = tostring;
            this.httpWebResponse = httpWebResponse;
        }
        public bool Error { get { return error; } }
        public string StatusCode { get { return statusCode; } }
        public string Timeout { get { return timeout; } }
        public string ErrorMessage { get { return errormessage; } }
        public string Size { get { return size; } }
        public Cookies[] Cookies { get { return cookies; } }
        public Headers[] HeadersResponse { get { return headersresponse; } }
        public override string ToString() => tostring;
        private readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        private string SizeSuffix(long value, int decimalPlaces = 0)
        {
            if (value < 0)
            {
                throw new ArgumentException("Bytes should not be negative", "value");
            }
            var mag = (int)Math.Max(0, Math.Log(value, 1024));
            var adjustedSize = Math.Round(value / Math.Pow(1024, mag), decimalPlaces);
            return String.Format("{0} {1}", adjustedSize, SizeSuffixes[mag]);
        }
        public void ToFile(string filename)
        {
            try
            {
                byte[] buffer = new byte[1024];
                FileStream fileStream = File.OpenWrite(filename);
                using (Stream input = httpWebResponse.GetResponseStream())
                {
                    size = SizeSuffix(input.Length);
                    int size_ = input.Read(buffer, 0, buffer.Length);
                    while (size_ > 0)
                    {
                        fileStream.Write(buffer, 0, size_);
                        size_ = input.Read(buffer, 0, buffer.Length);
                    }
                }
                fileStream.Flush();
                fileStream.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
    }
}
