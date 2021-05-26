using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aes.Communication.Api.Models;
using Aes.Communication.Application.Conversations.GetConversations;
using Aes.Communication.Domain.Messages;

namespace Aes.Communication.Api.Conversations
{
    /// <summary>
    /// Request object to return a list of conversations matching provided criteria.
    /// </summary>
    public class GetConversationsModel
    {
        //public string View { get; set; }
        /// <summary>
        /// Id of the business object that the conversation is about.  Required when SubjectType used.
        /// </summary>
        public string SubjectId { get; set; }
        /// <summary>
        /// Type of business object (invoice,dispute,tieout, or deal). Required when SubjectId used.
        /// </summary>
        public string SubjectType { get; set; }
        /// <summary>
        /// Id of the business object (usually a tieout) used to group related conversations. Required when ParentType used.
        /// </summary>
        public string ParentId { get; set; }
        /// <summary>
        /// Type of business object (invoice,dispute,tieout, or deal). Required when ParentId used.
        /// </summary>
        public string ParentType { get; set; }

        public static GetConversationsRequest Map(GetConversationsModel model)
        {
            //use EntityId model to map entity type
            EntityId subject = null;
            //both id & type required 
            if (!string.IsNullOrWhiteSpace(model.SubjectId) &&
                !string.IsNullOrWhiteSpace(model.SubjectType))
            {
                var sId = new EntityIdModel
                {
                    Id = model.SubjectId,
                    Type = model.SubjectType
                };

                subject = EntityIdModel.Map(sId);
            }

            EntityId parent = null;
            //both id & type required 
            if (!string.IsNullOrWhiteSpace(model.ParentId) &&
                !string.IsNullOrWhiteSpace(model.ParentType))
            {
                var pId = new EntityIdModel
                {
                    Id = model.ParentId,
                    Type = model.ParentType
                };

                parent = EntityIdModel.Map(pId);
            }

            return new GetConversationsRequest
            {
                Subject = subject,
                Parent = parent
            };
        }
    }
}
