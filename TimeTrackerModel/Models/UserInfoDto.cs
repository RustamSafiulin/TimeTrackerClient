
using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace TimeTracker.Models
{
    public interface IDto
    {
        Dictionary<string, string> ToParamsQuery(); 
    }

    public class UserInfoDto : IDto
    {
        #region Props and Fields

        [JsonProperty(PropertyName = "profile_id")]
        public string ProfileId { get; set; }

        public Dictionary<string, string> ToParamsQuery()
        {
            var result = new Dictionary<string, string>();
            result.Add("profile_id", ProfileId);
            return result;
        }

        #endregion

        public UserInfoDto()
        { }
    }
}