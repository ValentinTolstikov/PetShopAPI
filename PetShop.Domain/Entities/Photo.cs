using System.ComponentModel.DataAnnotations;

namespace PetShop.Domain.Entities;

public class Photo
{
    [Key]
    public int Id { get; set; }
    public string AltName { get; set; }
    public byte[] Data { get; set; }
    
    public virtual ProductPhoto ProductPhoto { get; set; }
}