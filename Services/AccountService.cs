using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using BankApp.EnumEntities;
using BankApp.Models;
using BankApp.Repository;

namespace BankApp.Services
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<Account> _accountRepository;
        private readonly ApplicationDbContext _dbContext;

        public AccountService(IRepository<Account> accountRepository, ApplicationDbContext dbContext)
        {
            _accountRepository = accountRepository;
            _dbContext = dbContext;
        }
        public async Task CreateAccountAsync(string accountType, int customerId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(accountType))
            {
                throw new ArgumentException("Account type cannot be null or empty", nameof(accountType));
            }
            if (customerId <= 0)
            {
                throw new ArgumentException("Invalid customer ID", nameof(customerId));
            }
            var min = 25670918;
            var max = 26679786;
            Random random = new();
            var newAccount = new Account()
            {
                AccountType = accountType,
                AccountNumber = random.Next(min, max).ToString(),
                CustomerId = customerId
            };
            await _accountRepository.AddAsync(newAccount, cancellationToken);
        }

        public async Task<Account?> GetAllAccountTranactionAsync(int customerId, int accountId, CancellationToken cancellationToken)
        {
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.CustomerId == customerId && a.Id == accountId)
            ?? throw new NullReferenceException("account not found");

            if (account == null)
            {
                // Handle the case where account is not found
                return null; // or throw an exception, or handle it according to your application logic
            }
            await _dbContext.Entry(account).Reference(a => a.Customer).LoadAsync(cancellationToken);

            await _dbContext.Entry(account).Collection(a => a.Transaction!).LoadAsync(cancellationToken);

            return account;
        }
        public async Task<decimal> CreditAccountAsync(int customerId, string accountNumber, decimal amount, string transactionDetail)
        {
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.CustomerId == customerId && a.AccountNumber == accountNumber)
             ?? throw new NullReferenceException("account not found");
            account.CreditAccount(amount, transactionDetail);
            await _dbContext.SaveChangesAsync();
            return account.Amount;
        }
        public async Task<decimal> DebitAccountAsync(int customerId, string accountNumber, decimal amount, string transactionDetail)
        {
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.CustomerId == customerId && a.AccountNumber == accountNumber)
            ?? throw new NullReferenceException("account not found");
            account.DebitAccount(amount, transactionDetail);
            await _dbContext.SaveChangesAsync();

            return account.Amount;

        }
        public async Task<List<Account>> GetAllAccountsAsync(int customerId, CancellationToken cancellationToken)
        {
            var accounts = customerId <= 0
                            ? throw new ArgumentOutOfRangeException(nameof(customerId))
                            : await _accountRepository.GetAllAsync(cancellationToken);
            var userAccounts = accounts.Where(a => a.CustomerId == customerId).ToList();
            return userAccounts;
        }

    }
}