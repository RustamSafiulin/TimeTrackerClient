
using System;

using Newtonsoft.Json;

namespace TimeTracker.Models
{
    public class ErrorMsgDto
    {
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        public ErrorMsgDto()
        { }
    }
}