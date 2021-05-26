using System;
using System.Collections.Generic;
using System.Text;

namespace Aes.Communication.Application.Users
{
    public interface IUserService
    {
        UserDto GetByUserName(string userName);
        UserDto Get(int id);
    }
}
