﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Diagnostics;
namespace SpeedRequest
{
    public class SpeedRequest
    {
        private string statusCode { get; set; }
        private bool error { get; set; }
        private string timeout { get; set; }
        private string errormessage { get; set; }
        private string size { get; set; }
        private List<Headers> headersresponse { get; set; }
        Stopwatch stopwatch = new Stopwatch();
        public SpeedRequest() { }

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
        public Responses Responses()
        {
            return new RemoteResponses(statusCode, error, timeout, errormessage, size, headersresponse);
        }
        public Requests Requests()
        {
            return new RemoteRequests();
        }
        private void SetHeadersResponse(HttpWebResponse response)
        {
            if (headersresponse != null) headersresponse = null;
            headersresponse = new List<Headers>();
            foreach (string key in response.Headers.AllKeys)
            {
                headersresponse.Add(new Headers() { Name = key, Value = response.Headers[key] });
            }    
        }
        private void DefaultHeaders(HttpWebRequest request, Method method, string contentType)
        {
            request.Timeout = -1;
            request.ReadWriteTimeout = -1;
            request.ContinueTimeout = -1;
            request.AllowAutoRedirect = true;
            request.CookieContainer = new CookieContainer();
            if (Requests().UserAgent != null) request.UserAgent = Requests().UserAgent;
            if (Requests().Accept != null) request.Accept = Requests().Accept;
            if (Requests().Connection != null) request.Connection = Requests().Connection;
            if (contentType != null) request.ContentType = contentType;
            else request.ContentType = "application/x-www-form-urlencoded";
            if (Requests().Host != null) request.Host = Requests().Host;
            if (Requests().Referer != null) request.Referer = Requests().Referer;
            if (Requests().Headers != null)
            {
                foreach (Headers headers in Requests().Headers)
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
            var response = (HttpWebResponse)ex.Response;
            if (response != null)
            {
                statusCode = response.StatusCode.ToString();
                error = true;
                errormessage = ex.Message.ToString();
                timeout = stopwatch.ElapsedMilliseconds + " ms";
                SetHeadersResponse(response);
                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                if (responseString != null) size = SizeSuffix(responseString.Length);
                return responseString;
            }
            return null;
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
            SetHeadersResponse(response);
            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            if (responseString != null) size = SizeSuffix(responseString.Length);
            return responseString;
        }
        public string RequestUrl(string url, Method method, string postData, string contentType = null, MultipartContent multipartContent = null)
        {
            if (contentType == null) contentType = "";
            stopwatch.Reset();
            stopwatch.Start();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.DefaultConnectionLimit = 256;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;
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
                else if (postData != null)
                {
                    var data = Encoding.UTF8.GetBytes(postData);
                    request.ContentLength = data.Length;
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
                var response = (HttpWebResponse)request.GetResponse();
                return SuccessResponse(response);
            }
            catch (WebException ex)
            {
                return WebExceptionResponse(ex);
            }
            //catch (Exception ex)
            //{
            //    throw new Exception(ExceptionResponse(ex));
            //}
        }
    }
}
