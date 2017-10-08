using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Core.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
 
namespace Core.Controllers
{
    [Route("api/[controller]")]
    public class SampleDbController : Controller
    {
        private readonly CoreContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SampleDbController(CoreContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // [Authorize]
        public async Task<IActionResult> Get()
        {
            // var currentUser = await GetCurrentUserAsync();
            // if (currentUser == null)
            // {
            //     return Json(new Array[] {});
            // }

            return Json(await GetUsers());
        }

        // [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Post post)
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                return Json(new Array[] {});
            }

            if (ModelState.IsValid)
            {
                if (post.User != null && post.User.Id != null) {
                    var selectedDbUser = _context.Users.SingleOrDefault(u => u.OwnerId == post.User.Id);
                    if (selectedDbUser != null && selectedDbUser.IsLoggedIn && checkWithCurrentUser(selectedDbUser)) {
                        _context.Posts.Add(new Post{ Id = Guid.NewGuid().ToString(), UserId = selectedDbUser.Id, Content = post.Content });
                        _context.SaveChanges();
                    }
                }
            }
 
            return Json(await GetUsers());
        }

        // [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] Post post)
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                return Json(new Array[] {});
            }

            if (ModelState.IsValid)
            {
                if (post.Id != null && post.User != null && post.User.Id != null) {
                    var selectedDbUser = _context.Users.SingleOrDefault(u => u.OwnerId == post.User.Id);
                    if (selectedDbUser != null && selectedDbUser.IsLoggedIn && checkWithCurrentUser(selectedDbUser)) {
                        var selectedDbPost = _context.Posts.SingleOrDefault(u => u.Id == post.Id);
                        _context.Remove(selectedDbPost);
                        _context.SaveChanges();
                    }
                }
            }
 
            return Json(await GetUsers());
        }

        private async Task<object> GetUsers() {
            var currentUser = await GetCurrentUserAsync();
            var dbUsers = await _context.Users
                        .Include(u => u.Posts)
                        .ToArrayAsync();
            var users = (from user in _userManager.Users
                        select user).ToList();

            return dbUsers.Select(dbu => new
            {
                firstName = dbu.FirstName,
                lastName = dbu.LastName,
                // isLoggedIn = dbu.IsLoggedIn,
                // isLoggedIn = (dbu.IsLoggedIn && isLastLogin(dbu) && Request.Cookies[".AspNetCore.Identity.Application"] != null) ? true : false,
                isLoggedIn = dbu.OwnerId != null ? users.FirstOrDefault(u => u.Id == dbu.OwnerId) == currentUser ? true : false : false,
                lastLoginDate = dbu.LastLoginDate,
                posts = dbu.Posts.Select(p => new { Id = p.Id, Content = p.Content}),
                user = dbu.OwnerId != null ? users.FirstOrDefault(u => u.Id == dbu.OwnerId) : null,
                roles = dbu.OwnerId != null ? _userManager.GetRolesAsync(users.FirstOrDefault(u => u.Id == dbu.OwnerId)).GetAwaiter().GetResult() : null
            });
             
        }

        private bool checkWithCurrentUser(Models.User user) {
            // var lastLoginDbUser = _context.Users.OrderByDescending(u => u.LastLoginDate).FirstOrDefault();
            var currentUserId = _userManager.GetUserId(User);
            var currentDbUser = _context.Users.FirstOrDefault(u => u.OwnerId == currentUserId);

            return user == currentDbUser;
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

    }
}