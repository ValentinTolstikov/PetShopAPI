using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace PetShop.Auth;

public class AuthOptions
{
    public const string ISSUER = "PetShopServer";
    public const string AUDIENCE = "PetShopClient";
    const string KEY = "WorkingWorkSuperSecretPasswordAndSomeMoreWords";
    public static SymmetricSecurityKey GetSymmetricSecurityKey() => 
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}