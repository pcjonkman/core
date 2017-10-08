using System;
using System.Collections.Generic;
using Core.Models;
using Core.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Core.Models.UsersViewModels
{
    public class IndexViewModel 
    {
        public IndexViewModel()
        {
            this.dbUsers = new List<User>();
            this.Users = new List<UserRolesDbUsersViewModel>();
            this.Roles = new List<IdentityRole>();
        }

        public IndexViewModel(List<User> dbusers, List<ApplicationUser> users, List<IdentityRole> roles, UserManager<ApplicationUser> usermanager)
            : this()
        {
            if (dbusers == null) throw new ArgumentNullException("dbUsers");
            if (users == null) throw new ArgumentNullException("users");
            if (roles == null) throw new ArgumentNullException("roles");
            if (usermanager == null) throw new ArgumentNullException("usermanager");

            this.dbUsers = dbusers;
            this.Roles = roles;

            foreach(var user in users) {
                this.Users.Add(new UserRolesDbUsersViewModel(user, dbUsers, roles, usermanager.GetRolesAsync(user).GetAwaiter().GetResult()));
            }
        }
        
        public List<User> dbUsers { get; set; }
        public List<UserRolesDbUsersViewModel> Users { get; set; }
        public List<IdentityRole> Roles { get; set; }
    }
}