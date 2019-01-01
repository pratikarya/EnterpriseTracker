using EnterpriseTracker.Api.Common;
using EnterpriseTracker.Core.Common.Contract.Dto;
using EnterpriseTracker.Core.Security.Contract.Dto;
using EnterpriseTracker.Core.User.Contract;
using System;
using System.Linq;

namespace EnterpriseTracker.Api.User
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public ResultDto<UserDto> Login(LoginDto loginDto)
        {
            var response = new ResultDto<UserDto>();

            var username = loginDto.Username;
            var password = loginDto.Password;

            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                response.Status = ResultStatus.ValidationError;

                return response;
            }

            try
            {

                var context = GetDataContext();

                var user = context.tbl_Users.FirstOrDefault(x => x.Email.Equals(username) && x.Password.Equals(password));

                if(user != null)
                {
                    response.Result = Helper.ConvertToDto(user);

                    return response;
                }

                response.Status = ResultStatus.ValidationError;
            }
            catch(Exception ex)
            {
                response.Status = ResultStatus.ServerError;
            }

            return response;
        }
    }
}