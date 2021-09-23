using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Diagnostics;
namespace SpeedRequest
{
    public class HttpRequest
    {
        public HttpRequest()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.DefaultConnectionLimit = 256;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;
        }
        private string statusCode { get; set; }
        private bool error { get; set; }
        private string timeout { get; set; }
        private string errormessage { get; set; }
        private string size { get; set; }
        private Cookies[] cookies { get; set; }
        private Headers[] headersresponse { get; set; }
        /////////////////////////////////////////////////
        public string UserAgent { get; set; }
        public string Accept { get; set; }
        public string Connection { get; set; }
        public string Host { get; set; }
        public string Referer { get; set; }
        public string ContentType { get; set; }
        public bool AllowAutoRedirect { get; set; }
        public bool IgnoreProtocolErrors { get; set; }
        public int Timeout { get; set; }
        public int ContinueTimeout { get; set; }
        public int ReadWriteTimeout { get; set; }
        public bool KeepAlive { get; set; }
        public Proxy Proxy { get; set; }
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
        Stopwatch stopwatch = new Stopwatch();
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
        private void SetHeadersResponse(HttpWebResponse response)
        {
            List<Headers> listheaders = new List<Headers>();
            foreach (string key in response.Headers.AllKeys)
            {
                listheaders.Add(new Headers() { Name = key, Value = response.Headers[key] });
            }
            headersresponse = listheaders.ToArray();
        }
        private void DefaultHeaders(HttpWebRequest request, Method method, string contentType)
        {
            request.Timeout = -1;
            request.ReadWriteTimeout = -1;
            request.ContinueTimeout = -1;
            request.CookieContainer = new CookieContainer();
            if (UserAgent != null) request.UserAgent = UserAgent;
            if (Accept != null) request.Accept = Accept;
            if (Connection != null) request.Connection = Connection;
            if (contentType != null) request.ContentType = contentType;
            else if (ContentType != null) request.ContentType = ContentType;
            else request.ContentType = "application/x-www-form-urlencoded";
            if (Host != null) request.Host = Host;
            if (Referer != null) request.Referer = Referer;
            request.AllowAutoRedirect = AllowAutoRedirect;
            if (Timeout != 0) request.Timeout = Timeout;
            if (ContinueTimeout != 0) request.ContinueTimeout = ContinueTimeout;
            if (ReadWriteTimeout != 0) request.ReadWriteTimeout = ReadWriteTimeout;
            request.KeepAlive = KeepAlive;
            if (Proxy.WebProxy != null) request.Proxy = Proxy.WebProxy;
            if (Headers != null)
            {
                foreach (Headers headers in Headers)
                {
                    request.Headers[headers.Name] = headers.Value;
                }
            }
            if (method != 0) request.Method = method.ToString();
            else request.Method = Method.GET.ToString();
        }
        private string WebExceptionResponse(WebException ex)
        {
            stopwatch.Stop();
            if (IgnoreProtocolErrors == false) throw new Exception(ex.Message.ToString());
            var response = (HttpWebResponse)ex.Response;
            if (response != null)
            {
                statusCode = response.StatusCode.ToString();
                error = true;
                errormessage = ex.Message.ToString();
                timeout = stopwatch.ElapsedMilliseconds + " ms";
                List<Cookies> list_cookies = new List<Cookies>();
                foreach (Cookie cok in response.Cookies)
                {
                    list_cookies.Add(new Cookies() { Name = cok.Name, Value = cok.Value, Domain = cok.Domain, Path = cok.Path, Port = cok.Port, Secure = cok.Secure, TimeStamp = cok.TimeStamp, Expires = cok.Expires, Expired = cok.Expired, Discard = cok.Discard, Comment = cok.Comment, CommentUri = cok.CommentUri, Version = cok.Version });
                }
                cookies = list_cookies.ToArray();
                SetHeadersResponse(response);
                string HttpResponsetring = new StreamReader(response.GetResponseStream()).ReadToEnd();
                if (HttpResponsetring != null) size = SizeSuffix(HttpResponsetring.Length);
                return HttpResponsetring;
            }
            throw new Exception(ex.Message.ToString());
        }
        private string ExceptionResponse(Exception ex)
        {
            stopwatch.Stop();
            errormessage = ex.Message.ToString();
            error = true;
            return ex.Message.ToString();
        }
        private string SuccessResponse(HttpWebResponse response)
        {
            stopwatch.Stop();
            statusCode = (int)response.StatusCode + " " + response.StatusCode.ToString();
            error = false;
            timeout = stopwatch.ElapsedMilliseconds + " ms";
            List<Cookies> list_cookies = new List<Cookies>();
            foreach (Cookie cok in  response.Cookies)
            {
                list_cookies.Add(new Cookies() { Name = cok.Name, Value = cok.Value, Domain = cok.Domain, Path = cok.Path, Port = cok.Port, Secure = cok.Secure, TimeStamp = cok.TimeStamp, Expires = cok.Expires, Expired = cok.Expired, Discard = cok.Discard, Comment = cok.Comment, CommentUri = cok.CommentUri, Version = cok.Version });
            }
            cookies = list_cookies.ToArray();
            SetHeadersResponse(response);
            string HttpResponsetring = new StreamReader(response.GetResponseStream()).ReadToEnd();
            if (HttpResponsetring != null) size = SizeSuffix(HttpResponsetring.Length);
            return HttpResponsetring;
        }
        public HttpResponse Post(string url, Method method, string contentType, string postData)
        {
            stopwatch.Reset();
            stopwatch.Start();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.DefaultConnectionLimit = 256;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var request = (HttpWebRequest)WebRequest.Create(url);
                DefaultHeaders(request, method, contentType);
                if (postData != null)
                {
                    var data = Encoding.UTF8.GetBytes(postData);
                    request.ContentLength = data.Length;
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
                var response = (HttpWebResponse)request.GetResponse();
                return new HttpResponse(statusCode, error, timeout, errormessage, size, cookies, headersresponse, SuccessResponse(response), response);
            }
            catch (WebException ex)
            {
                return new HttpResponse(statusCode, error, timeout, errormessage, size, cookies, headersresponse, WebExceptionResponse(ex), (HttpWebResponse)ex.Response);
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionResponse(ex));
            }
        }
        public HttpResponse Post(string url, Method method, string contentType, MultipartContent multipartContent)
        {
            stopwatch.Reset();
            stopwatch.Start();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.DefaultConnectionLimit = 256;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var request = (HttpWebRequest)WebRequest.Create(url);
                DefaultHeaders(request, method, contentType);
                if (multipartContent != null)
                {
                    string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
                    request.ContentType = "multipart/form-data; boundary=" + boundary;
                    using (var stream = request.GetRequestStream())
                    {
                        foreach (BytesContent bytesContent in multipartContent.BytesContent)
                        {
                            stream.Write(bytesContent.Content, bytesContent.Offset, bytesContent.Count);
                        }
                    }
                }
                var response = (HttpWebResponse)request.GetResponse();
                return new HttpResponse(statusCode, error, timeout, errormessage, size, cookies, headersresponse, SuccessResponse(response), response);
            }
            catch (WebException ex)
            {
                return new HttpResponse(statusCode, error, timeout, errormessage, size, cookies, headersresponse, WebExceptionResponse(ex), (HttpWebResponse)ex.Response);
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionResponse(ex));
            }
        }
        public HttpResponse Post(string url, Method method, string postData)
        {
            stopwatch.Reset();
            stopwatch.Start();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.DefaultConnectionLimit = 256;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var request = (HttpWebRequest)WebRequest.Create(url);
                DefaultHeaders(request, method, null);
                if (postData != null)
                {
                    var data = Encoding.UTF8.GetBytes(postData);
                    request.ContentLength = data.Length;
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
                var response = (HttpWebResponse)request.GetResponse();
                return new HttpResponse(statusCode, error, timeout, errormessage, size, cookies, headersresponse, SuccessResponse(response), response);
            }
            catch (WebException ex)
            {
                return new HttpResponse(statusCode, error, timeout, errormessage, size, cookies, headersresponse, WebExceptionResponse(ex), (HttpWebResponse)ex.Response);
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionResponse(ex));
            }
        }
        public HttpResponse Post(string url, Method method, MultipartContent multipartContent)
        {
            stopwatch.Reset();
            stopwatch.Start();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.DefaultConnectionLimit = 256;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var request = (HttpWebRequest)WebRequest.Create(url);
                DefaultHeaders(request, method, null);
                if (multipartContent != null)
                {
                    string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
                    request.ContentType = "multipart/form-data; boundary=" + boundary;
                    using (var stream = request.GetRequestStream())
                    {
                        foreach (BytesContent bytesContent in multipartContent.BytesContent)
                        {
                            stream.Write(bytesContent.Content, bytesContent.Offset, bytesContent.Count);
                        }
                    }
                }
                var response = (HttpWebResponse)request.GetResponse();
                return new HttpResponse(statusCode, error, timeout, errormessage, size, cookies, headersresponse, SuccessResponse(response), response);
            }
            catch (WebException ex)
            {
                return new HttpResponse(statusCode, error, timeout, errormessage, size, cookies, headersresponse, WebExceptionResponse(ex), (HttpWebResponse)ex.Response);
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionResponse(ex));
            }
        }
        public HttpResponse Get(string url)
        {
            stopwatch.Reset();
            stopwatch.Start();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.DefaultConnectionLimit = 256;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var request = (HttpWebRequest)WebRequest.Create(url);
                DefaultHeaders(request, 0, null);
                var response = (HttpWebResponse)request.GetResponse();
                return new HttpResponse(statusCode, error, timeout, errormessage, size, cookies, headersresponse, SuccessResponse(response), response);
            }
            catch (WebException ex)
            {
                return new HttpResponse(statusCode, error, timeout, errormessage, size, cookies, headersresponse, WebExceptionResponse(ex), (HttpWebResponse)ex.Response);
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionResponse(ex));
            }
        }
        public HttpResponse Post(Uri url, Method method, string contentType, string postData)
        {
            stopwatch.Reset();
            stopwatch.Start();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.DefaultConnectionLimit = 256;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var request = (HttpWebRequest)WebRequest.Create(url);
                DefaultHeaders(request, method, contentType);
                if (postData != null)
                {
                    var data = Encoding.UTF8.GetBytes(postData);
                    request.ContentLength = data.Length;
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
                var response = (HttpWebResponse)request.GetResponse();
                return new HttpResponse(statusCode, error, timeout, errormessage, size, cookies, headersresponse, SuccessResponse(response), response);
            }
            catch (WebException ex)
            {
                return new HttpResponse(statusCode, error, timeout, errormessage, size, cookies, headersresponse, WebExceptionResponse(ex), (HttpWebResponse)ex.Response);
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionResponse(ex));
            }
        }
        public HttpResponse Post(Uri url, Method method, string contentType, MultipartContent multipartContent)
        {
            stopwatch.Reset();
            stopwatch.Start();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.DefaultConnectionLimit = 256;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var request = (HttpWebRequest)WebRequest.Create(url);
                DefaultHeaders(request, method, contentType);
                if (multipartContent != null)
                {
                    string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
                    request.ContentType = "multipart/form-data; boundary=" + boundary;
                    using (var stream = request.GetRequestStream())
                    {
                        foreach (BytesContent bytesContent in multipartContent.BytesContent)
                        {
                            stream.Write(bytesContent.Content, bytesContent.Offset, bytesContent.Count);
                        }
                    }
                }
                var response = (HttpWebResponse)request.GetResponse();
                return new HttpResponse(statusCode, error, timeout, errormessage, size, cookies, headersresponse, SuccessResponse(response), response);
            }
            catch (WebException ex)
            {
                return new HttpResponse(statusCode, error, timeout, errormessage, size, cookies, headersresponse, WebExceptionResponse(ex), (HttpWebResponse)ex.Response);
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionResponse(ex));
            }
        }
        public HttpResponse Post(Uri url, Method method, string postData)
        {
            stopwatch.Reset();
            stopwatch.Start();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.DefaultConnectionLimit = 256;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var request = (HttpWebRequest)WebRequest.Create(url);
                DefaultHeaders(request, method, null);
                if (postData != null)
                {
                    var data = Encoding.UTF8.GetBytes(postData);
                    request.ContentLength = data.Length;
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
                var response = (HttpWebResponse)request.GetResponse();
                return new HttpResponse(statusCode, error, timeout, errormessage, size, cookies, headersresponse, SuccessResponse(response), response);
            }
            catch (WebException ex)
            {
                return new HttpResponse(statusCode, error, timeout, errormessage, size, cookies, headersresponse, WebExceptionResponse(ex), (HttpWebResponse)ex.Response);
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionResponse(ex));
            }
        }
        public HttpResponse Post(Uri url, Method method, MultipartContent multipartContent)
        {
            stopwatch.Reset();
            stopwatch.Start();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.DefaultConnectionLimit = 256;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var request = (HttpWebRequest)WebRequest.Create(url);
                DefaultHeaders(request, method, null);
                if (multipartContent != null)
                {
                    string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
                    request.ContentType = "multipart/form-data; boundary=" + boundary;
                    using (var stream = request.GetRequestStream())
                    {
                        foreach (BytesContent bytesContent in multipartContent.BytesContent)
                        {
                            stream.Write(bytesContent.Content, bytesContent.Offset, bytesContent.Count);
                        }
                    }
                }
                var response = (HttpWebResponse)request.GetResponse();
                return new HttpResponse(statusCode, error, timeout, errormessage, size, cookies, headersresponse, SuccessResponse(response), response);
            }
            catch (WebException ex)
            {
                return new HttpResponse(statusCode, error, timeout, errormessage, size, cookies, headersresponse, WebExceptionResponse(ex), (HttpWebResponse)ex.Response);
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionResponse(ex));
            }
        }
        public HttpResponse Get(Uri url)
        {
            stopwatch.Reset();
            stopwatch.Start();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.DefaultConnectionLimit = 256;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var request = (HttpWebRequest)WebRequest.Create(url);
                DefaultHeaders(request, 0, null);
                var response = (HttpWebResponse)request.GetResponse();
                return new HttpResponse(statusCode, error, timeout, errormessage, size, cookies, headersresponse, SuccessResponse(response), response);
            }
            catch (WebException ex)
            {
                return new HttpResponse(statusCode, error, timeout, errormessage, size, cookies, headersresponse, WebExceptionResponse(ex), (HttpWebResponse)ex.Response);
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionResponse(ex));
            }
        }
    }
}
