using System;
using System.Collections.Generic;
using Core.Models;
using Core.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Core.Models.UsersViewModels
{
    public class EditViewModel 
    {
        public EditViewModel() {

        }

        public EditViewModel(ApplicationUser user, List<User> users, List<IdentityRole> roles, UserManager<ApplicationUser> usermanager) : this()
        {
            if (user == null) throw new ArgumentNullException("user");
            if (users == null) throw new ArgumentNullException("users");
            if (roles == null) throw new ArgumentNullException("roles");
            if (usermanager == null) throw new ArgumentNullException("usermanager");

            this.Roles = roles;

            this.User = new UserRolesDbUsersViewModel(user, users, roles, usermanager.GetRolesAsync(user).GetAwaiter().GetResult());
        }

        public string Id { get; set; }

        public UserRolesDbUsersViewModel User { get; set; }
        public List<IdentityRole> Roles { get; set; }
    }
}