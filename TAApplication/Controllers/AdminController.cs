using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TAApplication.Areas.Data;

namespace TAApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private UserManager<TAUser> _um;
        public AdminController(ILogger<AdminController> logger, UserManager<TAUser> um)
        {
            _logger = logger;
            _um = um;
        }
        public IActionResult Roles()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Change_Role(string user_id, string role)
        {
            try
            {
                var user = await _um.FindByIdAsync(user_id);
                if (await _um.IsInRoleAsync(user, role))
                {
                    await _um.RemoveFromRoleAsync(user, role);
                }
                else 
                {
                    await _um.AddToRoleAsync(user, role);
                }
            }
            catch (Exception ex) {
                return NotFound(new { success = false, message = "failed!" });
            }
            
            return Ok(new { success = true, message = "added!" });
        }

    }
}
