using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Infrastructure.CosmosDataAccess;

namespace Aes.Communication.Infrastructure.Tests
{
    public class Settings
    {

        public class Cosmos
        {
            public const string CommunicationsDatabase = "Communications";
            public const string ConnectionString =
                "AccountEndpoint=https://sandbox-aesidc-cosmossql.documents.azure.com:443/;AccountKey=zGFOzwdh9qqlqzWNA5VdE3owTJKSFyR4yIt1PLle3d3RLbJnaKeYzSoo4E0gP03j2N2HBtHo0MAJIcdB9mTYmA==;";

            //dev
            public const string ValidConversationId = "744e7853-788b-46b0-8eba-e60441907780";
            public const string ConversationsCollection = "Conversations";

            ////QA
            //public const string ValidConversationId = "cce9e324-26cb-4f58-be51-b3455df3c9eb";
            //public const string ConversationsCollection = "Conversations_QA";
        }

        public class Json
        {
            public const string ConversationsFilePath = "C:\\Users\\dhaug\\Documents\\Darwin Projects\\Aes.Communication\\src\\Aes.Communication.Infrastructure\\JsonDataAccess\\Json\\Data\\Conversation.json";
            public const string MessagesFilePath = "C:\\Users\\dhaug\\Documents\\Darwin Projects\\Aes.Communication\\src\\Aes.Communication.Infrastructure\\JsonDataAccess\\Json\\Data\\Message.json";
            public const string EventsFilePath = "C:\\Users\\dhaug\\Documents\\Darwin Projects\\Aes.Communication\\src\\Aes.Communication.Infrastructure\\JsonDataAccess\\Json\\Data\\Event.json";
            public const string ValidConversationId = "2a0edce4-7848-4dc4-9014-f1f1850e5bcb";
        }
    }
}
