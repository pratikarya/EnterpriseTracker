using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseTracker.Core.Message
{
    public class OrdersListUpdatedMessage : BaseMessage
    {
        public OrdersListUpdatedMessage(object sender) : base(sender)
        {
        }
    }
}
