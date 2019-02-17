using EnterpriseTracker.Core.AppContents.Category.Contract.Dto;
using EnterpriseTracker.Core.AppContents.Order.Contract.Dto;
using EnterpriseTracker.Core.AppContents.Product.Contract.Dto;
using EnterpriseTracker.Core.Order.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects.Category.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects.Order.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects.Product.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects.User.Contract.Dto;
using EnterpriseTracker.Core.User.Contract.Dto;
using EnterpriseTracker.Core.Utility;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnterpriseTracker.Core.RealmObjects
{
    public class BaseService
    {
        public RealmConfiguration Config { get; set; }

        public BaseService()
        {
            Config = new RealmConfiguration()
            {
                SchemaVersion = Constants.RealmSchemaVersion
            };
        }

        //public OrderDto ConvertToDto(OrderRealmDto realmOrder)
        //{
        //    var order = new OrderDto();
        //    order.Id = Guid.Parse(realmOrder.Id);
        //    order.Status = (OrderStatus)realmOrder.Status;
        //    order.ContactNumber = realmOrder.ContactNumber;
        //    order.Items = new List<OrderItemDto>();
        //    order.CreatedDate = realmOrder.CreatedDate.LocalDateTime;
        //    order.ModifiedDate = realmOrder.ModifiedDate.LocalDateTime;
        //    foreach (var realmOrderItem in realmOrder.Items.OrderByDescending(x => x.Time))
        //    {
        //        var orderItem = ConvertToDto(realmOrderItem);
        //        order.Items.Add(orderItem);
        //    }
        //    return order;
        //}

        public OrderItemDto ConvertToDto(OrderItemRealmDto realmOrderItem)
        {
            var orderItem = new OrderItemDto();
            orderItem.Id = Guid.Parse(realmOrderItem.Id);
            orderItem.Status = (OrderItemStatus)realmOrderItem.Status;
            orderItem.ExtraCharge = realmOrderItem.ExtraCharge;
            orderItem.CreatedDate = realmOrderItem.CreatedDate.LocalDateTime;
            orderItem.DeliveryCharge = realmOrderItem.DeliveryCharge;
            orderItem.DesignCharge = realmOrderItem.DesignCharge;
            orderItem.Details = realmOrderItem.Details;
            orderItem.Images = realmOrderItem.Images;
            orderItem.Message = realmOrderItem.Message;
            orderItem.ModifiedDate = realmOrderItem.ModifiedDate.LocalDateTime;
            orderItem.PrintCharge = realmOrderItem.PrintCharge;
            orderItem.Status = (OrderItemStatus)realmOrderItem.Status;
            orderItem.Time = realmOrderItem.Time.LocalDateTime;
            orderItem.Units = realmOrderItem.Units;
            orderItem.CarryBag = realmOrderItem.CarryBag;
            orderItem.Extra = realmOrderItem.Extra;
            orderItem.ContactNumber = realmOrderItem.ContactNumber;
            //orderItem.Owner = ConvertToDto(realmOrderItem.Owner);
            //orderItem.Customer = ConvertToDto(realmOrderItem.Customer);
            orderItem.Product = ConvertToDto(realmOrderItem.Product);
            return orderItem;
        }

        public ProductDto ConvertToDto(ProductRealmDto realmProduct)
        {
            var product = new ProductDto();
            product.Id = Guid.Parse(realmProduct.Id);
            product.Status = (ProductStatus)realmProduct.Status;
            product.IsVeg = realmProduct.IsVeg;
            product.Name = realmProduct.Name;
            product.Price = realmProduct.Price;
            product.Unit = realmProduct.Unit;
            return product;
        }

        public UserDto ConvertToDto(UserRealmDto realmUser)
        {
            var user = new UserDto();
            user.Id = Guid.Parse(realmUser.Id);
            user.Address = realmUser.Address;
            user.FirstName = realmUser.FirstName;
            user.LastName = realmUser.LastName;
            user.Mobile = realmUser.Mobile;
            user.Email = realmUser.Email;
            return user;
        }

        public CategoryDto ConvertToDto(CategoryRealmDto realmCategory)
        {
            var category = new CategoryDto();
            category.Id = Guid.Parse(realmCategory.Id);
            category.Desc = realmCategory.Desc;
            category.Name = realmCategory.Name;
            category.Status = (CategoryStatus)realmCategory.Status;
            category.Products = new List<ProductDto>();
            foreach(var realmProduct in realmCategory.Products)
            {
                var product = ConvertToDto(realmProduct);
                category.Products.Add(product);
            }
            return category;
        }
    }
}
