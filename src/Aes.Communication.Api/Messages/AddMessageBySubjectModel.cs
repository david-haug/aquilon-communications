using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aes.Communication.Api.Models;
using Aes.Communication.Application.Conversations.AddMessageByConversationSubject;
using Aes.Communication.Domain.ValueObjects;

namespace Aes.Communication.Api.Messages
{
    public class AddMessageBySubjectModel
    {
        /// <summary>
        /// Indicates what object the message is about
        /// </summary>
        /// <remarks>Required</remarks>
        public EntityIdModel ConversationSubject { get; set; }

        /// <summary>
        /// Contents of the message (required) 
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Indicates if a message can be viewed by the organization and counterparty (true) or just the organization (false)
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// Creator of the message (required) 
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Organization of the creator (required) 
        /// </summary>
        public Organization Organization { get; set; }

        public IEnumerable<Attachment> Attachments { get; set; }

        /// <summary>
        /// Indicates what object the message is about.  If excluded, the Conversation subject will be used. For example, a message about a deal can be included in an invoice conversation.
        /// </summary>
        public EntityIdModel MessageSubject { get; set; }

        public static AddMessageByConversationSubjectRequest Map(AddMessageBySubjectModel model)
        {
            return new AddMessageByConversationSubjectRequest
            {
                ConversationSubject = model.ConversationSubject != null ? EntityIdModel.Map(model.ConversationSubject): null,
                Body = model.Body,
                IsPublic = model.IsPublic,
                User = model.User,
                Organization = model.Organization,
                Attachments = model.Attachments,
                MessageSubject = model.MessageSubject != null ? EntityIdModel.Map(model.MessageSubject) : null
            };
        }
    }
}
