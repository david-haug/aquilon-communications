using System;
using System.Collections.Generic;
using System.Text;

namespace Aes.Communication.Application.Validation
{
    public class ValidationError
    {
        public ValidationError()
        {

        }

        public ValidationError(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public ValidationError(string code, string message, string field, string[] path)
        {
            Code = code;
            Message = message;
            Field = field;
            Path = path;
        }

        public string Code { get; set; }
        public string Message { get; set; }
        public string Field { get; set; }
        public string[] Path { get; set; }
    }
}
