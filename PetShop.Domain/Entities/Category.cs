namespace PetShop.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public byte[] Image { get; set; }
}