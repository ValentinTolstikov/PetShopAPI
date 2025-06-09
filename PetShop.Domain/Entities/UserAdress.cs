using System.ComponentModel.DataAnnotations;

namespace PetShop.Domain.Entities;

public class UserAdress
{
    [Key]
    public int Id { get; set; }
    public int IdUser { get; set; }
    public string City { get; set; }
    public string Streat { get; set; }
    public string House { get; set; }
    public string HouseAdditional { get; set; }
}