using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class User: IdentityUser
    {
        public string DisplayName { get; set; }
        public string SSN { get; set; }
        public DateTime DateOfBirth { get; set; }
        public virtual Address Address { get; set; }
        public string Role { get; set; }

        public virtual TaxPayer taxPayer { get; set; }

        public virtual Admin admin { get; set; }

    }
}
