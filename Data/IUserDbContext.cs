using System.Threading.Tasks;
using chatapplication.Models;

namespace chatapplication.Data
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(string username, User updatedUser);
        Task<bool> DeleteUserAsync(string username);
    }
}