
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TimeTracker.Models
{
    public class ProfileDto : IDto
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }

        public ProfileDto()
        { }

        public Dictionary<string, string> ToParamsQuery()
        {
            throw new NotImplementedException();
        }
    }
}
