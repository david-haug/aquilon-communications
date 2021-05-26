using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aes.Communication.Domain.Conversations;
using Aes.Communication.Domain.Messages;
using Aes.Communication.Domain.ValueObjects;
using Aes.Communication.Infrastructure.Json;
using Newtonsoft.Json;

namespace Aes.Communication.Infrastructure.Conversations
{
    public class ConversationJsonRepository : JsonRepository<Conversation>, IConversationRepository
    {
        private string _file;

        public ConversationJsonRepository(string file)
        {
            _file = file;
        }

        public Conversation Get(Guid id)
        {
            var data = Create(_file, new ConversationParser());
            return data.FirstOrDefault(d => d.Id == id);
        }

        public Conversation GetBySubject(EntityId entityId)
        {
            throw new NotImplementedException();
        }

        //public void Save(Conversation conversation)
        //{
        //    //delete existing
        //    Delete(conversation);

        //    //update (add)
        //    var data = Create(_file, new ConversationParser());
        //    data.Add(conversation);
        //    SaveJsonToFile(data, _file);
        //}

        public Task Save(Conversation conversation)
        {
            //delete existing
            Delete(conversation);

            //update (add)
            var data = Create(_file, new ConversationParser());
            data.Add(conversation);
            SaveJsonToFile(data, _file);

            return Task.CompletedTask;
        }

        private void Delete(Conversation obj)
        {
            var oldItems = Create(_file, new ConversationParser());
            var items = oldItems.Where(i => i.Id != obj.Id).ToList();
            SaveJsonToFile(items, _file);
        }

        public IEnumerable<Conversation> GetAll()
        {
            var data = Create(_file, new ConversationParser());
            return data;
        }

       public class ConversationParser : IDomainObjectParser<Conversation>
        {
            public Conversation Create(dynamic data)
            {
                var messages = new List<Message>();
                var msgParser = new MessageParser();
                foreach (var m in data.messages)
                {
                    messages.Add(msgParser.Create(m));
                }

                var id = Guid.Parse(data.id.ToString());

                var title = data.topic.title.ToString();
                Dictionary<string, string> attributes = null;
                if (data.topic.attributes != null)
                    attributes =
                        JsonConvert.DeserializeObject<Dictionary<string, string>>(data.topic.attributes.ToString());

                var topic = new ConversationTopic(title, attributes);
                var entityId = new EntityId(data.subject.id.ToString(),
                    (MessageEntityType) Convert.ToInt16(data.subject.entityType));

                var userFlags = new List<ConversationUserFlag>();

                if (data.userFlags != null)
                {
                    var flagParser = new UserFlagParser();
                    foreach (var flag in data.userFlags)
                    {
                        userFlags.Add(flagParser.Create(flag));
                    }
                }

                EntityId parent = null;
                if (data.parent != null)
                {
                    parent = new EntityId(data.parent.id.ToString(),(MessageEntityType)Convert.ToInt16(data.parent.entityType));
                }

                return Conversation.Load(id, entityId, (DateTime) data.dateCreated,
                    Convert.ToInt32(data.createdByUserId), messages, topic, userFlags, parent, Convert.ToInt32(data.organizationId), Convert.ToInt32(data.counterpartyId));
            }
        }

        public class MessageParser : IDomainObjectParser<Message>
        {
            public Message Create(dynamic msg)
            {
                var id = Guid.Parse(msg.id.ToString());
                var conversationId = Guid.Parse(msg.conversationId.ToString());
                var entityId = new EntityId(msg.subject.id.ToString(),
                    (MessageEntityType) Convert.ToInt16(msg.subject.entityType));

                User user = null;
                if (msg.user != null)
                {
                    var userParser = new UserParser();
                    user = userParser.Create(msg.user);
                }
                else
                {
                    if (msg.createdByUserId != null)
                        user = new User {UserId = (int) msg.createdByUserId};
                }

                var orgParser = new OrganizationParser();
                var org = orgParser.Create(msg.organization);

                var message = Message.Load(id, conversationId, entityId, msg.body.ToString(), (bool) msg.isPublic, user,
                    (DateTime) msg.dateCreated, null, org);

                if (msg.attachments != null)
                {
                    var attachments = new List<Attachment>();
                    var attachmentParser = new AttachmentParser();
                    foreach (var attachment in msg.attachments)
                    {
                        var a = attachmentParser.Create(attachment);
                        if (attachment != null)
                            attachments.Add(a);
                    }

                    if (attachments.Any())
                        message.Attachments = attachments;
                }
                return message;
            }
        }

        public class UserFlagParser : IDomainObjectParser<ConversationUserFlag>
        {
            public ConversationUserFlag Create(dynamic flag)
            {
                return new ConversationUserFlag(Guid.Parse(flag.conversationId.ToString()), (int) flag.userId,
                    (DateTime) flag.dateCreated);
            }
        }

        public class UserParser : IDomainObjectParser<User>
        {
            public User Create(dynamic data)
            {
                return new User
                {
                    UserId = (int) data.userId,
                    FirstName = data.firstName,
                    LastName = data.lastName
                };
            }
        }

        public class OrganizationParser : IDomainObjectParser<Organization>
        {
            public Organization Create(dynamic data)
            {
                var isMember = false;
                if (data.isMember != null)
                    isMember = (bool) data.isMember;

                var isAquilon = false;
                if (data.isAquilon != null)
                    isAquilon = (bool)data.isAquilon;


                return new Organization
                {
                    Id = (int)data.id,
                    Name = data.name
                };
            }
        }

        public class AttachmentParser : IDomainObjectParser<Attachment>
        {
            public Attachment Create(dynamic data)
            {
                if (data == null) return null;
                return new Attachment
                {
                    FileId = data.fileId ?? data.fileId.ToString(),
                    FileName = data.fileName ?? data.fileName.ToString(),
                    FilePath = data.filePath ?? data.filePath.ToString(),
                    FileSize = data.fileSize ?? data.fileSaize.ToString()
                };
            }
        }



    }
}
