using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private static List<User> Users = new List<User>
        {
            new User { Id = 1, Name = "John Doe", Email = "john.doe@example.com" },
            new User { Id = 2, Name = "Jane Smith", Email = "jane.smith@example.com" }
        };

        // GET: api/user
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return Users;
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }
            return user;
        }

        // POST: api/user
        [HttpPost]
        public ActionResult<User> CreateUser(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Email))
            {
                return BadRequest(new { Message = "Name and Email are required" });
            }

            user.Id = Users.Max(u => u.Id) + 1;
            Users.Add(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // PUT: api/user/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateUser(int id, User updatedUser)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            if (string.IsNullOrWhiteSpace(updatedUser.Name) || string.IsNullOrWhiteSpace(updatedUser.Email))
            {
                return BadRequest(new { Message = "Name and Email are required" });
            }

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            return NoContent();
        }

        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            Users.Remove(user);
            return NoContent();
        }
    }
}
