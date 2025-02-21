namespace PetShop.Domain.Entities;

public class Photo
{
    public int Id { get; set; }
    public string AltName { get; set; }
    public byte[] Data { get; set; }
}