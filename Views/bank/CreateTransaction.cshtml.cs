using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BankApp.Views.bank
{
    public class CreateTransaction : PageModel
    {
        private readonly ILogger<CreateTransaction> _logger;

        public CreateTransaction(ILogger<CreateTransaction> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}