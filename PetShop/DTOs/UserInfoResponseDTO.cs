namespace PetShop.DTOs;

public class UserInfoResponseDto
{
    public UserInfoResponseDto(string username, string email, string dateOfBirth, string photo)
    {
        Username = username;
        Email = email;
        DateOfBirth = dateOfBirth;
        Photo = photo;
    }

    public string Username { get; set; }
    public string Email { get; set; }
    public string DateOfBirth { get; set; }
    public string Photo { get; set; }
}