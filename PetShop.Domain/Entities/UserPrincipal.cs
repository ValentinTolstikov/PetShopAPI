namespace PetShop.Domain.Entities;

public class UserPrincipal
{
    public int Id { get; set; }
    public string Token { get; set; }
    public int IdUser { get; set; }
    public DateTime UnfrasheDate { get; set; }
    public User User { get; set; }
}