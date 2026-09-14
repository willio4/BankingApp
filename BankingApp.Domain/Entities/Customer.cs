using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
    public class Customer(Guid customerId, string firstName, string lastName, string email, string phoneNumber, DateTime dateOfBirth)
    {
        /// <summary>
        /// Customer unique identifier
        /// </summary>
        [Key]
        public Guid Id { get; private set; } = customerId;
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
        public string Email { get; set; } = email;
        /// <summary>
        /// customer phone number
        /// </summary>
        [Required]
        [DataType(DataType.PhoneNumber)]
        [StringLength(12)]
        public string PhoneNumber { get; set; } = phoneNumber;
        /// <summary>
        /// customer date of birth
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateOfBirth { get; private set; } = dateOfBirth;
        /// <summary>
        /// all accounts associated with customer
        /// </summary>
        private readonly List<Account> _accounts = [];

        /// <summary>
        /// adds account ownership to customer
        /// </summary>
        /// <param name="account">account to be added</param>
        public void AddAccount(Account account)
        {
            _accounts.Add(account);
        }

        public List<Account> GetAccounts()
        {
            return _accounts;
        }
    }
}