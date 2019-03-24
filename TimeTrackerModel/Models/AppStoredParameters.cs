
using System;

namespace TimeTracker.Models
{
    [Serializable]
    public class AppStoredParameters
    {
        public AppStoredParameters()
        { }

        public bool RememberMe { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string ServiceBaseAddress { get; set; }
    }
}
