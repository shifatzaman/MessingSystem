﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Areas.Member.Controllers
{

    [Area("Member")]
    public class MealsController : Controller
    {
        private readonly ILogger<MealsController> _logger;

        public MealsController(ILogger<MealsController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult MonthlyBill()
        {
            return View();
        }
    }
}
