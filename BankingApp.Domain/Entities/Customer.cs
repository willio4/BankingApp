using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Domain.Enums;

namespace BankingApp.Domain.Entities
{
    /// <summary>
    /// Customer constructor
    /// </summary>
    /// <param name="firstName">customer first name</param>
    /// <param name="lastName">customer last name</param>
    /// <param name="email">customer email</param>
    /// <param name="phoneNumber">customer phone number</param>
    /// <param name="dateOfBirth">customer date of birth</param>
    public class Customer(Guid id, string firstName, string lastName, string email, string phoneNumber, DateTimeOffset dateOfBirth, CustomerStatus customerStatus = CustomerStatus.Active)
    {
        /// <summary>
        /// Customer unique identifier
        /// </summary>
        [Key]
        public Guid Id { get; private set; } = id;
        /// <summary>
        /// customer first name
        /// </summary>
        [Required]
        [StringLength(25)]
        public string FirstName { get; set; } = firstName;
        /// <summary>
        /// customer last name
        /// </summary>
        [Required]
        [StringLength(25)]
        public string LastName { get; set; } = lastName;
        /// <summary>
        /// customer email
        /// </summary>
        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(50)]
        [RegularExpression(@"^[\w\d.]+@[\w\d.]+$")]
        public string Email { get; set; } = email;
        /// <summary>
        /// customer phone number
        /// </summary>
        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\d{3}-?\d{3}-?\d{4}$")]
        public string PhoneNumber { get; set; } = phoneNumber;
        /// <summary>
        /// customer date of birth
        /// </summary>
        [Required]
        public DateTimeOffset DateOfBirth { get; set; } = dateOfBirth;

        public CustomerStatus CustomerStatus { get; set; } = customerStatus;
        /// <summary>
        /// all accounts associated with customer
        /// </summary>
        private readonly List<Account> _accounts = [];

        public IReadOnlyCollection<Account> Accounts => _accounts.AsReadOnly();

        /// <summary>
        /// adds account ownership to customer
        /// </summary>
        /// <param name="account">account to be added</param>
        public void AddAccount(Account account)
        {
            _accounts.Add(account);
        }
    }
}