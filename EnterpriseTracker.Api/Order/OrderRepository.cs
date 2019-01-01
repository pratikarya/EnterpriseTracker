using EnterpriseTracker.Core.Common.Contract.Dto;
using EnterpriseTracker.Core.Order.Contract.Dto;
using EnterpriseTracker.Api.Common;
using System;
using System.Linq;

namespace EnterpriseTracker.Api.Order
{
    public class OrderRepository : BaseRepository, IOrderRepository
    {
        public ResultDto<OrderDto> UpdateOrder(SearchDto<OrderDto> request)
        {
            var response = new ResultDto<OrderDto>();
            var isNew = false;
            var orderDto = request.RequestDto;
            tbl_Order tblOrder;

            try
            {
                var context = GetDataContext();

                //new entry
                isNew = orderDto.Id == Guid.Empty;

                if (isNew)
                {   
                    Guid newId;

                    do
                    {
                        newId = new Guid();
                    } while (context.tbl_Orders.Any(x => x.Id.Equals(newId)));

                    tblOrder = new tbl_Order
                    {
                        Id = newId
                    };
                    orderDto.Id = newId;
                }
                else
                {
                    tblOrder = context.tbl_Orders.FirstOrDefault(x => x.Id.Equals(orderDto.Id));
                }

                if(tblOrder == null)
                {
                    response.Status = ResultStatus.ServerError;
                    response.Result = orderDto;
                    return response;
                }

                tblOrder.OwnerId = orderDto.Owner.Id;
                tblOrder.CustomerId = orderDto.Customer.Id;
                tblOrder.ProductId = orderDto.Product.Id;
                tblOrder.Message = orderDto.Message;
                tblOrder.Details = orderDto.Details;
                tblOrder.Delivery = orderDto.Delivery;
                tblOrder.AdvanceAmount = orderDto.AdvanceAmount;
                tblOrder.DeliveryCharge = orderDto.DeliveryCharge;
                tblOrder.Design = orderDto.Design;
                tblOrder.DesignCharge = orderDto.DesignCharge;
                tblOrder.BalanceAmount = orderDto.BalanceAmount;
                tblOrder.TotalAmount = orderDto.TotalAmount;
                tblOrder.Units = orderDto.Units;
                tblOrder.Time = orderDto.Time;
                tblOrder.Status = (int) orderDto.Status;
                tblOrder.Images = orderDto.Images;

                if (isNew)
                    context.tbl_Orders.InsertOnSubmit(tblOrder);

                context.SubmitChanges();

                response.Result = orderDto;
                response.Status = ResultStatus.Ok;

            }
            catch(Exception ex)
            {
                response.Status = ResultStatus.ServerError;
            }

            return response;
        }
    }
}