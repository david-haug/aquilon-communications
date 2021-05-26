using System;
using System.Collections.Generic;
using System.Text;

namespace Aes.Communication.Domain.Messages
{
    public class EntityId
    {
        public EntityId(string id, MessageEntityType type)
        {
            Id = id;
            EntityType = type;
        }
        public string Id { get; }
        public MessageEntityType EntityType { get; }

        public string Type
        {
            get
            {
                switch (EntityType)
                {
                    case MessageEntityType.TieOut:
                        return "tieout";
                    case MessageEntityType.Invoice:
                        return "invoice";
                    case MessageEntityType.Dispute:
                        return "dispute";
                    case MessageEntityType.Deal:
                        return "deal";
                    default:
                        return "";
                }

            }
        }

    }



}
