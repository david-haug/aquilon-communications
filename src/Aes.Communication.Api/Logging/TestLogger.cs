using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Aes.Communication.Api.Logging
{
    public class TestLogger:ILogger
    {
        public void Log(LogEntry logEntry)
        {
            Console.WriteLine($"LOGGED -- {JsonConvert.SerializeObject(logEntry.Parameters)}");
        }
    }
}
