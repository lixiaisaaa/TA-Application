/**
* Author:    Xia Li
* Partner:   Wenlin Li
* Date:      10/17/2022
* Course:    CS 4540, University of Utah, School of Computing
* Copyright: CS 4540 and Xia Li and Wenlin Li - This work may not be copied for use in Academic Coursework.
*
* I, Xia Li and Wenlin Li, certify that I wrote this code from scratch and did 
* not copy it in part or whole from another source.  Any references used 
* in the completion of the assignment are cited in my README file and in
* the appropriate method header.
*
* File Contents
*
* This css file controller for admin.
*/

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
        public async Task<IActionResult> changeRole(string user_id, string role, string add_remove)
        {
            try
            {
                var user = await _um.FindByIdAsync(user_id);
                if (await _um.IsInRoleAsync(user, role) && add_remove == "remove")
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
