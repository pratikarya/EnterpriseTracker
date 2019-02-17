using EnterpriseTracker.Core.AppContents.Category.Contract.Dto;
using EnterpriseTracker.Core.AppContents.Order.Contract.Dto;
using EnterpriseTracker.Core.AppContents.Product.Contract.Dto;
using EnterpriseTracker.Core.Common.Contract.Dto;
using EnterpriseTracker.Core.Order.Contract.Dto;
using EnterpriseTracker.Core.RealmObjects.Common.Contract.Dto;
using EnterpriseTracker.Core.User.Contract.Dto;
using System.Collections.Generic;

namespace EnterpriseTracker.Core.RealmObjects
{
    public interface IRealmService
    {
        ResultDto<CategoryDto> CreateCategory(SearchDto<CategoryDto> category);
        ResultDto<OrderItemDto> UpdateOrderItem(SearchDto<OrderItemDto> search);
        ResultDto<List<OrderItemDto>> GetOrderItems();
        ResultDto<UserDto> CreateUser(SearchDto<UserDto> user);

        ResultDto<List<CategoryDto>> GetCategories();
        //ResultDto<List<OrderDto>> GetOrders();

        string GetValue(string key);
        void SetValue(string key, string value);
    }
}
