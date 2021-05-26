using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aes.Communication.Domain.ValueObjects;


namespace Aes.Communication.Api.Conversations.MarkAsRead
{
    public class MarkAsReadModel
    {
        public User User { get; set; }
        public int OrganizationId { get; set; }
    }
}
