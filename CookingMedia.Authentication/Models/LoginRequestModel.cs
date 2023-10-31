using System.ComponentModel.DataAnnotations;

namespace CookingMedia.Authentication.Models;

public class LoginRequestModel
{
    [EmailAddress]
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
