using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using BankApp.EnumEntities;
using BankApp.Models;
using BankApp.Repository;
using Mapster;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace BankApp.Services
{
    public class CustomerService(IRepository<Customer> customerRepository, IPasswordhasher passwordhasher, IAccountService accountService, ApplicationDbContext dbContext, IHttpContextAccessor httpContext) : ICustomerService
    {

        private readonly IRepository<Customer> _customerRepository = customerRepository;
        private readonly IPasswordhasher _passwordhasher = passwordhasher;
        private readonly IAccountService _accountService = accountService;
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IHttpContextAccessor _httpContext = httpContext;

        public async Task CreateCustomerAsync(CustomerRegistrationRequestDto requestDto, CancellationToken cancellationToken)
        {
            _ = requestDto ?? throw new ArgumentNullException(nameof(requestDto));
            requestDto.PassWord = _passwordhasher.HashPassword(requestDto.PassWord!);
            requestDto.Answer = _passwordhasher.HashPassword(requestDto.Answer!);
            Customer customer = requestDto.Adapt<Customer>();
            var newCustomer = await _customerRepository.AddAsync(customer, cancellationToken);
            await _accountService.CreateAccountAsync(nameof(AccountType.checking), newCustomer.Id, cancellationToken);

        }

        public Task DeleteCustomerAsync(int customerId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Customer> GetCustomerAsync(int CustomerId, CancellationToken cancellationToken)
        {

            return await _customerRepository.GetByIdAsync(CustomerId) ?? throw new NullReferenceException("customer not found");
        }

        public async Task<bool> LoginCustomerAsync(string email, string password, CancellationToken cancellationToken)
        {
            try
            {
                var customer = (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                    ? throw new NullReferenceException("password or email cannot be null")
                    : await _dbContext.Customers.FirstOrDefaultAsync(c => c.Email == email, cancellationToken)
                    ?? throw new NullReferenceException("Customer not found");

                _passwordhasher.VerifyPassword(customer.PassWord!, password);

                if (customer.Id < 0)
                {
                    throw new NullReferenceException("customer not found");
                }

                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, customer.Id.ToString()),
                    new(ClaimTypes.Email,email),
                    new(ClaimTypes.Role, "Customer"),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.Now.AddMinutes(20)
                };

                await _httpContext.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<Customer> GetSecurityQuestionAsync(int customerId, CancellationToken cancellationToken)
        {
            return customerId <= 0 ?
                     throw new ArgumentOutOfRangeException(nameof(customerId))
                     : await _dbContext.Customers.FirstOrDefaultAsync(c => c.Id == customerId, cancellationToken)
                     ?? throw new NullReferenceException("customer not found");

        }

        public async Task<bool> ConfirmSecurityAnswerAsync(string answer, int customerId, CancellationToken cancellationToken)
        {
            var customer = customerId <= 0 ?
                       throw new ArgumentOutOfRangeException(nameof(customerId))
                       : await _dbContext.Customers.FirstOrDefaultAsync(c => c.Id == customerId, cancellationToken)
                       ?? throw new NullReferenceException("customer not found");

            return _passwordhasher.VerifyPassword(customer.Answer!, answer);
        }

        public async Task<Customer> GetCustomerWithAccount(int customerId, CancellationToken cancellationToken)
        {
            var customer = customerId <= 0 ?
                       throw new ArgumentOutOfRangeException(nameof(customerId))
                            : await _customerRepository.GetByIdAsync(customerId)
                            ?? throw new NullReferenceException("customer not found");
            _dbContext.Entry(customer).Collection(x => x.Account!).Load();
            return customer;

        }

        public async Task<Customer> GetCustomerWithEmailAsync(string? email, CancellationToken cancellationToken)
        {
            return string.IsNullOrEmpty(email!)
                            ? throw new ArgumentNullException(nameof(email))
                            : await _dbContext.Customers.FirstOrDefaultAsync(c => c.Email! == email, cancellationToken)
                            ?? throw new NullReferenceException("Customer not found");


        }
    }
}
