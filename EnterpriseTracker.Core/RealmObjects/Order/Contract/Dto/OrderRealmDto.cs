using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnterpriseTracker.Core.RealmObjects.Order.Contract.Dto
{
    public class OrderRealmDto : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }
        public IList<OrderItemRealmDto> Items { get; }
        public float AdvanceAmount { get; set; }
        public int Status { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
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
    }
}
