using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aes.Communication.Api.Models;
using Aes.Communication.Api.Models.Requests;
using Aes.Communication.Application.Conversations;

namespace Aes.Communication.Api.Conversations
{
    /// <summary>
    /// Request object to create new conversation.
    /// </summary>
    public class CreateConversationModel
    {
        /// <summary>
        /// (required) Business entity that the conversation is about. 
        /// </summary>
        public EntityIdModel Subject { get; set; }
        /// <summary>
        /// (required) Object representing the conversation's title and metadata (attributes).
        /// </summary>
        public TopicModel Topic { get; set; }
        /// <summary>
        /// (required) Business entity, usually a tieout, used to group related conversations. Can be the same as the subject.
        /// </summary>
        public EntityIdModel Parent { get; set; }
        /// <summary>
        /// (required) User id of conversation creator 
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// (required) Organization id of conversation creator 
        /// </summary>
        public int OrganizationId { get; set; }
        /// <summary>
        /// (required) Organization id of the second party in the conversation
        /// </summary>
        public int CounterpartyId { get; set; }
        /// <summary>
        /// (optional) Messages in the conversation. Messages are typically added to a previously created conversation.
        /// </summary>
        public IEnumerable<AddMessageModel> Messages { get; set; }

        public static CreateConversationRequest Map(CreateConversationModel model)
        {
            return new CreateConversationRequest
            {
                Subject = EntityIdModel.Map(model.Subject),
                Parent = EntityIdModel.Map(model.Parent),
                Title = model.Topic?.Title,
                HeaderAttributes = model.Topic?.Attributes,
                CreatedByUserId = model.UserId,
                OrganizationId = model.OrganizationId,
                CounterpartyId = model.CounterpartyId,
                Messages = model.Messages?.Select(m => AddMessageModel.Map(m, null))
            };
        }
    }
}
