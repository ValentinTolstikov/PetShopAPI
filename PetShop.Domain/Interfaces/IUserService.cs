using PetShop.Domain.DTOs;
using PetShop.Domain.Entities;

namespace PetShop.Domain.Interfaces;

public interface IUserService
{
    public Task<AuthResponseDTO> Authorize(string username, string password);
    public Task<User> CreateUser(User user);
}