namespace PetShop.Domain.Entities;

public class Transaction
{
    public int Id { get; set; }
    public int IdUser { get; set; }
    public DateTime OrderDate { get; set; }
    public bool IsDeliver { get; set; }
    public bool IsDeleted { get; set; }
}