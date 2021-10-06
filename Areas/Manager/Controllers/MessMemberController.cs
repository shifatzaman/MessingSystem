using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Areas.Manager.Controllers
{

    [Area("Manager")]
    public class MessMemberController : Controller
    {
        private readonly ILogger<MessMemberController> _logger;

        public MessMemberController(ILogger<MessMemberController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult Meals()
        {
            return View();
        }

        public IActionResult ExtraMessings()
        {
            return View();
        }

        public IActionResult CafeterialBills()
        {
            return View();
        }

        public IActionResult UtilityBills()
        {
            return View();
        }

        public IActionResult MonthlyBill()
        {
            return View();
        }

        public IActionResult Rooms()
        {
            return View();
        }
    }
}
