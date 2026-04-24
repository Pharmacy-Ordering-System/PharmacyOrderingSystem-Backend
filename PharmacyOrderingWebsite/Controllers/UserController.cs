using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyOrderingWebsite.DTOs;
using PharmacyOrderingWebsite.Services;
using System.Security.Claims;

namespace PharmacyOrderingWebsite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        //Get logged-in user ID
        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }


        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = GetUserId();

            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }


        [HttpPut("update")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserDto dto)
        {
            var userId = GetUserId();
            var result = await _userService.UpdateUserAsync(userId, dto);

            if (!result)
                return NotFound(new { success = false, message = "User not found" });

            //Return JSON instead of plain string
            return Ok(new { success = true, message = "Profile updated successfully" });
        }

        // 5️⃣ ADMIN → Delete user
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);

            if (!result)
                return NotFound("User not found");

            return Ok("User deleted successfully");
        }
        [HttpGet("{id}/details")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserFullDetails(int id)
        {
            var data = await _userService.GetUserFullDetails(id);
            return Ok(data);
        }
    }
}