using EnterpriseTracker.Core.Common.Contract.Dto;
using EnterpriseTracker.Core.Security.Contract.Dto;
using EnterpriseTracker.Core.User.Contract;

namespace EnterpriseTracker.Api.User
{
    public interface IUserRepository
    {
        ResultDto<UserDto> Login(LoginDto loginDto);        
    }
}