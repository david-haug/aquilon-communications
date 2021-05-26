using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Aes.Communication.Api.Conversations;
using Aes.Communication.Application.Conversations;
using Aes.Communication.Domain.Messages;
using Aes.Communication.Domain.ValueObjects;

namespace Aes.Communication.Api.Models.Requests
{
    /// <summary>
    /// Model for adding a message to an existing conversation
    /// </summary>
    
    public class AddMessageModel
    {
        /// <summary>
        /// Contents of the message (required) 
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Indicates if a message can be viewed by the organization and counterparty (true) or just the organization (false)
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// Indicates what object the message is about
        /// </summary>
        /// <remarks>If excluded, the Conversation subject will be used. For example, a message about a deal can be included in an invoice conversation.</remarks>
        public EntityIdModel Subject { get; set; }

        /// <summary>
        /// Creator of the message (required) 
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Organization of the creator (required) 
        /// </summary>
        public Organization Organization { get; set; }

        /// <summary>
        /// Files attached to the message.
        /// </summary>
        public IEnumerable<Attachment> Attachments { get; set; }

        public static AddConversationMessageRequest Map(AddMessageModel request, string conversationId)
        {
            return new AddConversationMessageRequest
            {
                ConversationId = conversationId,
                Body = request.Body,
                Subject = request.Subject != null ? EntityIdModel.Map(request.Subject) : null,
                IsPublic = request.IsPublic,
                User = request.User,
                Organization = request.Organization,
                Attachments = request.Attachments
            };

        }
    }
}
