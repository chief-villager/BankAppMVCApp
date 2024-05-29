using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankApp.Models
{
    public class CustomerRegistrationRequestDto
    {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required]
        [DisplayName("House Address")]
        public string? Address { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? PassWord { get; set; }
        [Required]
        [DisplayName("Security Question")]
        public string? Question { get; set; }
        [Required]
        public string? Answer { get; set; }

    }
}