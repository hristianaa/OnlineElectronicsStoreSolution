using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/users
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _userService.GetById(id);
            if (user == null)
                return NotFound(new { Message = "User not found." });

            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public IActionResult PostUser([FromBody] User user)
        {
            if (!ModelState.IsValid || user == null)
                return BadRequest(new { Message = "Invalid user data." });

            _userService.Create(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public IActionResult PutUser(int id, [FromBody] User user)
        {
            if (id != user.Id)
                return BadRequest(new { Message = "User ID mismatch." });

            var existing = _userService.GetById(id);
            if (existing == null)
                return NotFound(new { Message = "User not found." });

            _userService.Update(user);
            return Ok(new { Message = "User updated successfully." });
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var existing = _userService.GetById(id);
            if (existing == null)
                return NotFound(new { Message = "User not found." });

            _userService.Delete(id);
            return Ok(new { Message = "User deleted successfully." });
        }
    }
}
