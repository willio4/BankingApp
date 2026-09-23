using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using BankingApp.Domain.Enums;

namespace BankingApp.Application.DTOs
{
    public class RegisterRequest(string FirstName, string LastName, string Email, string PhoneNumber, string Password, string ConfirmPassword, DateTimeOffset DateOfBirth, UserType UserType)
    {
        [Required(ErrorMessage = "{0} can't be blank")]
        public string FirstName { get; set; } = FirstName;
        [Required(ErrorMessage = "{0} can't be blank")]
        public string LastName { get; set; } = LastName;
        [Required(ErrorMessage = "{0} can't be blank")]
        [EmailAddress(ErrorMessage = "{0} should be in a proper email address format")]
        public string Email { get; set; } = Email;
        [Required(ErrorMessage = "Phone can't be blank")]
        [RegularExpression(@"^\d{3}-?\d{3}-?\d{4}$", ErrorMessage = "Format: 000-000-0000/0000000000")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = PhoneNumber;
        [Required(ErrorMessage = "{0} can't be blank")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = Password;
        [Required(ErrorMessage = "{0} can't be blank")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = ConfirmPassword;

        [Required(ErrorMessage = "{0} can not be blank")]
        [DataType(DataType.DateTime)]
        public DateTimeOffset DateOfBirth { get; set; } = DateOfBirth;

        public UserType UserType { get; set; } = UserType;

        public RegisterResponse ToResponse()
        {
            return new RegisterResponse(FirstName, LastName, Email, PhoneNumber, UserType.ToString());
        }
    }

}