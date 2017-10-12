using System;
using System.Linq;
using Core.Models.Identity;

namespace Core.Models
{
    public static class CoreExtensions
    {
        public static void SeedData(this CoreContext context, bool isDevelopment = false, string ownerId = null)
        {
            if (context.AllMigrationsApplied())
            {

                if (context.Users.Any()) {
                    return;
                }

                var adminUser = context.Users.Add(new User { FirstName = "Admin", LastName = "User", OwnerId = ownerId }).Entity;

                context.Posts.Add(new Post { UserId = adminUser.Id, Content = "Welcome to the app!" });

                if (isDevelopment) {
                    var testUser1 = context.Users.Add(new User { FirstName = "Jan", LastName = "Janssen" }).Entity;
                    var testUser2 = context.Users.Add(new User { FirstName = "Karel", LastName = "Karels" }).Entity;
                    
                    context.Posts.AddRange(
                        new Post { UserId = testUser1.Id, Content = "What a piece of junk!" },
                        new Post { UserId = testUser2.Id, Content = "More junk!" },
                        new Post { UserId = testUser2.Id, Content = "A lot of junk!" }
                    );
                }

                context.SaveChanges();

            }
        }

        public static void SetLoggedIn(this CoreContext context, ApplicationUser user, bool login) {
            var selectedDbUser = context.Users.SingleOrDefault(u => u.OwnerId == user.Id);
            if (selectedDbUser == null) {
                selectedDbUser = context.Users.Add(new User { FirstName = "New", LastName = "User", OwnerId = user.Id }).Entity;
            }

            selectedDbUser.IsLoggedIn = login;
            if (login) {
                selectedDbUser.LastLoginDate = DateTime.UtcNow;
            }
            context.SaveChanges();

        }
    }
}