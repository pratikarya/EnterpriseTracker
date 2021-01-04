using EnterpriseTracker.Core.AppContents.Order.Contract.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseTracker.Core.TextProcess
{
    public interface ITextProcessor
    {
        OrderDto ConvertTextToOrder(string text);
    }
}
