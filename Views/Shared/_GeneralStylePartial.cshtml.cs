using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BankApp.Views.Shared
{
    public class _GeneralStylePartial : PageModel
    {
        private readonly ILogger<_GeneralStylePartial> _logger;

        public _GeneralStylePartial(ILogger<_GeneralStylePartial> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}