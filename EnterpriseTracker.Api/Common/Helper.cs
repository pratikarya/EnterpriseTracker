using EnterpriseTracker.Core.User.Contract;

namespace EnterpriseTracker.Api.Common
{
    public static class Helper
    {
        public static UserDto ConvertToDto(tbl_User tblUser)
        {
            var user = new UserDto();

            user.Id = tblUser.Id;
            user.FirstName = tblUser.FirstName;
            user.LastName = tblUser.LastName;
            user.Password = tblUser.Password;
            user.Mobile = tblUser.Mobile;
            user.Email = tblUser.Email;
            user.Status = (UserStatus)tblUser.Status;

            return user;
        }
    }
}