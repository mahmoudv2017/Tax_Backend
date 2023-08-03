using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class AppIdentityContextSeed
    {
        public static async Task SeedAsync(  UserManager<User> usermanager , RoleManager<IdentityRole> roleManager ,AppIdentityContext IdentityContext)
        {
            if(!roleManager.Roles.Any()) {

              await roleManager.CreateAsync(new IdentityRole("TaxPayer"));
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                
            }
            if(!usermanager.Users.Any())
            {
                var newUser = new User
                {
                    DisplayName = "Mahmoud Hehsam",
                    UserName = "mahmoudv2012",
                    Role = "Admin",
                    PhoneNumber= "01202645204",
                    Email = "mahmoudv2012@gmail.com",
                    SSN = "2971110600232"
                };


               await usermanager.CreateAsync(newUser,"Vcut2020@");
                await usermanager.AddToRoleAsync(newUser, newUser.Role);
                var newTaxPayer = new TaxPayer
                {
                    User = newUser,
                };

                await IdentityContext.TaxPayers.AddAsync(newTaxPayer);
                await IdentityContext.SaveChangesAsync();


            }

            var newUser2 = new User
            {
                DisplayName = "omar Hehsam",
                UserName = "omarv2012",
                Role = "TaxPayer",
                PhoneNumber = "01202645204",
                Email = "omarv2012@gmail.com",
                SSN = "2971110600234"
            };


            await usermanager.CreateAsync(newUser2, "Vcut2020@");
            await usermanager.AddToRoleAsync(newUser2, newUser2.Role);
            var newTaxPayer2 = new TaxPayer
            {
                User = newUser2,
            };

            await IdentityContext.TaxPayers.AddAsync(newTaxPayer2);
            await IdentityContext.SaveChangesAsync();

        }
    }
}
