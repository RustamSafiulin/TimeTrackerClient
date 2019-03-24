
using System;
using System.Net;
using System.Collections.Generic;

namespace TimeTracker.Service
{
    public class Response<TSuccess, TError>
    {
        public TSuccess SuccessBody { get; set; }

        public TError ErrorBody { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public Dictionary<string, string> CookieStore { get; set; }

        public Response()
        {
            CookieStore = new Dictionary<string, string>();
        }
    }

    public class DownloadResponse<TError>
    {
        public byte[] SuccessBody { get; set; }

        public TError ErrorBody { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public Dictionary<string, string> CookieStore { get; set; }

        public DownloadResponse()
        {
            CookieStore = new Dictionary<string, string>();
        }
    }
}
