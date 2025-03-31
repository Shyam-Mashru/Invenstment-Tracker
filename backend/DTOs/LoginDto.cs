using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email Is Required...")]
        [EmailAddress(ErrorMessage = "Provide Vaild Email...")]
        public string UserEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password Is Required...")]
        public string Password { get; set; } = string.Empty;
    }
}
