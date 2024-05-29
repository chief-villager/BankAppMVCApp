using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BankApp.Models;

namespace BankApp.Services
{
    public interface ICustomerService
    {

        Task CreateCustomerAsync(CustomerRegistrationRequestDto requestDto, CancellationToken cancellationToken);
        Task<Customer> GetCustomerWithAccount(int customerId, CancellationToken cancellationToken);
        Task DeleteCustomerAsync(int customerId, CancellationToken cancellationToken);
        Task<Customer> GetCustomerAsync(int CustomerId, CancellationToken cancellationToken);
        Task<bool> LoginCustomerAsync(string email, string password, CancellationToken cancellationToken);
        Task<Customer> GetSecurityQuestionAsync(int customerId, CancellationToken cancellationToken);
        Task<bool> ConfirmSecurityAnswerAsync(string answer, int customerId, CancellationToken cancellationToken);
        Task<Customer> GetCustomerWithEmailAsync(string email, CancellationToken cancellationToken);

    }
}