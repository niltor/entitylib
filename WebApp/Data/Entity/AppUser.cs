using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Data.Entity
{
    public class AppUser : IdentityUser<Guid>
    {

        public List<Lib> Libs { get; set; }
        public List<Entity> Entities { get; set; }
    }
}
