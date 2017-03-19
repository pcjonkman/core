using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
 
namespace Core.Controllers
{
    [Route("api/[controller]")]
    public class SampleDbController : Controller
    {
        private readonly CoreContext _context;
 
        public SampleDbController(CoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Get()
        {
            var users = await _context.Users
                .Include(u => u.Posts)
                .ToArrayAsync();
 
            var response = users.Select(u => new
            {
                firstName = u.FirstName,
                lastName = u.LastName,
                posts = u.Posts.Select(p => p.Content)
            });
 
            return Json(response);
        }
    }
}