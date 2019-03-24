
using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace TimeTracker.Models
{
    public class SettingDto
    {
        [JsonProperty(PropertyName = "profile_id")]
        public string ProfileId { get; set; }

        [JsonProperty(PropertyName = "activity_categories")]
        public List<string> ActivityCategories { get; set; }

        [JsonProperty(PropertyName = "tracked_sites")]
        public List<string> TrackedSites { get; set; }

        [JsonProperty(PropertyName = "notify_need_start")]
        public bool NotificationNeedStart { get; set; }

        [JsonProperty(PropertyName = "enable_sound_notify")]
        public bool EnableSoundNotify { get; set; }

        [JsonProperty(PropertyName = "enable_popup_notify")]
        public bool EnablePopupNotify { get; set; }

        [JsonProperty(PropertyName = "notify_need_finish")]
        public bool NotificationNeedFinish { get; set; }

        public SettingDto()
        { }
    }
}