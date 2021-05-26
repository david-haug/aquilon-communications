using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aes.Communication.Application.Validation;

namespace Aes.Communication.Api.Models
{
    public class ValidationErrorModel
    {
        public int Status { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public IEnumerable<ValidationError> Errors { get; set; }
    }
}
