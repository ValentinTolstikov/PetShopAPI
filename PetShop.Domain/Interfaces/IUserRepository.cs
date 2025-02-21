using PetShop.Domain.Entities;

namespace PetShop.Domain.Interfaces;

public interface IUserRepository
{
    public Task<User?> GetUserByUsernameAndPassword(string username, string password);
    public Task<User> CreateNew(User user);
    public Task<bool> UserExists(string username);
}