using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<User> GetUserFromClaimsEmail(this UserManager<User> userManager , 
            ClaimsPrincipal user )
        {
            var email = user.FindFirstValue(ClaimTypes.Email);
           return await userManager.Users.Include(x => x.Address).FirstOrDefaultAsync(user => user.Email == email);
        }
    }
}
