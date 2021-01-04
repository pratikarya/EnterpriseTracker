using EnterpriseTracker.Core.AppContents.Category.Contract.Dto;
using EnterpriseTracker.Core.AppContents.Order.Contract.Dto;
using EnterpriseTracker.Core.Common.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects.Category.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects.Common.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects.Media.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects.Order.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects.Product.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects.User.Contract.Dto;
using EnterpriseTracker.Core.User.Contract.Dto;
using EnterpriseTracker.Core.Utility;
using MvvmCross;
using MvvmCross.Logging;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnterpriseTracker.Core.RealmObjects.Order
{
    public class RealmService : BaseService, IOfflineService
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
                    bool isNewOrder = order.Id == Guid.Empty;
                    var realmOrder = new OrderRealmDto();
                    var allOrders = realm.All<OrderRealmDto>().ToList();

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
                        realmOrder.CreatedDate = DateTime.Now;
                    }
                    else
                    {
                        realmOrder = realm.Find<OrderRealmDto>(order.Id.ToString());
                    }

                    realmOrder.ModifiedDate = DateTime.Now;
                    realmOrder.ExtraCharge = order.ExtraCharge;
                    realmOrder.DeliveryCharge = order.DeliveryCharge;
                    realmOrder.DesignCharge = order.DesignCharge;
                    realmOrder.Details = order.Details;
                    realmOrder.Images = order.Images;
                    realmOrder.Message = order.Message;
                    realmOrder.PrintCharge = order.PrintCharge;
                    realmOrder.CarryBag = order.CarryBag;
                    realmOrder.Extra = order.Extra;
                    realmOrder.ContactNumber = order.ContactNumber;
                    realmOrder.Status = (int)order.Status;
                    realmOrder.Time = order.Time;
                    realmOrder.Units = order.Units;

                    var realmMedias = realm.All<MediaRealmDto>().ToList();
                    if (order.MediaList?.Count > 0)
                    {
                        foreach (var media in order.MediaList)
                        {
                            var realmMedia = realmMedias.FirstOrDefault(x => x.Id == media.Id.ToString());
                            var isNewMedia = realmMedia == null;
                            if (isNewMedia)
                            {
                                Guid newMediaId;
                                do
                                {
                                    newMediaId = Guid.NewGuid();
                                }
                                while (newMediaId == Guid.Empty || realmMedias.Any(x => x.Id == newMediaId.ToString()));

                                realmMedia = new MediaRealmDto();
                                realmMedia.Id = newMediaId.ToString();
                                media.Id = newMediaId;
                            }
                            realmMedia.Bytes = media.Bytes;
                            realmMedia.Url = media.Url;
                            realmMedia.Type = (int)media.Type;
                            if (isNewMedia)
                            {
                                realm.Add(realmMedia);
                                realmOrder.MediaList.Add(realmMedia);
                            }
                        }
                    }

                    if(order.Print != null)
                    {
                        var realmPrints = realm.All<PrintRealmDto>().ToList();
                        var realmPrint = realmPrints.FirstOrDefault(x => x.Id == order.Print.Id.ToString());

                        var isNewPrint = realmPrint == null;
                        if (isNewPrint)
                        {
                            Guid newPrintId;
                            do
                            {
                                newPrintId = Guid.NewGuid();
                            }
                            while (newPrintId == Guid.Empty || realmPrints.Any(x => x.Id == newPrintId.ToString()));

                            realmPrint = new PrintRealmDto();
                            realmPrint.Id = newPrintId.ToString();
                            order.Print.Id = newPrintId;
                        }
                        realmPrint.PrintShape = (int)order.Print.PrintShape;
                        realmPrint.Height = order.Print.Height;
                        realmPrint.Width = order.Print.Width;

                        var realmMedia = realmMedias.FirstOrDefault(x => x.Id == order.Print.Media.Id.ToString());
                        var isNewMedia = realmMedia == null;
                        if (isNewMedia)
                        {
                            Guid newMediaId;
                            do
                            {
                                newMediaId = Guid.NewGuid();
                            }
                            while (newMediaId == Guid.Empty || realmMedias.Any(x => x.Id == newMediaId.ToString()));

                            realmMedia = new MediaRealmDto();
                            realmMedia.Id = newMediaId.ToString();
                            order.Print.Media.Id = newMediaId;
                        }
                        realmMedia.Bytes = order.Print.Media.Bytes;
                        realmMedia.Url = order.Print.Media.Url;
                        realmMedia.Type = (int)order.Print.Media.Type;
                        if (isNewMedia)
                        {
                            realm.Add(realmMedia);
                            realmPrint.Media = realmMedia;
                        }
                        if (isNewPrint)
                        {
                            realm.Add(realmPrint);
                            realmOrder.Print = realmPrint;
                        }
                    }

                    var realmProduct = realm.Find<ProductRealmDto>(order.Product.Id.ToString());
                    //var realmOwner = realm.Find<UserRealmDto>(order.Owner.Id.ToString());
                    //var realmCustomer = realm.Find<UserRealmDto>(order.Customer.Id.ToString());

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
                    //    realmCustomer.FirstName = order.Customer.FirstName;
                    //    realmCustomer.LastName = order.Customer.LastName;
                    //    realmCustomer.Email = order.Customer.Email;
                    //    realmCustomer.Mobile = order.Customer.Mobile;
                    //    realmCustomer.Address = order.Customer.Address;
                    //    realm.Add(realmCustomer);
                    //    order.Customer.Id = newUserId;
                    //}

                    //realmOrder.Owner = realmOwner;
                    //realmOrder.Customer = realmCustomer;
                    realmOrder.Product = realmProduct;
                    if (isNewOrder)
                    {
                        realm.Add(realmOrder);
                    }

                    result.Result = ConvertToDto(realmOrder);
                    result.Status = ResultStatus.Ok;
                }
                catch (Exception ex)
                {
                    result.Result = null;
                    result.ErrorMessage = ex.Message;
                    result.Status = ResultStatus.ServerError;
                    Mvx.IoCProvider.Resolve<IMvxLog>().Trace(ex, "", null);
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
            catch (Exception ex)
            {
                result.Result = null;
                result.ErrorMessage = ex.Message;
                result.Status = ResultStatus.ServerError;
                Mvx.IoCProvider.Resolve<IMvxLog>().Trace(ex, "", null);
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
            catch (Exception ex)
            {
                result.Result = null;
                result.ErrorMessage = ex.Message;
                result.Status = ResultStatus.ServerError;
                Mvx.IoCProvider.Resolve<IMvxLog>().Trace(ex, "", null);
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
            catch (Exception ex)
            {
                result.Result = null;
                result.ErrorMessage = ex.Message;
                result.Status = ResultStatus.ServerError;
                Mvx.IoCProvider.Resolve<IMvxLog>().Trace(ex, "", null);
            }
            return result;
        }

        public ResultDto<List<OrderDto>> GetOrders(OrdersSearchDto search)
        {
            var result = new ResultDto<List<OrderDto>>();
            try
            {
                var realm = Realm.GetInstance(Config);
                List<OrderRealmDto> realmOrders;
                realmOrders = realm.All<OrderRealmDto>().ToList();
                if (search.Date.HasValue)
                {
                    realmOrders = realmOrders.Where(x => x.Time.LocalDateTime.Date == search.Date.Value.Date).ToList();
                }
                var sortedRealmOrders = realmOrders.OrderBy(x => x.Time.LocalDateTime).ToList();
                List<OrderDto> orders = new List<OrderDto>();
                foreach (var realmOrder in sortedRealmOrders)
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
                result.ErrorMessage = ex.Message;
                result.Status = ResultStatus.ServerError;
                Mvx.IoCProvider.Resolve<IMvxLog>().Trace(ex, "", null);
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
