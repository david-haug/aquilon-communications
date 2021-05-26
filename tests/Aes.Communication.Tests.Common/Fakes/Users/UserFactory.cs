using System;
using System.Collections.Generic;
using System.Text;
using Aes.Communication.Domain.ValueObjects;

namespace Aes.Communication.Tests.Common.Fakes.Users
{
    public class UserFactory
    {
        private User _user = new User
        {
            UserId = 1,
            FirstName = "Johnny",
            LastName = "Tester",
            Email = "jtester@aquiloninc.com"
        };

        public User Create()
        {
            return _user;
        }


    }
}
