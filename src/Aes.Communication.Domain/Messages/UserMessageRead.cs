using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Domain.ValueObjects;

namespace Aes.Communication.Domain.Messages
{
    public class UserMessageRead
    {
        public UserMessageRead(User user, int organizationId, DateTime dateRead)
        {
            User = user;
            OrganizationId = organizationId;
            DateRead = dateRead;
        }
        public User User { get; }
        public int OrganizationId { get; }
        public DateTime DateRead { get; }
    }
}
