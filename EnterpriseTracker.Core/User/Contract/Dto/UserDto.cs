using System;

namespace EnterpriseTracker.Core.User.Contract.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public UserType Type { get; set; }
        public bool HasSignedUp { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public UserStatus Status { get; set; }
        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }

    public enum UserType
    {
        Customer,
        Admin
    }

    public enum UserStatus
    {
        Inactive,
        Active
    }
}
