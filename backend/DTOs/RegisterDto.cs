using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Username Is Required...")]
        [MinLength(2, ErrorMessage = "Username Must Be At Least 2 Characters...")]
        [MaxLength(16, ErrorMessage = "Username Must Be At Most 16 Characters...")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Username Can Only Contain Letters And Numbers...")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email Is Required...")]
        [EmailAddress(ErrorMessage = "Provide Vaild Email...")]
        public string UserEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password Is Required...")]
        [MinLength(8, ErrorMessage = "Password Must Be At Least 8 Characters...")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm Password Is Required...")]
        [Compare("Password", ErrorMessage = "Password Does Not Match...")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
