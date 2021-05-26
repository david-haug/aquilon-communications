using System;
using System.Collections.Generic;
using System.Text;

namespace Aes.Communication.Domain.Conversations
{
    public class ConversationUserFlag:IEntity
    {
        public ConversationUserFlag(Guid conversationId, int userId, DateTime dateCreated)
        {
            ConversationId = conversationId;
            UserId = userId;
            DateCreated = dateCreated;
        }

        public Guid ConversationId { get; set; }
        public int UserId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
