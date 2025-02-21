namespace PetShop.Domain.Entities;

public class ProductTag
{
    public int Id { get; set; }
    public int IdProduct { get; set; }
    public int IdTag { get; set; }
    public Product Product { get; set; }
    public Tag Tag { get; set; }
}