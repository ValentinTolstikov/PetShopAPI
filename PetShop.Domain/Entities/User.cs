namespace PetShop.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string email { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public byte[] Photo { get; set; }
    public int Role { get; set; }
}