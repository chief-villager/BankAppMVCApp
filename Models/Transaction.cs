using System.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BankApp.Models
{
    public class Transaction : Auditable
    {
        public Guid Id { get; set; }
        [Required]
        public string? TransactionType { get; set; }
        [Required]
        public string? TransactionDetail { get; set; }
        [Required]
        public decimal TransactionAmount { get; set; }
        public int AccountId { get; set; }
        public virtual Account? Account { get; set; }


    }
}