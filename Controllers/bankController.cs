using BankApp.EnumEntities;
using BankApp.Models;
using BankApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using X.PagedList;

namespace BankApp.Controllers
{
    [Route("[controller]")]
    public class bankController : Controller
    {
        private readonly ILogger<bankController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IAccountService _accountService;
        private readonly ICustomerService _customerService;
        private readonly ICookieService _cookieService;

        public bankController(
            ILogger<bankController> logger,
            ApplicationDbContext db,
            IAccountService accountService,
            ICustomerService customerService,
            ICookieService cookieService)
        {
            _logger = logger;
            _db = db;
            _accountService = accountService;
            _customerService = customerService;
            _cookieService = cookieService;
        }

        [AllowAnonymous]
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("Login")]
        public IActionResult Login()
        {
            if (TempData.ContainsKey("login"))
            {
                ViewBag.login = TempData["login"];
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestDto request, CancellationToken cancellationToken)
        {

            if (ModelState.IsValid)
            {
                var loginResult = await _customerService.LoginCustomerAsync(request.Email!, request.Password!, cancellationToken);
                if (loginResult)
                {
                    return RedirectToAction("security");
                }

            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View();
        }



        [Authorize]
        [Route("security")]
        public async Task<IActionResult> Security(CancellationToken cancellationToken)
        {
            var customerId = await _cookieService.GetUserId( cancellationToken);
            var currentCustomer = await _customerService.GetSecurityQuestionAsync(customerId, cancellationToken);
            return currentCustomer == null ? NotFound() : View(currentCustomer);
        }

        [Authorize]
        [HttpPost("Security")]
        public async Task<IActionResult> Security(string question, string answer, CancellationToken cancellationToken)
        {

            var customerId = await _cookieService.GetUserId( cancellationToken);
            var confirmation = await _customerService.ConfirmSecurityAnswerAsync(answer, customerId, cancellationToken);
            if (confirmation)
            {
                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.Message = "Wrong Security Answer";
            }

            return View();
        }


        [AllowAnonymous]
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(CustomerRegistrationRequestDto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(request); // Return the view with the request object to preserve entered data
            }
            await _customerService.CreateCustomerAsync(request, cancellationToken);
            TempData["login"] = "Registration Successful";
            return RedirectToAction("Login");
        }


        [Authorize]
        [Route("Dashboard")]
        public async Task<IActionResult> Dashboard(CancellationToken cancellationToken)
        {
            var customerId = await _cookieService.GetUserId( cancellationToken);

            var accounts = await _accountService.GetAllAccountsAsync(customerId, cancellationToken);
            return View(accounts);

        }

        [Authorize]
        [Route("Logout")]
        public async Task<IActionResult> LogoutAsync()
        {

            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");

        }

        [Authorize]
        [Route("Account")]
        public IActionResult Account()
        {
            return View();
        }

        [Authorize]
        [HttpGet("Account/{Id:int}")]
        public async Task<IActionResult> Account([FromRoute] int Id, int? page, CancellationToken cancellationToken)
        {
            var customerId = await _cookieService.GetUserId(cancellationToken);
            Account? accounts = Id <= 0 ? throw new ArgumentOutOfRangeException(nameof(Id)) 
                : await _accountService.GetAllAccountTranactionAsync(customerId, Id, cancellationToken);
            ViewBag.balance = accounts!.Amount;
            var pageNumber = page ?? 1;
            var onePageOfProducts = accounts.Transaction.ToPagedList(pageNumber, 5);
            ViewBag.OnePageOfProducts = onePageOfProducts;
            return View(onePageOfProducts);
        }


        [Route("Createtransaction")]
        public IActionResult Createtransaction()
        {
            return View();
        }

        [HttpPost("CreateTransaction")]
        public async Task<IActionResult> CreateTransaction(CreateTransactionRequestDto transaction, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var customerId = await _cookieService.GetUserId(cancellationToken);
                _ = transaction.TransactionType == nameof(TransactionType.Credit)
                            ? await _accountService.CreditAccountAsync(customerId, transaction.AccountNumber!, transaction.Amount, transaction.TransactionDetail!)
                            : await _accountService.DebitAccountAsync(customerId, transaction.AccountNumber!, transaction.Amount, transaction.TransactionDetail!);
                return Ok();
            }

            ModelState.AddModelError(string.Empty, "incorect entry");
            return BadRequest(ModelState);

        }

        [Authorize]
        [Route("profile")]
        public async Task<IActionResult> Profile(CancellationToken cancellationToken)
        {

            var customerId = await _cookieService.GetUserId(cancellationToken);
            var customer = await _customerService.GetCustomerAsync(customerId, cancellationToken) 
                ?? throw new NullReferenceException("customerID NOT FOUND");
            return View(customer);

        }

        [Authorize]
        [Route("Transfer")]
        public async Task<IActionResult> TransferAsync(CancellationToken cancellationToken)
        {
            var customerId = await _cookieService.GetUserId(cancellationToken);
            var customer = await _customerService.GetCustomerWithAccount(customerId, cancellationToken) 
                ?? throw new NullReferenceException("customer NOT FOUND");
            return View(customer);
        }
    }

}



