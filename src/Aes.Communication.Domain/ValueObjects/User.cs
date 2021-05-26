﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Aes.Communication.Domain.ValueObjects
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
