using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BankingApp.Domain.IdentityEntities
{
    public class ApplicationUser() : IdentityUser<Guid>
    {
        [PersonalData]
        public string? FirstName { get; set; }
        [PersonalData]
        public string? LastName { get; set; }
    }
}