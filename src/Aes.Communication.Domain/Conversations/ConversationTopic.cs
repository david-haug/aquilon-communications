using System;
using System.Collections.Generic;
using System.Text;

namespace Aes.Communication.Domain.Conversations
{
    public class ConversationTopic:IEntity
    {
        public ConversationTopic()
        {

        }
        public ConversationTopic(string title)
        {
            Title = title;
        }

        public ConversationTopic(string title, IDictionary<string, string> attributes)
        {
            Title = title;
            Attributes = attributes;
        }

        public string Title { get; set; }
        public IDictionary<string,string> Attributes { get; set; }
    }
}
