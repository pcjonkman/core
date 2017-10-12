using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class User
    {
        public string Id { get; set; }
 
        public string FirstName { get; set; }
 
        public string LastName { get; set; }

        public string OwnerId { get; set; }

        public bool IsLoggedIn { get; set; }

        public DateTime LastLoginDate { get; set; }

        public List<Post> Posts { get; set; }
    }
}