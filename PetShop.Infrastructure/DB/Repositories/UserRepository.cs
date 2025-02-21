using Microsoft.EntityFrameworkCore;
using PetShop.Domain.Entities;
using PetShop.Domain.Interfaces;

namespace PetShop.Infrastructure.DB.Repositories;

public class UserRepository : IUserRepository
{
    private readonly PetShopContext _context;
    
    public UserRepository(PetShopContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByUsernameAndPassword(string username, string password)
    {
        return await _context.User.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
    }

    public async Task<User> CreateNew(User user)
    {
        await _context.User.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> UserExists(string username)
    {
        return (await _context.User.FirstOrDefaultAsync(u => u.Username == username)) is not null;
    }
}