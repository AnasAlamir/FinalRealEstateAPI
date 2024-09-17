using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Services.Dto.User;

namespace FinalRealEstateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Get all users
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }

        // Get user by ID
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }

        // Create a new user
        [HttpPost]
        public IActionResult CreateUser([FromBody] UserInsertDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _userService.CreateUser(userDto);
            return Ok("User created successfully.");
        }

        // Update an existing user
        [HttpPut]
        public IActionResult UpdateUser([FromBody] UserUpdateDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _userService.UpdateUser(userDto);
            return Ok("User updated successfully.");
        }

        // Delete a user by ID
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            _userService.DeleteUser(id);
            return Ok("User deleted successfully.");
        }

        // Authenticate user by email and password
        [HttpGet("authenticate")]
        public IActionResult AuthenticateUser(string email, string password)
        {
            try
            {
                var user = _userService.AuthenticateUser(email, password);
                return Ok(user);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid email or password.");
            }
        }

        // Get user's properties
        [HttpGet("{userId}/properties")]
        public IActionResult GetUserProperties(int userId)
        {
            var properties = _userService.GetUserProperties(userId);
            return Ok(properties);
        }

        // Get user's inquiries
        [HttpGet("{userId}/inquiries")]
        public IActionResult GetUserInquiries(int userId)
        {
            var inquiries = _userService.GetUserInquiries(userId);
            return Ok(inquiries);
        }

        // Get user's favorites
        [HttpGet("{userId}/favorites")]
        public IActionResult GetUserFavorites(int userId)
        {
            var favorites = _userService.GetUserFavorites(userId);
            return Ok(favorites);
        }

        // Update user's password
        [HttpPut("{userId}/password")]
        public IActionResult UpdateUserPassword(int userId, [FromBody] string newPassword)
        {
            _userService.UpdateUserPassword(userId, newPassword);
            return Ok("Password updated successfully.");
        }
    }
}
