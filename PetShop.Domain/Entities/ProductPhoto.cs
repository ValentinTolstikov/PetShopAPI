using System.ComponentModel.DataAnnotations;

namespace PetShop.Domain.Entities;

public class ProductPhoto
{
    [Key]
    public int IdProductPhoto { get; set; }
    public int IdProduct { get; set; }
    public int IdPhoto { get; set; }
    public virtual Product Product { get; set; }
    public virtual Photo Photo { get; set; }
}