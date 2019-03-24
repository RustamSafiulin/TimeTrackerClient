
using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace TimeTracker.Models
{
    public class EmptyRequestBodyDto : IDto
    {
        #region Props and Fields
        
        public Dictionary<string, string> ToParamsQuery()
        {
            throw new NotImplementedException();
        }

        #endregion

        public EmptyRequestBodyDto()
        { }
    }
}
