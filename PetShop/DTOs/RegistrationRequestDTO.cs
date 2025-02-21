namespace PetShop.DTOs;

public class RegistrationRequestDTO
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string email { get; set; }
    public DateOnly DateOfBirth { get; set; }
}