using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApp.Models;

namespace BankApp.Services
{
    public interface IAccountService
    {
        Task CreateAccountAsync(string accountType, int customerId, CancellationToken cancellationToken);
        Task<Account?> GetAllAccountTranactionAsync(int Customerid, int accountId, CancellationToken cancellationToken);
        Task<decimal> CreditAccountAsync(int customerId, string accountNumber, decimal amount, string transactionDetail);
        Task<decimal> DebitAccountAsync(int customerId, string accountNumber, decimal amount, string transactionDetail);
        Task<List<Account>> GetAllAccountsAsync(int customerId, CancellationToken cancellationToken);
    }
}