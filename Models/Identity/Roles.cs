using System.Collections.Generic;

namespace Core.Models.Identity
{
    public static class Roles
    {
        private static readonly string[] roles = { "Admin", "User" };

        public static string Admin { get { return roles[0]; } }

        public static string User { get { return roles[1]; } }

        public static IEnumerable<string> All { get { return roles; } }
    }
}