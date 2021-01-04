using EnterpriseTracker.Core.AppContents.Category.Contract.Dto;
using EnterpriseTracker.Core.AppContents.Media.Contract.Dto;
using EnterpriseTracker.Core.AppContents.Order.Contract.Dto;
using EnterpriseTracker.Core.AppContents.Product.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects.Category.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects.Media.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects.Order.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects.Product.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects.User.Contract.Dto;
using EnterpriseTracker.Core.User.Contract.Dto;
using EnterpriseTracker.Core.Utility;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;

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

        private void OnMigration(Migration migration, ulong oldSchemaVersion)
        {
            //if(oldSchemaVersion == 3)
            //{
            //    var newOrders = migration.NewRealm.All<OrderRealmDto>();

            //    var oldOrders = migration.OldRealm.All("OrderRealmDto");

            //    for (var i = 0; i < newOrders.Count(); i++)
            //    {
            //        var oldOrder = oldOrders.ElementAt(i);
            //        var newOrder = newOrders.ElementAt(i);

            //        newOrder.FromShop = oldOrder.Owner == null;
            //    }
            //}
        }

        public OrderDto ConvertToDto(OrderRealmDto realmOrder)
        {
            var order = new OrderDto();
            order.Id = Guid.Parse(realmOrder.Id);
            order.Status = (OrderStatus)realmOrder.Status;
            order.ExtraCharge = realmOrder.ExtraCharge;
            order.CreatedDate = realmOrder.CreatedDate.LocalDateTime;
            order.DeliveryCharge = realmOrder.DeliveryCharge;
            order.DesignCharge = realmOrder.DesignCharge;
            order.Details = realmOrder.Details;
            order.Images = realmOrder.Images;
            order.Message = realmOrder.Message;
            order.ModifiedDate = realmOrder.ModifiedDate.LocalDateTime;
            order.PrintCharge = realmOrder.PrintCharge;
            order.Status = (OrderStatus)realmOrder.Status;
            order.Time = realmOrder.Time.LocalDateTime;
            order.Units = realmOrder.Units;
            order.CarryBag = realmOrder.CarryBag;
            order.Extra = realmOrder.Extra;
            order.ContactNumber = realmOrder.ContactNumber;
            if(realmOrder.Print != null)
                order.Print = ConvertToDto(realmOrder.Print);
            if(realmOrder.MediaList?.Count > 0)
            {
                order.MediaList = new List<MediaDto> { };
                foreach(var realmMedia in realmOrder.MediaList)
                {
                    order.MediaList.Add(ConvertToDto(realmMedia));
                }
            }
            //order.Owner = ConvertToDto(realmOrder.Owner);
            //order.Customer = ConvertToDto(realmOrder.Customer);
            order.Product = ConvertToDto(realmOrder.Product);
            return order;
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

        public MediaDto ConvertToDto(MediaRealmDto realmMedia)
        {
            var media = new MediaDto();
            media.Id = Guid.Parse(realmMedia.Id);
            media.Bytes = realmMedia.Bytes;
            media.Type = (MediaType)realmMedia.Type;
            media.Url = realmMedia.Url;
            return media;
        }

        public PrintDto ConvertToDto(PrintRealmDto realmPrint)
        {
            var print = new PrintDto();
            print.Id = Guid.Parse(realmPrint.Id);
            print.PrintShape = (PrintShape) realmPrint.PrintShape;
            print.Height = realmPrint.Height;
            print.Width = realmPrint.Width;
            print.Media = ConvertToDto(realmPrint.Media);
            return print;
        }
    }
}
