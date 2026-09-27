

using BankingApp.Domain.Enums;

namespace BankingApp.Application.DTOs
{
    public class RegisterResponse(string FirstName, string LastName, string Email, string PhoneNumber, string UserType)
    {
        public string FirstName { get; set; } = FirstName;
        public string LastName { get; set; } = LastName;
        public string Email { get; set; } = Email;
        public string PhoneNumber { get; set; } = PhoneNumber;
        public string UserType { get; set; } = UserType;

    }
}