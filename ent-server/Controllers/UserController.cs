using ent_server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;

namespace ent_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly UserContext _userContext;

        public UserController (UserContext userContext)
        {
            _userContext = userContext;
        }


        [HttpPost]
        [Route("register")]
        async public Task<ActionResult> RegisterUser(User user)
        {
            if (_userContext.Users.Where(u => u.Email == user.Email).IsNullOrEmpty())
            {
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.Password = passwordHash;

                user.CreatedDate = DateTime.Now;

                await _userContext.AddAsync(user);
                await _userContext.SaveChangesAsync();

                return Ok(new { success = true });
            }

            return BadRequest(new { success = false, error = "Cannot create, user exists" });

        }

        [HttpPost]
        [Route("login")]
        public ActionResult LoginUser(String email, String password)
        {
            Console.WriteLine(email);
            Console.WriteLine(password);

            User? user = _userContext.Users.Where(u => u.Email == email).FirstOrDefault();

            if (user == null) { return BadRequest(new { success = false, error = "User does not exist" }); }

            if (BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return Ok(new { success = true, token = "ABC123" });
            }

            return BadRequest(new { success = false, error = "Incorrect password" });
        }
    }
}
