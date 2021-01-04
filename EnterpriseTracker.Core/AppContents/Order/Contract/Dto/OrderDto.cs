using EnterpriseTracker.Core.AppContents.Media.Contract.Dto;
using EnterpriseTracker.Core.AppContents.Product.Contract.Dto;
using EnterpriseTracker.Core.User.Contract;
using EnterpriseTracker.Core.User.Contract.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EnterpriseTracker.Core.AppContents.Order.Contract.Dto
{
    public class OrderDto
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
        public OrderStatus Status { get; set; }
        public List<MediaDto> MediaList { get; set; }
        public PrintDto Print { get; set; }

        public float TotalAmount
        {
            get
            {
                if (Product == null)
                    return 0;

                var totalAmount = PrintCharge + ExtraCharge + DeliveryCharge + DesignCharge + (Product.Price * Units);

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

    public enum OrderStatus
    {
        NotConfirmed,
        Confirmed,
        Preparing,
        Ready,
        Completed,
        Cancelled
    }    
}
