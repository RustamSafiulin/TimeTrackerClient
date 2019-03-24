
using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace TimeTracker.Models
{
    public class WorkIntervalDto
    {
        #region Props and Fields

        [JsonProperty(PropertyName = "begin")]
        public Int64 Begin { get; set; }

        [JsonProperty(PropertyName = "end")]
        public Int64 End { get; set; }

        #endregion

        public WorkIntervalDto()
        { }
    }

    public class ActivityDto : IDto
    {
        #region Props and Fields

        [JsonProperty(PropertyName = "id")]
        public string Id { get;set; }
        
        [JsonProperty(PropertyName = "profile_id")]
        public string ProfileId { get; set; }
        
        [JsonProperty(PropertyName = "is_started")]
        public Boolean IsStarted { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    
        [JsonProperty(PropertyName = "created_at")]
        public Int64 CreatedAt { get; set; }
        
        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }
        
        //actual begin time
        [JsonProperty(PropertyName = "begin_time")]
        public Int64 BeginTime { get; set; }

        //planned begin time
        [JsonProperty(PropertyName = "planned_begin_time")]
        public Int64 PlannedBeginTime { get; set; }
        
        //[JsonProperty(PropertyName = "expected_duration")]
        //public Int64 ExpectedDuration { get; set; }
        
        //duration (without use expected duration)
        [JsonProperty(PropertyName = "actual_duration")]
        public Int64 ActualDuration { get; set; }
        
        [JsonProperty(PropertyName = "work_intervals")]
        public List<WorkIntervalDto> WorkIntervals { get; set; }

        #endregion

        public ActivityDto()
        { }

        public Dictionary<string, string> ToParamsQuery()
        {
            throw new NotImplementedException();
        }
    }
}
