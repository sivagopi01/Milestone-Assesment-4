using MongoDB.Driver;
using chatapplication.Models;
using System.Threading.Tasks;

namespace chatapplication.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UserRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("chatapp");
            _usersCollection = database.GetCollection<User>("users");
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _usersCollection.Find(u => u.Username == username).FirstOrDefaultAsync();
        }

        public async Task<User> CreateUserAsync(User user)
        {
            await _usersCollection.InsertOneAsync(user);
            return user;
        }

        public async Task<User> UpdateUserAsync(string username, User updatedUser)
        {
            var update = Builders<User>.Update.Set(u => u.Password, updatedUser.Password);
            await _usersCollection.UpdateOneAsync(u => u.Username == username, update);
            return await GetUserByUsernameAsync(username);
        }

        public async Task<bool> DeleteUserAsync(string username)
        {
            var result = await _usersCollection.DeleteOneAsync(u => u.Username == username);
            return result.DeletedCount > 0;
        }
    }
}
