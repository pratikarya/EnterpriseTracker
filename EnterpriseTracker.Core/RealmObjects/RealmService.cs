using EnterpriseTracker.Core.AppContents.Category.Contract.Dto;
using EnterpriseTracker.Core.Common.Contract.Dto;
using EnterpriseTracker.Core.Order.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects.Category.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects.Common.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects.Order.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects.Product.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects.User.Contract.Dto;
using EnterpriseTracker.Core.User.Contract.Dto;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnterpriseTracker.Core.RealmObjects.Order
{
    public class RealmService : BaseService, IRealmService
    {
        public ResultDto<OrderDto> UpdateOrder(SearchDto<OrderDto> search)
        {
            var order = search.RequestDto;
            var result = new ResultDto<OrderDto>();
            var realm = Realm.GetInstance(Config);
            realm.Write(() =>
            {
                try
                {
                    var realmOrder = new OrderRealmDto();
                    var allOrders = realm.All<OrderRealmDto>().ToList();
                    var isNewOrder = order.Id == Guid.Empty;

                    if (isNewOrder)
                    {
                        Guid newOrderId;
                        do
                        {
                            newOrderId = Guid.NewGuid();
                        }
                        while (newOrderId == Guid.Empty || allOrders.Any(x => x.Id == newOrderId.ToString()));
                        order.Id = newOrderId;
                        realmOrder.Id = newOrderId.ToString();
                        realmOrder.CreatedDate = order.CreatedDate;
                    }
                    else
                    {
                        realmOrder = realm.Find<OrderRealmDto>(order.Id.ToString());
                    }

                    realmOrder.ContactNumber = order.ContactNumber;
                    realmOrder.AdvanceAmount = order.AdvanceAmount;
                    realmOrder.Status = (int)order.Status;
                    realmOrder.ModifiedDate = order.ModifiedDate;
                    foreach (var orderItem in order.Items)
                    {
                        bool isNewOrderItem = orderItem.Id == Guid.Empty;
                        var realmOrderItem = new OrderItemRealmDto();
                        var allOrderItems = realm.All<OrderItemRealmDto>().ToList();

                        if (isNewOrderItem)
                        {
                            Guid newOrderItemId;
                            do
                            {
                                newOrderItemId = Guid.NewGuid();
                            }
                            while (newOrderItemId == Guid.Empty || allOrderItems.Any(x => x.Id == newOrderItemId.ToString()));

                            orderItem.Id = newOrderItemId;
                            realmOrderItem.Id = newOrderItemId.ToString();
                            realmOrderItem.CreatedDate = orderItem.CreatedDate;
                        }
                        else
                        {
                            realmOrderItem = realm.Find<OrderItemRealmDto>(orderItem.Id.ToString());
                        }

                        realmOrderItem.ModifiedDate = orderItem.ModifiedDate;
                        realmOrderItem.AdditionalCharge = orderItem.AdditionalCharge;
                        realmOrderItem.DeliveryCharge = orderItem.DeliveryCharge;
                        realmOrderItem.DesignCharge = orderItem.DesignCharge;
                        realmOrderItem.Details = orderItem.Details;
                        realmOrderItem.Images = orderItem.Images;
                        realmOrderItem.Message = orderItem.Message;
                        realmOrderItem.PrintCharge = orderItem.PrintCharge;
                        realmOrderItem.Status = (int)orderItem.Status;
                        realmOrderItem.Time = orderItem.Time;
                        realmOrderItem.Units = orderItem.Units;

                        var realmProduct = realm.Find<ProductRealmDto>(orderItem.Product.Id.ToString());
                            //var realmOwner = realm.Find<UserRealmDto>(orderItem.Owner.Id.ToString());
                            //var realmCustomer = realm.Find<UserRealmDto>(orderItem.Customer.Id.ToString());

                            //if (realmCustomer == null)
                            //{
                            //    realmCustomer = new UserRealmDto();
                            //    var allUsers = realm.All<UserRealmDto>().ToList();
                            //    Guid newUserId;
                            //    do
                            //    {
                            //        newUserId = Guid.NewGuid();
                            //    }
                            //    while (newUserId == Guid.Empty || allUsers.Any(x => x.Id == newUserId.ToString()));
                            //    realmCustomer.Id = newUserId.ToString();
                            //    realmCustomer.FirstName = orderItem.Customer.FirstName;
                            //    realmCustomer.LastName = orderItem.Customer.LastName;
                            //    realmCustomer.Email = orderItem.Customer.Email;
                            //    realmCustomer.Mobile = orderItem.Customer.Mobile;
                            //    realmCustomer.Address = orderItem.Customer.Address;
                            //    realm.Add(realmCustomer);
                            //    orderItem.Customer.Id = newUserId;
                            //}

                            //realmOrderItem.Owner = realmOwner;
                            //realmOrderItem.Customer = realmCustomer;
                            realmOrderItem.Product = realmProduct;
                        if (isNewOrderItem)
                        {
                            realm.Add(realmOrderItem);
                            realmOrder.Items.Add(realmOrderItem);
                        }
                    }

                    if (isNewOrder)
                        realm.Add(realmOrder);

                    result.Result = order;
                    result.Status = ResultStatus.Ok;
                }
                catch (Exception ex)
                {
                    result.Result = null;
                    result.Status = ResultStatus.ServerError;
                }
            });
            return result;
        }

        public ResultDto<CategoryDto> CreateCategory(SearchDto<CategoryDto> search)
        {
            var category = search.RequestDto;
            var result = new ResultDto<CategoryDto>();
            try
            {
                var realm = Realm.GetInstance(Config);
                realm.Write(() =>
                {
                    var realmCategory = new CategoryRealmDto();
                    var allCategories = realm.All<CategoryRealmDto>().ToList();
                    Guid newCategoryId;
                    do
                    {
                        newCategoryId = Guid.NewGuid();
                    }
                    while (newCategoryId == Guid.Empty || allCategories.Any(x => x.Id == newCategoryId.ToString()));
                    category.Id = newCategoryId;
                    realmCategory.Id = newCategoryId.ToString();
                    realmCategory.Desc = category.Desc;
                    realmCategory.Name = category.Name;
                    realmCategory.Status = (int)category.Status;
                    foreach (var product in category.Products)
                    {
                        var allProducts = realm.All<CategoryRealmDto>().ToList();
                        var realmProduct = new ProductRealmDto();
                        Guid newProductId;
                        do
                        {
                            newProductId = Guid.NewGuid();
                        }
                        while (newProductId == Guid.Empty || allProducts.Any(x => x.Id == newProductId.ToString()));
                        product.Id = newProductId;
                        realmProduct.Id = newProductId.ToString();
                        realmProduct.Status = (int)product.Status;
                        realmProduct.IsVeg = product.IsVeg;
                        realmProduct.Name = product.Name;
                        realmProduct.Price = product.Price;
                        realmProduct.Unit = product.Unit;
                        realm.Add(realmProduct);
                        realmCategory.Products.Add(realmProduct);
                    }
                    realm.Add(realmCategory);
                });
                result.Result = category;
                result.Status = ResultStatus.Ok;
            }
            catch (Exception)
            {
                result.Result = null;
                result.Status = ResultStatus.ServerError;
            }
            return result;
        }

        public ResultDto<UserDto> CreateUser(SearchDto<UserDto> search)
        {
            var user = search.RequestDto;
            var result = new ResultDto<UserDto>();
            try
            {
                var realm = Realm.GetInstance(Config);
                realm.Write(() =>
                {
                    var realmUser = new UserRealmDto();
                    var allUsers = realm.All<UserRealmDto>().ToList();
                    Guid newUserId;
                    do
                    {
                        newUserId = Guid.NewGuid();
                    }
                    while (newUserId == Guid.Empty || allUsers.Any(x => x.Id == newUserId.ToString()));
                    realmUser.Id = newUserId.ToString();
                    realmUser.FirstName = user.FirstName;
                    realmUser.LastName = user.LastName;
                    realmUser.Email = user.Email;
                    realmUser.Mobile = user.Mobile;
                    realmUser.Address = user.Address;
                    realm.Add(realmUser);
                });
                result.Result = user;
                result.Status = ResultStatus.Ok;
            }
            catch (Exception)
            {
                result.Result = null;
                result.Status = ResultStatus.ServerError;
            }
            return result;
        }

        public ResultDto<List<CategoryDto>> GetCategories()
        {
            var result = new ResultDto<List<CategoryDto>>();
            try
            {
                var categories = new List<CategoryDto>();
                var realm = Realm.GetInstance(Config);
                var realmCategories = realm.All<CategoryRealmDto>().ToList();
                foreach (var realmCategory in realmCategories)
                {
                    var category = ConvertToDto(realmCategory);
                    categories.Add(category);
                }

                result.Result = categories;
                result.Status = ResultStatus.Ok;
            }
            catch (Exception)
            {
                result.Result = null;
                result.Status = ResultStatus.ServerError;
            }
            return result;
        }

        public ResultDto<List<OrderDto>> GetOrders()
        {
            var result = new ResultDto<List<OrderDto>>();
            try
            {
                var realm = Realm.GetInstance(Config);
                var realmOrders = realm.All<OrderRealmDto>().OrderByDescending(x => x.Items.Max(y => y.Time)).ToList();
                List<OrderDto> orders = new List<OrderDto>();
                foreach (var realmOrder in realmOrders)
                {
                    var order = ConvertToDto(realmOrder);
                    orders.Add(order);
                }
                result.Result = orders;
                result.Status = ResultStatus.Ok;
            }
            catch (Exception ex)
            {
                result.Result = null;
                result.Status = ResultStatus.ServerError;
            }
            return result;
        }

        public string GetValue(string key)
        {
            var realm = Realm.GetInstance(Config);
            var gen = realm.Find<GenericRealmDto>(key);
            if (gen != null)
                return gen.Value;
            return "";
        }

        public void SetValue(string key, string value)
        {
            var realm = Realm.GetInstance(Config);

            realm.Write(() =>
            {
                var gen = realm.Find<GenericRealmDto>(key);
                if (gen == null)
                {
                    gen = new GenericRealmDto()
                    {
                        Key = key,
                        Value = value
                    };
                    realm.Add(gen);
                }
                else
                {
                    gen.Value = value;
                }
            });
        }
    }
}
