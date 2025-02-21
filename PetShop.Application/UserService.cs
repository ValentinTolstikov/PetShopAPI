using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PetShop.Domain.DTOs;
using PetShop.Domain.Entities;
using PetShop.Domain.Exceptions;
using PetShop.Domain.Interfaces;

namespace PetShop.Application;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<AuthResponseDTO> Authorize(string username, string password)
    {
        var user = await _userRepository.GetUserByUsernameAndPassword(username, password);
        
        if (user is  null)
            throw new UserNotFoundException("Invalid username or password");
        
        return new AuthResponseDTO() { Username = user.Username };
    }

    public async Task<User> CreateUser(User user)
    {
        if (DateOnly.FromDateTime(DateTime.Now).CompareTo(user.DateOfBirth) <= 0)
            throw new NotImplementedException();
        
        if (await _userRepository.UserExists(user.Username))
            throw new NotImplementedException();
        
        var createdUser = await _userRepository.CreateNew(user);
        
        if (createdUser is null)
            throw new UserNotFoundException("Invalid username or password");
        
        return createdUser;
    }
}