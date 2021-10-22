using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class AccountsController : Controller
    {
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(ILogger<AccountsController> logger)
        {
            _logger = logger;
        }

        public IActionResult ExtraMessing()
        {
            return View();
        }

        public IActionResult MonthlyBill()
        {
            return View();
        }

        public IActionResult CafeteriaBill()
        {
            return View();
        }

    }
}
