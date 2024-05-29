using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankApp.Models
{
    public class CreateTransactionRequestDto
    {
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public string? AccountNumber { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string? TransactionDetail { get; set; }
        [Required]
        public string? TransactionType { get; set; }
    }
}