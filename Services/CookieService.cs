
using System.Security.Claims;
using System.Threading.Tasks;

namespace BankApp.Services
{
    
    public class CookieService : ICookieService
    {
        private readonly ICustomerService _customerService;
        private readonly IHttpContextAccessor _contextAccessor;

        public CookieService(
         ICustomerService customerService,
         IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            _customerService = customerService;
        }
        public async Task<int> GetUserId( CancellationToken cancellationToken)
        {
            var email = _contextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value 
                ?? throw new NullReferenceException("Email not found");
            var customer = await  _customerService.GetCustomerWithEmailAsync(email, cancellationToken);
            return customer.Id;

        }
    }
}
