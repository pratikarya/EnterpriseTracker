using EnterpriseTracker.Api.Controllers.Common;
using EnterpriseTracker.Api.Order;
using EnterpriseTracker.Core.Common.Contract.Dto;
using EnterpriseTracker.Core.Order.Contract.Dto;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EnterpriseTracker.Api.Controllers
{
    public class OrderController :  BaseController
    {
        public IOrderRepository OrderRepository { get; set; }

        [HttpPost]
        public HttpResponseMessage UpdateOrder(SearchDto<OrderDto> requestDto)
        {
            if (ModelState.IsValid)
                return Request.CreateResponse(HttpStatusCode.OK, OrderRepository.UpdateOrder(requestDto));

            return CreateResponse(HttpStatusCode.BadRequest, ModelState, requestDto);
        }
    }
}