using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aes.Communication.Api.Logging
{
    public class LogEntry
    {
        public DateTime LogDate { get; set; }
        public dynamic Parameters { get; set; }
        public string Route { get; set; }
        

    }

}
