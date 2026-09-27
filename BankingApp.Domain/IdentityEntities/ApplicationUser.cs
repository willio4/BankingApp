using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BankingApp.Infrastructure.IdentityEntities
{
    public class ApplicationUser(string FirstName, string LastName, DateTimeOffset DateOfBirth) : IdentityUser<Guid>
    {
        [PersonalData]
        public string? FirstName { get; set; } = FirstName;
        [PersonalData]
        public string? LastName { get; set; } = LastName;
        [PersonalData]
        public DateTimeOffset DateOfBirth { get; set; } = DateOfBirth;
    }
}