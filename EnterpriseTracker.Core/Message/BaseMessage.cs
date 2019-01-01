using MvvmCross.Plugin.Messenger;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseTracker.Core.Message
{
    public class BaseMessage : MvxMessage
    {
        public BaseMessage(object sender) : base(sender)
        {
        }
    }
}
