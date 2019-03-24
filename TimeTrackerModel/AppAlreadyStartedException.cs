using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker
{
    public class AppAlreadyStartedException : Exception
    {
        public AppAlreadyStartedException(String message)
            : base(message)
        { }
    }
}
