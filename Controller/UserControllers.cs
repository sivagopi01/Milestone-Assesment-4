using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using chatapplication.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;

namespace chatapplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("MyCorsPolicy")]
    public class UserController : ControllerBase
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UserController(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("chatapp");
            _usersCollection = database.GetCollection<User>("users");
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromBody] User loginUser)
        {
            if (loginUser == null || string.IsNullOrEmpty(loginUser.Username) || string.IsNullOrEmpty(loginUser.Password))
            {
                return BadRequest("Invalid user credentials.");
            }

            var user = await _usersCollection.Find(u => u.Username == loginUser.Username && u.Password == loginUser.Password).FirstOrDefaultAsync();

            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(user);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<User>> GetUser(string username)
        {
            var user = await _usersCollection.Find(u => u.Username == username).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            if (user == null || string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("User details are required.");
            }

            var existingUser = await _usersCollection.Find(u => u.Username == user.Username).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                return Conflict("User already exists.");
            }

            await _usersCollection.InsertOneAsync(user);
            return CreatedAtAction(nameof(GetUser), new { username = user.Username }, user);
        }

        [HttpPut("{username}")]
        public async Task<IActionResult> UpdateUser(string username, User updatedUser)
        {
            var existingUser = await _usersCollection.Find(u => u.Username == username).FirstOrDefaultAsync();
            if (existingUser == null)
            {
                return NotFound();
            }

            var update = Builders<User>.Update
                .Set(u => u.Password, updatedUser.Password);

            await _usersCollection.UpdateOneAsync(u => u.Username == username, update);
            return NoContent();
        }

        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            var result = await _usersCollection.DeleteOneAsync(u => u.Username == username);
            if (result.DeletedCount == 0)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}