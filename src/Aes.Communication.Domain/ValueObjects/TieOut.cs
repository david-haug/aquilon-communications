using System;
using System.Collections.Generic;
using System.Text;

namespace Aes.Communication.Domain.ValueObjects
{
    public class TieOut
    {
        public int TieOutId { get; set; }
        public int TpKey { get; set; }
        public int CommodityId { get; set; }
        public int ProductId { get; set; }

    }
}
