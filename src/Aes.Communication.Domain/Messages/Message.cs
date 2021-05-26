using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Aes.Communication.Domain.Conversations;
using Aes.Communication.Domain.ValueObjects;

namespace Aes.Communication.Domain.Messages
{
    public class Message:IEntity
    {
        public Guid Id { get; private set; }
        public Guid ConversationId { get; private set; }
        public string Body { get; private set; }
        public bool IsPublic { get; private set; }
        public DateTime DateCreated { get; private set; }
        private List<UserMessageRead> _messageReads = new List<UserMessageRead>();
        public ReadOnlyCollection<UserMessageRead> ReadByUsers => _messageReads.AsReadOnly();
        public EntityId Subject { get; private set; }
        public Organization Organization { get; private set; }
        public User User { get; set; }

        //TODO: attachments as domain object
        //private List<Attachment> _attachments = new List<Attachment>();
        //public ReadOnlyCollection<Attachment> Attachments => _attachments.AsReadOnly();
        public IEnumerable<Attachment> Attachments { get; set; }


        /// <summary>
        /// Used when creating a message that does not exist
        /// </summary>
        public static Message Create(Conversation conversation, EntityId subject, string body, bool isPublic, User createdByUser, Organization organization)
        {
            return Load(Guid.NewGuid(),conversation.Id,subject,body,isPublic,createdByUser,DateTime.Now, null,organization);
        }

        /// <summary>
        /// Used when getting an existing message
        /// </summary>
        public static Message Load(Guid id, Guid conversationId, EntityId subject, string body, bool isPublic, User createdByUser, DateTime dateCreated, IEnumerable<UserMessageRead> messageReads, Organization organization)
        {
            //Need to parse guid
            if (conversationId == null)
                throw new ArgumentException("conversationId");

            if (string.IsNullOrWhiteSpace(body))
                throw new ArgumentException("body");

            if (createdByUser.UserId == 0)
                throw new ArgumentException("userId");

            if (string.IsNullOrWhiteSpace(subject.Id))
                throw new ArgumentException("subject");

            if (organization == null)
                throw new ArgumentException("organization");
            if (organization.Id==0)
                throw new ArgumentException("organization.id");

            var message = new Message
            {
                Id = id,
                ConversationId = conversationId,
                Subject = subject,
                Body = body,
                IsPublic = isPublic,
                User = createdByUser,
                DateCreated = dateCreated,
                Organization = organization
            };

            if (messageReads != null)
            {
                //AddMessageReads();
                //validate

                message._messageReads.AddRange(messageReads);
            }

            return message;
        }

        internal void MarkAsRead(User user, int organizationId)
        {
            //AddMessageReads

            //validate user,orgid

            //don't add if exists


            _messageReads.Add(new UserMessageRead(user, organizationId, DateTime.Now));
        }


    }
}
