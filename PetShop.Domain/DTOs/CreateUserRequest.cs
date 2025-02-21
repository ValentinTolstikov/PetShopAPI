namespace PetShop.Domain.DTOs;

public class CreateUserRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public int Role = 1;
}