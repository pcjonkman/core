using System;
using System.Collections.Generic;
using Core.Models;
using Core.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Core.Models.UsersViewModels
{
    public class UserRolesDbUsersViewModel
    {
        public UserRolesDbUsersViewModel()
        {
            this.Roles = new List<SelectRoleViewModel>();
        }

        public UserRolesDbUsersViewModel(ApplicationUser user, List<User> dbusers, List<IdentityRole> roles, IList<string> inroles) : this()
        {
            if (user == null) throw new ArgumentNullException("user");
            if (dbusers == null) throw new ArgumentNullException("dbusers");
            if (roles == null) throw new ArgumentNullException("roles");
            if (inroles == null) throw new ArgumentNullException("inroles");

            this.ApplicationUser = user;

            // foreach (var users in dbusers)
            // {
            this.User = dbusers.Find(r => r.OwnerId == this.ApplicationUser.Id);
            // }

            // foreach (var role in inroles)
            // {
            //     this.Roles.Add(new SelectRoleViewModel(roles.Find(r => r.Name == role)));
            // }
            foreach (var role in roles)
            {
                this.Roles.Add(new SelectRoleViewModel(role));
            }
            
            foreach (var role in inroles)
            {
                var selUserRole = this.Roles.Find(r => r.Role.Name == role);
                selUserRole.Selected = true;
            }
        }

        public User User { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public List<SelectRoleViewModel> Roles { get; set; }
    }
}
