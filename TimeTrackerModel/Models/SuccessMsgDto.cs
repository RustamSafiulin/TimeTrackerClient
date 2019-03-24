
using System;

using Newtonsoft.Json;

namespace TimeTracker.Models
{
    public class SuccessMsgDto
    {
        [JsonProperty(PropertyName = "msg")]
        public string Msg { get; set; }

        public SuccessMsgDto()
        { }
    }
}