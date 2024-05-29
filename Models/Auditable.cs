using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BankApp.Models
{
    public class Auditable
    {
        public DateTime CreatedDate { get; set; }
        public Auditable()
        {
            CreatedDate = DateTime.Now;


        }
    }
}