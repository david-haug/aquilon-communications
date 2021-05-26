using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aes.Communication.Domain.Messages;

namespace Aes.Communication.Api.Models
{
    public class EntityIdModel
    {
        /// <summary>
        /// Unique id of business object
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// The business object’s type. Select from invoice, dispute, tieout, or deal.
        /// </summary>
        public string Type { get; set; }

        public static EntityId Map(EntityIdModel model)
        {
            if (model == null)
                return null;

            MessageEntityType type = (MessageEntityType)0;
            switch (model.Type)
            {
                case "deal":
                    type = MessageEntityType.Deal;
                    break;
                case "dispute":
                    type = MessageEntityType.Dispute;
                    break;
                case "invoice":
                    type = MessageEntityType.Invoice;
                    break;
                case "tieout":
                    type = MessageEntityType.TieOut;
                    break;
                default:
                    type = MessageEntityType.Unknown;
                    break;
            }

            return new EntityId(model.Id, type);
        }
    }
}
