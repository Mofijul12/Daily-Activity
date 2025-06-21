using DailyActivities_Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DailyActivities_Web.Controllers
{
    [Authorize(Roles = "Admin")] // Ensure only Admin can access this controller
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Action to display a list of users (with their email)
        public async Task<IActionResult> UserList()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users); // Pass the users to the view
        }

        // Action to view activities for a specific user
        public async Task<IActionResult> UserActivities(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var userActivities = await _context.DailyActivities
                .Where(d => d.UserId == userId)
                .ToListAsync();

            ViewBag.UserName = user.UserName;
            return View(userActivities);
        }
    }
}
