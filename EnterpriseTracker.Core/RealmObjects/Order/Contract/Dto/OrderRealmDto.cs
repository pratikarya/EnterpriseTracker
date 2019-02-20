using EnterpriseTracker.Core.RealmObjects.Media.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects.Product.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects.User.Contract.Dto;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseTracker.Core.RealmObjects.Order.Contract.Dto
{
    public class OrderRealmDto : RealmObject
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
        public float AdvanceAmount { get; set; }
        public float ExtraCharge { get; set; }
        public float DeliveryCharge { get; set; }
        public float DesignCharge { get; set; }
        public bool CarryBag { get; set; }
        public bool Extra { get; set; }
        public double ContactNumber { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public int Status { get; set; }
        public IList<MediaRealmDto> MediaList;
        public PrintRealmDto Print { get; set; }
        public float TotalAmount
        {
            get
            {
                var totalAmount = PrintCharge + ExtraCharge + DeliveryCharge + DesignCharge + (Product.Price * Units);
                if (Units == 0.5 & Product.Price == 600)
                    totalAmount += 10;
                if (CarryBag)
                    totalAmount += 10;
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
