using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseTracker.Core.Message
{
    public class ValueSelectedMessage<T> : BaseMessage
    {
        public T SelectedValue;
        public ValueSelectedMessage(object sender, T selectedValue) : base(sender)
        {
            SelectedValue = selectedValue;
        }
    }
}
