

using BankingApp.Domain.Enums;

namespace BankingApp.Application.DTOs.User
{
    public class UserResponse(string FirstName, string LastName, string Email, string PhoneNumber)
    {
        public string FirstName { get; set; } = FirstName;
        public string LastName { get; set; } = LastName;
        public string Email { get; set; } = Email;
        public string PhoneNumber { get; set; } = PhoneNumber;
    }
}