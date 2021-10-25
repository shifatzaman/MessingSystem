using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Areas.Manager.Controllers
{

    [Area("Manager")]
    public class NoticeController : Controller
    {
        private readonly ILogger<NoticeController> _logger;

        public NoticeController(ILogger<NoticeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
