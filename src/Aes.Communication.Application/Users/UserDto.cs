using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Application.Conversations;
using Aes.Communication.Domain.ValueObjects;

namespace Aes.Communication.Application.Users
{
    public class UserDto
    {
        public int UserId { get; set; }
        //public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        //public string CorespondenceEmail { get; set; }
        //public string OfficePhone { get; set; }
        //public string CellPhone { get; set; }
        //public string Fax { get; set; }
        //public string AddressLine1 { get; set; }
        //public string AddressLine2 { get; set; }
        //public string City { get; set; }
        //public string State { get; set; }
        //public string ZipCode { get; set; }
        //public string Country { get; set; }

        //public IEnumerable<UserRoleDto> Roles { get; set; }


        //public bool IsAquilon { get; set; }
        //public bool IsActive { get; set; }
        //public int OrganizationId { get; set; }

        public static UserDto Map(User user)
        {
            return new UserDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
        }
    }

    //public class UserRoleDto
    //{
    //    public int UserRoleId { get; set; }
    //    public int UserId { get; set; }
    //    public bool IsActive { get; set; }
    //    public int RoleId { get; set; }
    //    public string RoleAbbr { get; set; }
    //    public string RoleDescr { get; set; }
    //    public DateTime Created { get; set; }
    //    public int CreatedBy { get; set; }
    //    public DateTime Modified { get; set; }
    //    public int ModifiedBy { get; set; }
    //    public int SortSequenceUi { get; set; }
    //}

}
