using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aes.Communication.Api.Logging
{
    public interface ILogger
    {
        void Log(LogEntry logEntry);
    }
}
