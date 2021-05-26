using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aes.Communication.Api.Logging
{
    public class Logger:ILogger
    {
        private ILogRepository _repository;
        public Logger(ILogRepository repository)
        {
            _repository = repository;
        }
        public void Log(LogEntry logEntry)
        {
           _repository.Save(logEntry);
        }

    }
}
