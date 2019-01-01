using EnterpriseTracker.Core.RealmObjects.Product.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects.User.Contract.Dto;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseTracker.Core.RealmObjects.Order.Contract.Dto
{
    public class OrderItemRealmDto : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }
        public UserRealmDto Owner { get; set; }
        public UserRealmDto Customer { get; set; }
        public ProductRealmDto Product { get; set; }
        public float Units { get; set; }
        public DateTimeOffset Time { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public string Images { get; set; }
        public float PrintCharge { get; set; }
        public float AdditionalCharge { get; set; }
        public float DeliveryCharge { get; set; }
        public float DesignCharge { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public int Status { get; set; }
        public float TotalAmount
        {
            get
            {
                var totalAmount = PrintCharge + AdditionalCharge + DeliveryCharge + DesignCharge + (Product.Price * Units);
                return totalAmount;
            }
        }

        public override string ToString()
        {
            var name = Units + " " + Product.Unit + " " + Product.Name;
            return name;
        }
    }
}
