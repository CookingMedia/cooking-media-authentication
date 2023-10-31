using System.ComponentModel.DataAnnotations;

namespace CookingMedia.Authentication.Models;

public class SignupRequestModel
{
    [EmailAddress]
    public string Email { get; set; } = null!;
    [MinLength(1)]
    public string Password { get; set; } = null!;
    public string Name { get; set; } = null!;
}
