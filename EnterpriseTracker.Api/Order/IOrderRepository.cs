using EnterpriseTracker.Core.Common.Contract.Dto;
using EnterpriseTracker.Core.Order.Contract.Dto;

namespace EnterpriseTracker.Api.Order
{
    public interface IOrderRepository
    {
        ResultDto<OrderDto> UpdateOrder(SearchDto<OrderDto> request);
    }
}