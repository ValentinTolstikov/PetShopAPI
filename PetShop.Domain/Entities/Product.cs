namespace PetShop.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int IdManufacturer { get; set; }
    public float Price { get; set; }
    public int CountInStock { get; set; }
    public int IdCategory { get; set; }
}