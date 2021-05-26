using System;
using System.Collections.Generic;
using System.Text;

namespace Aes.Communication.Domain.Messages
{
    public enum MessageEntityType
    {
        Unknown = 0,
        TieOut = 1,
        Invoice = 2,
        Dispute = 3,
        Deal = 4,
    }
}
