using System.Net;
using System.Net.Http;
using System.Web.Http;
using EnterpriseTracker.Api.User;
using EnterpriseTracker.Core.Security.Contract.Dto;
using EnterpriseTracker.Api.Controllers.Common;

namespace EnterpriseTracker.Api.Controllers
{
    public class UserController : BaseController
    {
        public IUserRepository UserRepository { get; set; }

        public UserController()
        {
            UserRepository = new UserRepository();
        }

        [HttpPost]
        public HttpResponseMessage Login(LoginDto requestDto)
        {
            if(ModelState.IsValid)
                return Request.CreateResponse(HttpStatusCode.OK, UserRepository.Login(requestDto));

            return CreateResponse(HttpStatusCode.BadRequest, ModelState, requestDto);
        }
    }
}