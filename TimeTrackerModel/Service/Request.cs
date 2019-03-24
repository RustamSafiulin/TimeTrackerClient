
using System;
using System.Collections.Generic;

namespace TimeTracker.Service
{
    public class Request<T>
    {
        public T Body { get; set; }

        public Dictionary<string, string> UrlSegments { get; set; }

        public KeyValuePair<string, string> AuthToken { get; set; }

        public OperationType OpName { get; set; }

        public Request()
        { }
    }
}
