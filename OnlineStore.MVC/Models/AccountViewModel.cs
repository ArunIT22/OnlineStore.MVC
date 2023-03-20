using System.ComponentModel.DataAnnotations;

namespace OnlineStore.MVC.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }

    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        [Compare("Password", ErrorMessage = "Password and Confirm Password doesn't match")]
        public string ConfirmPassword { get; set; } = null!;

        [Required]
        public string FirstName { get; set; } = null!;

        public string? LastName { get; set; }

        [Required]
        public string EmailId { get; set; } = null!;

        public string RoleName { get; set; } = "customer";
    }
}
