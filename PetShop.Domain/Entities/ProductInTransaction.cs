namespace PetShop.Domain.Entities;

public class ProductInTransaction
{
    public int Id { get; set; }
    public int IdProduct { get; set; }
    public int IdTransaction { get; set; }
    public double ProductSalingPrice { get; set; }
    public int SalingCount { get; set; }
}