namespace PetShop.DTOs;

public class UserInfoResponseDto
{
    public UserInfoResponseDto(string username, string email, string dateOfBirth)
    {
        Username = username;
        Email = email;
        DateOfBirth = dateOfBirth;
    }

    public string Username { get; set; }
    public string Email { get; set; }
    public string DateOfBirth { get; set; }
}