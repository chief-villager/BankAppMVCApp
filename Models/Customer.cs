using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BankApp.Models
{
    public class Customer : Auditable
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("First Name")]
        [Required]
        public string? FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required]
        public string? LastName { get; set; }
        [Required]
        [DisplayName("Password")]
        public string? PassWord { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        public string? Question { get; set; }
        [Required]
        public string? Answer { get; set; }
        public bool IsLoggedIn { get; set; }
        public virtual ICollection<Account>? Account { get; set; }


    }
}