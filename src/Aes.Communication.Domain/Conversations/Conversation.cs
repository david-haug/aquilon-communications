using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Aes.Communication.Domain.Messages;
using Aes.Communication.Domain.ValueObjects;

namespace Aes.Communication.Domain.Conversations
{
    public class Conversation:IEntity
    {
        public Guid Id { get; private set; }

        public EntityId Subject { get; private set; }

        private readonly List<Message> _messages = new List<Message>();
        public ReadOnlyCollection<Message> Messages => _messages.AsReadOnly();

        public DateTime DateCreated { get; private set; }
        public int CreatedByUserId { get; private set; }
        public ConversationTopic Topic { get; private set; }

        private List<ConversationUserFlag> _flags = new List<ConversationUserFlag>();
        public ReadOnlyCollection<ConversationUserFlag> UserFlags => _flags.AsReadOnly();
        public int OrganizationId { get; private set; }
        public int CounterpartyId { get; private set; }

        //related? associated?
        public EntityId Parent { get; private set; }

        public static Conversation Create(EntityId subjectEntityId, int userId, ConversationTopic topic, EntityId parent, int organizationId, int counterpartyId)
        {
            var conversation = Load(Guid.NewGuid(),subjectEntityId,DateTime.Now,userId, null, topic, null, parent, organizationId, counterpartyId);
            DomainEvents.Raise(new ConversationCreated(conversation, new DateTimeOffset(conversation.DateCreated)));
            return conversation;
        }

        public static Conversation Load(Guid id, EntityId subject, DateTime dateCreated, int userId, IEnumerable<Message> messages, ConversationTopic topic, 
            IEnumerable<ConversationUserFlag> flags, EntityId parent, int organizationId, int counterpartyId)
        {
            if (string.IsNullOrWhiteSpace(subject.Id) || subject.EntityType == MessageEntityType.Unknown)
                throw new ArgumentException("subject");
            if (userId == 0)
                throw new ArgumentException("userId");
            if (organizationId == 0)
                throw new ArgumentException("organizationId");
            if (counterpartyId == 0)
                throw new ArgumentException("counterpartyId");

            ValidateTopic(topic);

            var conversation = new Conversation
            {
                Id = id,
                Subject = subject,
                DateCreated = dateCreated,
                CreatedByUserId = userId,
                Topic = topic,
                Parent = parent,
                OrganizationId = organizationId,
                CounterpartyId = counterpartyId
            };

            if(messages!=null)
                conversation._messages.AddRange(messages);

            if (flags != null)
                conversation._flags.AddRange(flags);

            return conversation;
        }

        public void AddMessage(Message message)
        {
            if (message == null)
                throw new ArgumentNullException();

            if (message.Organization.Id != OrganizationId && message.Organization.Id != CounterpartyId)
                throw new ApplicationException("The message organization does not have access to conversation.");

            DomainEvents.Raise(new ConversationMessageAdded(message, new DateTimeOffset(message.DateCreated)));
            _messages.Add(message);
        }

        public void UpdateTopic(ConversationTopic topic)
        {
            ValidateTopic(topic);

            Topic = topic;
            DomainEvents.Raise(new ConversationTopicUpdated(topic, new DateTimeOffset(DateTime.Now)));
        }

        private static void ValidateTopic(ConversationTopic topic)
        {
            if (topic == null || string.IsNullOrWhiteSpace(topic.Title))
                throw new ArgumentException("topic");
        }

        public void ChangeParent(EntityId parent)
        {
            Parent = parent;
            DomainEvents.Raise(new ConversationParentChanged(this, new DateTimeOffset(DateTime.Now)));
        }

        public void AddUserFlag(int userId)
        {
            if (userId == 0)
                throw new ArgumentException("userId");

            var existing = _flags.FirstOrDefault(f => f.UserId == userId);
            if (existing != null)
                return;

            var flag = new ConversationUserFlag(Id, userId, DateTime.Now);
            DomainEvents.Raise(new ConversationUserFlagAdded(flag, new DateTimeOffset(flag.DateCreated)));
            _flags.Add(flag);
        }

        public void RemoveUserFlag(int userId)
        {
            if (userId == 0)
                throw new ArgumentException("userId");

            var existing = _flags.FirstOrDefault(f => f.UserId == userId);
            if (existing == null)
                return;

            DomainEvents.Raise(new ConversationUserFlagRemoved(existing, new DateTimeOffset(DateTime.Now)));
            _flags.Remove(existing);
        }

        /// <summary>
        /// Mark a message as read by user and organization
        /// </summary>
        /// <remarks>User can only appear in ReadByUsers list once and only added if message created by another user</remarks>
        public void MarkMessageAsRead(Message message, User user, int organizationId)
        {
            if(user.UserId == 0)
                throw new ArgumentException("userId");
            if (organizationId == 0)
                throw new ArgumentException("organizationId");
            if (organizationId != OrganizationId && organizationId != CounterpartyId)
                throw new ApplicationException("OrganizationId does not have access to conversation.");

            //only add read from other org
            if (organizationId == message.Organization.Id) return;
            //dont mark private messages as read
            if (!message.IsPublic) return;

            //do not add the same user AND org more than once
            //add to if doesn't exist
            if (!message.ReadByUsers.Any(r => r.User.UserId == user.UserId && 
                                             r.OrganizationId == organizationId))
            {
                message.MarkAsRead(user, organizationId);
                DomainEvents.Raise(new MessageMarkedAsRead(message, new DateTimeOffset(DateCreated)));
            }
        }

        public int GetUnreadMessageCountByOrganization(int organizationId)
        {
            var unread = 0;
            //only look at messages from other org because an org can't read their own message
            //do not include private messages
            foreach (var message in _messages.Where(m=>m.Organization.Id!= organizationId && m.IsPublic))
            {
                var isRead = message.ReadByUsers.Any(u => u.OrganizationId == organizationId);
                if (!isRead)
                    unread += 1;
            }

            return unread;
        }


    }
}
