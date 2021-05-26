using System;
using System.Collections.Generic;
using System.Text;

namespace Aes.Communication.Application.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException()
        {

        }

        public BadRequestException(string message) : base(message)
        {

        }
    }

}
