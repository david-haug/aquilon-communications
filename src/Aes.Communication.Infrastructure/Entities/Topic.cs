using System;
using System.Collections.Generic;
using System.Text;

namespace Aes.Communication.Infrastructure.Entities
{
    public class Topic
    {
        public string Title { get; set; }
        public IDictionary<string, string> Attributes { get; set; }
    }
}
