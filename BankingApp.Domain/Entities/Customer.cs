using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.Domain.Entities
{
    public class Customer(string firstName, string lastName, string email, string phoneNumber, DateTime dateOfBirth)
    {
        [Key]
        public Guid ID { get; private set; } = Guid.NewGuid();
        [Required]
        public string FirstName { get; set; } = firstName;
        [Required]
        public string LastName { get; set; } = lastName;
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = email;
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = phoneNumber;
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateOfBirth { get; private set; } = dateOfBirth;
        private readonly List<Account> _accounts = [];



        public void AddAccount(Account account)
        {
            _accounts.Add(account);
        }
    }
}