using EnterpriseTracker.Core.AppContents.Order.Contract.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseTracker.Core.TextProcess
{
    public class TextProcessor  : ITextProcessor
    {
        List<string> Keywords = new List<string> { "kg", "date", "message" };

        public OrderDto ConvertTextToOrder(string input)
        {
            input = input.ToLower();
            var order = new OrderDto();

            try
            {

                var startIndexOfKg = input.IndexOf(Keywords[0]);
                var startIndexOfDate = input.IndexOf(Keywords[1]);
                var startIndexOfMessage = input.IndexOf(Keywords[2]);
                var endIndexOfKg = input.LastIndexOf(Keywords[0]);
                var endIndexOfDate = input.LastIndexOf(Keywords[1]);
                var endIndexOfMessage = input.LastIndexOf(Keywords[2]);

                var amount = input.Substring(0, startIndexOfKg);
                var flavour = input.Substring(endIndexOfKg, startIndexOfDate);
                var date = input.Substring(endIndexOfDate, startIndexOfMessage);
                var message = input.Substring(endIndexOfMessage, input.Length - 1);
            }
            catch (Exception ex)
            { 

            }


            return order;
        }
    }
}
