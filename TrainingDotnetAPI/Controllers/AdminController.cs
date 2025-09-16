using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingDotnetAPI.Models;

namespace TrainingDotnetAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> logger;
        private readonly UserManager<AppUser> userManager;

        public AdminController( UserManager<AppUser> userManager, ILogger<AdminController> logger)
        {
            this.userManager=userManager;
            this.logger=logger;
        }

        [HttpGet("users")]  
        public async Task<IActionResult> GetUsers()
        {
            var users = await userManager.Users
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userDetails = new
            {
                user.Id,
                user.UserName,
                user.Email
            };
            return Ok(userDetails);
        }

        [HttpGet("user/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }
            var userDetails = new
            {
                user.Id,
                user.UserName,
                user.Email
            };
            return Ok(userDetails);
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded) return BadRequest("Failed to delete user.");

            return NoContent(); 
        }
    }
}
