using MagicMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MagicMVC.Data
{
    public static class SeedData2
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new[] { Constants.OwnerRole, Constants.FranchiseeRole, Constants.CustomerRole };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = role });
                }
            }
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            await EnsureUserHasRole(userManager, "s3607162@student.rmit.edu.au", Constants.OwnerRole);
            await EnsureUserHasRole(userManager, "s3379758@student.rmit.edu.au", Constants.OwnerRole);
            await EnsureUserHasRole(userManager, "franchisee@example.com", Constants.FranchiseeRole);
            await EnsureUserHasRole(userManager, "customer@example.com", Constants.CustomerRole);
        }
        private static async Task EnsureUserHasRole(
        UserManager<ApplicationUser> userManager, string userName, string role)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user != null && !await userManager.IsInRoleAsync(user, role))
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}
