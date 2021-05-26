using System;
using System.Collections.Generic;
using System.Text;

namespace Aes.Communication.Application.Validation
{
    public class ValidatorHelpers
    {
        public static bool IsValidEntityType(string type)
        {
            var validTypes = new List<string> { "tieout", "invoice", "dispute", "deal" };
            return validTypes.Contains(type);
        }
    }
}
