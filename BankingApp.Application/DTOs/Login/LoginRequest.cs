using System.ComponentModel.DataAnnotations;

namespace BankingApp.Application.DTOs
{
    public class LoginRequest(string Email, string Password)
    {
        [Required(ErrorMessage = "Email can not be blank")]
        [EmailAddress(ErrorMessage = "Email not in correct format")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = Email;
        [Required(ErrorMessage = "Password can not be blank")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = Password;
    }
}