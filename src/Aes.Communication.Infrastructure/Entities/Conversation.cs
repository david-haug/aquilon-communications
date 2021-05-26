using System;
using System.Collections.Generic;
using System.Text;

namespace Aes.Communication.Infrastructure.Entities
{
    public class Conversation
    {
        public Guid Id { get; set; }

        public EntityId Subject { get; set; }

        public IEnumerable<Message> Messages { get; set; }

        public DateTime DateCreated { get; set; }
        public int CreatedByUserId { get; set; }
        public Topic Topic { get; set; }

        public EntityId Parent { get; set; }
        public int OrganizationId { get; set; }
        public int CounterpartyId { get; set; }

    }
}
