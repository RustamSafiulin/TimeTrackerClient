
using System;

using Newtonsoft.Json;

namespace TimeTracker.Models
{
    public class SessionInfoDto
    {
        [JsonProperty(PropertyName = "profile_id")]
        public string ProfileId { get; set; }

        [JsonProperty(PropertyName = "session_id")]
        public string SessionId { get; set; }

        public SessionInfoDto()
        { }
    }
}
