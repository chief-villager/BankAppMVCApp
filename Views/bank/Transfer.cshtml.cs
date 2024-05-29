using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BankApp.Views.bank
{
    public class Transfer : PageModel
    {
        private readonly ILogger<Transfer> _logger;

        public Transfer(ILogger<Transfer> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}