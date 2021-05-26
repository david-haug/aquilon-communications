using System;
using System.Collections.Generic;
using System.Text;

namespace Aes.Communication.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {

        }
    }
}
