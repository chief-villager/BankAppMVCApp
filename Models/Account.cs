using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApp.EnumEntities;
using Mapster;

namespace BankApp.Models
{
    public class Account : Auditable
    {
        public int Id { get; set; }
        public string? AccountType { get; set; }
        public decimal Amount { get; set; }
        public string? AccountNumber { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual ICollection<Transaction>? Transaction { get; set; }

        public void CreditAccount(decimal amount, string transactionDetails)
        {
            _ = amount <= 0 ? throw new ArgumentOutOfRangeException(nameof(amount), " amount invalid") : Amount += amount;
            Transaction ??= new List<Transaction>();
            Transaction transaction = new()
            {
                TransactionAmount = amount,
                TransactionDetail = transactionDetails,
                TransactionType = nameof(TransactionType.Credit),
                AccountId = Id

            };
            Transaction!.Add(transaction);
        }

        public void DebitAccount(decimal amount, string transactionDetails)
        {
            if (Amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "you have no balanmce left");
            };
            _ = Amount < amount ? throw new ArgumentOutOfRangeException(nameof(amount), " you do not have enough to debit") : Amount -= amount;
            Transaction ??= new List<Transaction>();
            Transaction transaction = new()
            {
                TransactionAmount = amount,
                TransactionDetail = transactionDetails,
                TransactionType = nameof(TransactionType.Debit),
                AccountId = Id,
            };
            Transaction?.Add(transaction);
        }
    }
}