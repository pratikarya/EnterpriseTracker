using EnterpriseTracker.Core.AppContents.Category.Contract.Dto;
using EnterpriseTracker.Core.AppContents.Order.Contract.Dto;
using EnterpriseTracker.Core.Common.Contract.Dto;
using EnterpriseTracker.Core.User.Contract.Dto;
using System.Collections.Generic;

namespace EnterpriseTracker.Core.RealmObjects
{
    public interface IOfflineService
    {
        ResultDto<CategoryDto> CreateCategory(SearchDto<CategoryDto> category);
        ResultDto<OrderDto> UpdateOrder(SearchDto<OrderDto> search);
        ResultDto<List<OrderDto>> GetOrders(OrdersSearchDto search);
        ResultDto<UserDto> CreateUser(SearchDto<UserDto> user);
        ResultDto<List<CategoryDto>> GetCategories();
        string GetValue(string key);
        void SetValue(string key, string value);
        void ClearOfflineDb();
    }
}
