using System.Linq;
using System.Threading.Tasks;
using Core.Authorization;
using Core.Models;
using Core.Models.Identity;
using Core.Models.UsersViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers
{
    // [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly CoreContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public UsersController(
            CoreContext dbContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var isAuthorized = User.IsInRole(Core.Models.Identity.Roles.Admin) || 
                               User.IsInRole(Constants.ContactAdministratorsRole);

            var currentUserId = _userManager.GetUserId(User);

            var dbUsers = (from dbUser in _dbContext.Users
                        select dbUser).ToList();
            var users = (from user in _userManager.Users
                        select user).ToList();
            var roles = (from role in _roleManager.Roles
                        select role).ToList();

            // Only approved contacts are shown UNLESS you're authorized to see them
            // or you are the owner.
            if (!isAuthorized)
            {
                dbUsers = dbUsers.FindAll(u => u.OwnerId == currentUserId);
                users = users.FindAll(u => u.Id == currentUserId);
            }

            // var missingDbUsers = (from userList in users
            //             join dbUserList in dbUsers on userList.Id equals dbUserList.OwnerId into joinedList
            //             from dbUserList_sub in joinedList.DefaultIfEmpty()
            //             select new { 
            //                 id = userList.Id,
            //                 email = userList.Email,
            //                 ownerId = dbUserList_sub == null ? System.String.Empty : dbUserList_sub.Id 
            //             }).ToList().Where(x => x.ownerId == System.String.Empty);
            // if (missingDbUsers.Count() != 0) {
            //     foreach(var user in missingDbUsers) {
            //         _dbContext.Users.Add(new User { OwnerId = user.id });
            //         _dbContext.SaveChanges();
            //     }
            //     // foreach(var user in users) {
            //     //     var selectedDbUser = dbUsers.Find(u => u.OwnerId == user.Id); /// _dbContext.Users.SingleOrDefault(u => u.Id == user.Id);
            //     //     if (selectedDbUser == null) {
            //     //         //selectedDbUser = _dbContext.Users.Add(new User { OwnerId = user.Id }).Entity;
            //     //         //_dbContext.SaveChanges();
            //     //     }
            //     // }
            //     dbUsers = (from dbUser in _dbContext.Users
            //         select dbUser).ToList();
            // }

            var model = new IndexViewModel(dbUsers, users, roles, _userManager);

            return View(model);
        }

        public IActionResult Edit(string id)
        {
            // var currentUser = await GetCurrentUserAsync();
            // if (currentUser == null)
            // {
            //     return View("Error");
            // }

            var dbUsers = (from dbUser in _dbContext.Users
                        select dbUser).ToList();
            var user = (from users in _userManager.Users
                        select users).ToList().Find(r => r.Id == id);
            var roles = (from role in _roleManager.Roles
                        select role).ToList();

            var model = new EditViewModel(user, dbUsers, roles, _userManager) {
                Id = id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditViewModel model)
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                return View("Error");
            }

            if (ModelState.IsValid)
            {
                var selectedDbUser = _dbContext.Users.SingleOrDefault(u => u.Id == model.User.User.Id);
                if (selectedDbUser == null) {
                    selectedDbUser = _dbContext.Users.Add(new User { OwnerId = model.Id }).Entity;
                }
                selectedDbUser.FirstName = model.User.User.FirstName;
                selectedDbUser.LastName = model.User.User.LastName;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Roles(EditViewModel model)
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                return View("Error");
            }

            if (ModelState.IsValid)
            {
                var selectedDbUser = _dbContext.Users.SingleOrDefault(u => u.Id == model.User.User.Id);
                var user = (from users in _userManager.Users
                        select users).ToList().Find(r => r.Id == selectedDbUser.OwnerId);

                var dbUsers = (from dbUser in _dbContext.Users
                            select dbUser).ToList();
                var roles = (from role in _roleManager.Roles
                            select role).ToList();
                var userRoles = _userManager.GetRolesAsync(user).GetAwaiter().GetResult();

                var selectedRoles = model.User.Roles;
                foreach (var selectedRole in selectedRoles)
                {
                    var userRole = roles.Find(r => r.Id == selectedRole.Role.Id);
                    if (!(currentUser == user && Core.Models.Identity.Roles.Admin == userRole.Name)) { // Cannot change admin-group from own account
                        if (selectedRole.Selected && userRoles.FirstOrDefault(ur => ur == userRole.Name) == null) {
                            _userManager.AddToRoleAsync(user, userRole.Name).GetAwaiter().GetResult();
                        }
                        if (!selectedRole.Selected && userRoles.FirstOrDefault(ur => ur == userRole.Name) != null) {
                            _userManager.RemoveFromRoleAsync(user, userRole.Name).GetAwaiter().GetResult();
                        }
                    }
                }

                model = new EditViewModel(user, dbUsers, roles, _userManager) {
                    Id = model.User.User.Id
                };

            }

            return View(model);
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

    }
}