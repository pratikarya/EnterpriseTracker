using EnterpriseTracker.Core.AppContents.Product.Contract.Dto;
using EnterpriseTracker.Core.User.Contract;
using EnterpriseTracker.Core.User.Contract.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EnterpriseTracker.Core.AppContents.Order.Contract.Dto
{
    public class OrderItemDto
    {
        public Guid Id { get; set; }
        public UserDto Owner { get; set; }
        public UserDto Customer { get; set; }
        public ProductDto Product { get; set; }
        public float Units { get; set; }
        public DateTime Time { get; set; }
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
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public OrderItemStatus Status { get; set; }

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

    public enum OrderItemStatus
    {
        Invalid,
        Confirmed,
        Preparing,
        Ready,
        Delivered,
        Successfull,
        Cancelled
    }    
}
