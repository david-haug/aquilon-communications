using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Aes.Communication.Domain.Messages;
using Aes.Communication.Domain.ValueObjects;
using Aes.Communication.Infrastructure.Conversations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Aes.Communication.Infrastructure.Messages
{
    public class MessageJsonRepository:IMessageRepository
    {
        private string _file;

        public MessageJsonRepository(string file)
        {
            _file = file;
        }

        //public Message Get(Guid id)
        //{
        //    var data = GetData();
        //    return data.FirstOrDefault(d => d.Id == id);
        //}

        //public void Add(Message message)
        //{
        //    var items = GetData();
        //    items.Add(message);
        //    SaveJsonToFile(items, _file);
        //}

        //public void Update(Message message)
        //{
        //    throw new NotImplementedException();
        //}

        //private List<Message> ConvertDataToMessages(dynamic data)
        //{
        //    var messages = new List<Message>();
        //    var userParser = new ConversationJsonRepository.UserParser();
        //    foreach (var msg in data)
        //    {
        //        var id = Guid.Parse(msg.id.ToString());
        //        var conversationId = Guid.Parse(msg.conversationId.ToString());
        //        var entityId = new EntityId(msg.subject.id.ToString(), (MessageEntityType)Convert.ToInt16(msg.subject.entityType));

        //        User user = null;
        //        if (msg.user != null)
        //        {
        //            user = userParser.Create(msg.user);
        //        }
        //        else
        //        {
        //            if (msg.createdByUserId != null)
        //                user = new User { UserId = (int)msg.createdByUserId };
        //        }


        //        var message = Message.Load(id, conversationId, entityId, msg.body.ToString(), (bool)msg.isPublic, user, (DateTime)msg.dateCreated,null);
        //        messages.Add(message);
        //    }
        //    return messages;
        //}

        //private List<Message> GetData()
        //{
        //    var messages = new List<Message>();
        //    var json = string.Empty;
        //    var userParser = new ConversationJsonRepository.UserParser();
        //    using (StreamReader sr = File.OpenText(_file))
        //    {
        //        json = sr.ReadToEnd();
        //        dynamic data = JsonConvert.DeserializeObject(json);
        //        foreach (var msg in data)
        //        {
        //            var id = Guid.Parse(msg.id.ToString());
        //            var conversationId = Guid.Parse(msg.conversationId.ToString());
        //            var entityId = new EntityId(msg.subject.id.ToString(), (MessageEntityType)Convert.ToInt16(msg.subject.entityType));

        //            User user = null;
        //            if (msg.user != null)
        //            {

        //                user = userParser.Create(msg.user);
        //            }
        //            else
        //            {
        //                if (msg.createdByUserId != null)
        //                    user = new User { UserId = (int)msg.createdByUserId };
        //            }

        //            var message = Message.Load(id, conversationId, entityId, msg.body.ToString(), (bool)msg.isPublic, user, (DateTime)msg.dateCreated,null);
        //            messages.Add(message);
        //        }
        //    }

        //    return messages;
        //}

        //private void SaveJsonToFile(object obj, string file)
        //{
        //    using (FileStream fs = File.Open(file, FileMode.Truncate))
        //    using (StreamWriter sw = new StreamWriter(fs))
        //    using (JsonWriter jw = new JsonTextWriter(sw))
        //    {
        //        jw.Formatting = Formatting.Indented;
        //        var serializer = new JsonSerializer {ContractResolver = new CamelCasePropertyNamesContractResolver()};
        //        serializer.Serialize(jw, obj);
        //    }
        //}
        public Message Get(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
