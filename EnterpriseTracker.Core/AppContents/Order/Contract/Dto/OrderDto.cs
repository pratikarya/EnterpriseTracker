using System;
using EnterpriseTracker.Core.User.Contract;
using EnterpriseTracker.Core.AppContents.Product.Contract.Dto;
using System.Collections.Generic;
using System.Linq;
using EnterpriseTracker.Core.AppContents.Order.Contract.Dto;

namespace EnterpriseTracker.Core.Order.Contract.Dto
{
    public class OrderDto
    {
        public OrderDto()
        {
            Items = new List<OrderItemDto>();
        }
        public Guid Id { get; set; }
        public List<OrderItemDto> Items { get; set; }
        public float AdvanceAmount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ContactNumber { get; set; }
        public float TotalAmount
        {
            get
            {
                return Items != null ? Items.Sum(x => x.TotalAmount) : 0.0f;
            }
        }
        public float BalanceAmount
        {
            get
            {
                var balanceAmount = TotalAmount - AdvanceAmount;
                return balanceAmount;
            }
        }
        public DateTime? Time
        {
            get
            {
                var firstItem = Items.FirstOrDefault();
                if (firstItem != null)
                    return firstItem.Time;
                return null;
            }
        }
    }

    public enum OrderStatus
    {
        Invalid,
        Confirmed
    }
}
