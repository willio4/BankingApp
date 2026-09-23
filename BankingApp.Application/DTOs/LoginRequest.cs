using System.ComponentModel.DataAnnotations;

namespace BankingApp.Application.DTOs
{
    public class LoginRequest(string email, string password)
    {
        [Required(ErrorMessage = "Email can not be blank")]
        [EmailAddress(ErrorMessage = "Email not in correct format")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = email;
        [Required(ErrorMessage = "Password can not be blank")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = password;
    }
}