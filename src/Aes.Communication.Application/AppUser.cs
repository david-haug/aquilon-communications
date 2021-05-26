using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aes.Communication.Application
{
    public class AppUser
    {
        public int UserId { get; set; }
        public int OrganizationId { get; set; }
        public int[] Roles { get; set; }

        public bool HasRole(int role)
        {
            return Roles.Contains(role);
        }

        public bool OrgIsHierarchy { get; set; }
        public int[] OrganizationIds { get; set; }

    }
}
