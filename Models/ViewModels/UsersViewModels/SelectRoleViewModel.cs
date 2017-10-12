using System;
using System.Collections.Generic;
using Core.Models;
using Core.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Core.Models.UsersViewModels
{
    public class SelectRoleViewModel
    {
        public SelectRoleViewModel()
        {

        }
        public SelectRoleViewModel(IdentityRole role) : this()
        {
            if (role == null) throw new ArgumentNullException("role");

            this.Role = role;
        }

        public bool Selected { get; set; }
        public IdentityRole Role { get; set; }
    }
}