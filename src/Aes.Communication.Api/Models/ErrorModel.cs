using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aes.Communication.Api.Models
{
    public class ErrorModel
    {
        public int Status { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
