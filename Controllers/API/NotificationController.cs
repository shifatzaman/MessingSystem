using AutoMapper;
using MessingSystem.Domain;
using MessingSystem.Models;
using MessingSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Controllers.API
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;
        private readonly IUserService _userService;
        private readonly IInventoryService _inventoryService;
        private readonly IMapper _mapper;
        private readonly IMessingService _messingService;
        private readonly INotificationService _notificationService;

        public NotificationController(ILogger<NotificationController> logger,
                                 IUserService userService,
                                 IInventoryService inventoryService,
                                 IMapper mapper,
                                 IMessingService messingService,
                                 INotificationService notificationService)
        {
            _logger = logger;
            _userService = userService;
            _inventoryService = inventoryService;
            _mapper = mapper;
            _messingService = messingService;
            _notificationService = notificationService;
        }

        [HttpGet]
        [Authorize]
        public ResponseModel GetNotifications()
        {
            var response = new ResponseModel();

            if (!ModelState.IsValid)
            {
                response.Message = "Inalid Input Parameter";
            }

            try
            {
                if (User != null && User.Identity != null)
                {
                    var userId = Convert.ToInt32(User.Identity.GetId());
                    int unseenCount = 0;
                    var notifications = new List<Notification>();
                    (notifications, unseenCount) = _notificationService.GetNotifications(userId);
                    return response.CreateSuccessRespone(new {
                        notifications = notifications,
                        unseenCount = unseenCount
                    
                    }, "Notification list generated");
                }
                else
                {
                    response.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error Occured";
                _logger.LogDebug(string.Format("Error in notification/ - {0}", ex.Message));
            }

            return response;
        }

        [HttpGet]
        [Authorize]
        [Route("status/{notificationId}")]
        public ResponseModel UpdateNotificationSeenStatus(int notificationId)
        {
            var response = new ResponseModel();

            if (!ModelState.IsValid)
            {
                response.Message = "Inalid Input Parameter";
            }

            try
            {
                if (User != null && User.Identity != null)
                {
                    var userId = Convert.ToInt32(User.Identity.GetId());
                    _notificationService.UpdateSeenStatus(notificationId);
                    return response.CreateSuccessRespone(null, "Notification status updated");
                }
                else
                {
                    response.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error Occured";
                _logger.LogDebug(string.Format("Error in notification/status - {0}", ex.Message));
            }

            return response;
        }
    }
}
