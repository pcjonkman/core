using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Models.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Models
{
    public static class DbContextExtensions
    {
        public static bool AllMigrationsApplied(this DbContext context)
        {
            var applied = context.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var total = context.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            return !total.Except(applied).Any();
        }

        public static async void SeedData(this IApplicationBuilder app, bool isDevelopment, string password)
        {
            var context = app.ApplicationServices.GetService<ApplicationDbContext>();
            if (context.AllMigrationsApplied())
            {
                // The password is set with the following command:
                // dotnet user-secrets set SeedUserPW <pw>
                // The admin user can do anything

                var ownerId = app.EnsureUser(password, "admin@pcjonkman.nl", "admin@pcjonkman.nl");

                foreach (var role in Roles.All)
                {
                    await app.EnsureRole(await ownerId, role);
                }

                app.ApplicationServices.GetService<CoreContext>().SeedData(isDevelopment, await ownerId);
            }

        }

       private static async Task<string> EnsureUser(this IApplicationBuilder app, string password, string username, string email)
        {
            var userManager = app.ApplicationServices.GetService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                user = new ApplicationUser { UserName = username, Email = email };
                var result = await userManager.CreateAsync(user, password);
                if (!result.Succeeded) {
                    var errors = result.Errors.ToList();
                    var errorMessage = "";
                    if (errors.Count > 0) {
                        errorMessage = errors[0].Code + ":" + errors[0].Description;
                    }
                    Console.WriteLine("Error:Cannot create user:" + errorMessage);
                }
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(this IApplicationBuilder app, string uid, string role)
        {
            IdentityResult IR = null;
            var roleManager = app.ApplicationServices.GetService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = app.ApplicationServices.GetService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByIdAsync(uid);

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }        
    }
}
