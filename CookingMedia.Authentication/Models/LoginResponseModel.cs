namespace CookingMedia.Authentication.Models;

public class LoginResponseModel
{
    public string Token { get; set; } = null!;
    public UserResponseModel User { get; set; } = null!;
}
