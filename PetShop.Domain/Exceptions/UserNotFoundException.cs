namespace PetShop.Domain.Exceptions;

public class UserNotFoundException(string message) : Exception(message);