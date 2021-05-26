using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aes.Communication.Application;
using Microsoft.AspNetCore.Mvc;

namespace Aes.Communication.Api
{
    public class AppController:ControllerBase
    {
        public AppUser AppUser => ClaimsService.CreateAppUser(User.Claims);
    }
}
