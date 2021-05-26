using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aes.Communication.Api
{
    public class Routes
    {
        public class Conversations
        {
            public const string GetConversations = "GetConversations";
            public const string GetConversation = "GetConversation";
            public const string CreateConversation = "CreateConversation";
            public const string UpdateConversation = "UpdateConversation";

            public const string ReadMessages = "ReadMessages";
        }

        public class ConversationMessages
        {
            public const string CreateConversationMessage = "CreateConversationMessage";
            public const string CreateConversationMessageBySubject = "CreateConversationMessageBySubject";
        }
        public class Messages
        {
            public const string GetMessage = "GetMessage";
            public const string CreateMessage = "CreateMessage";

            //public const string ReadMessage = "ReadMessage";
            //public const string ReadMessages = "ReadMessages";  
        }

        //public class DisputeMessages
        //{
        //    public const string CreateDisputeMessage = "CreateDisputeMessage";
        //}

        //public class InvoiceMessages
        //{
        //    public const string CreateInvoiceMessage = "CreateInvoiceMessage";
        //}

    }

    public class CorsPolicies
    {
        public const string Dev = "DevCorsPolicy";
    }
}
