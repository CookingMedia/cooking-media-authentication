namespace CookingMedia.Authentication.Models;

public class UserResponseModel
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Role { get; set; } = null!;
}
