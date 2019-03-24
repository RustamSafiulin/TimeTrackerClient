
using RestSharp;

namespace TimeTracker.Service
{
    public class Options
    {
        public RestSharp.DataFormat ExchangeFormat { get; set; }
        public string BaseAddress { get; set; }

        public Options()
        { }
    }
}
