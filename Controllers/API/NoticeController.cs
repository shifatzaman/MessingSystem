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
    public class NoticeController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;
        private readonly IUserService _userService;
        private readonly IInventoryService _inventoryService;
        private readonly IMapper _mapper;
        private readonly IMessingService _messingService;
        private readonly INotificationService _notificationService;
        private readonly INoticeService _noticeService;
        public NoticeController(ILogger<NotificationController> logger,
                                 IUserService userService,
                                 IInventoryService inventoryService,
                                 IMapper mapper,
                                 IMessingService messingService,
                                 INotificationService notificationService,
                                 INoticeService noticeService)
        {
            _logger = logger;
            _userService = userService;
            _inventoryService = inventoryService;
            _mapper = mapper;
            _messingService = messingService;
            _notificationService = notificationService;
            _noticeService = noticeService;
        }

        [HttpGet]
        [Authorize]
        [Route("list")]
        public ResponseModel GetNotices(bool dashboard = false)
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
                    var notices = _noticeService.GetNotices(dashboard);

                    return response.CreateSuccessRespone(notices, "notice list generated");
                    
                }
                else
                {
                    response.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error Occured";
                _logger.LogDebug(string.Format("Error in notice/list/ - {0}", ex.Message));
            }

            return response;
        }

        [HttpPost]
        [Authorize]
        [Route("new")]
        public ResponseModel AddNewNotice([FromBody] NoticeViewModel model)
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

                    if (model.Id > 0) 
                    {
                        var existingNotice = _noticeService.GetNotice(model.Id);
                        existingNotice.Date = model.Date;
                        existingNotice.Title = model.Title;
                        existingNotice.Message = model.Message;
                        existingNotice.CreatedBy = model.CreatedBy;
                        existingNotice.IsVisible = model.IsVisible;

                        _noticeService.UpdateNotice(existingNotice);

                        return response.CreateSuccessRespone(null, "Notice updated successfully");
                    }
                    else
                    {
                        var notice = new Notice();
                        notice.Date = model.Date;
                        notice.Title = model.Title;
                        notice.Message = model.Message;
                        notice.CreatedBy = userId;
                        notice.IsVisible = model.IsVisible;

                        _noticeService.AddNotice(notice);
                        return response.CreateSuccessRespone(null, "Notice updated successfully");
                    }

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


        [HttpDelete]
        [Authorize]
        [Route("delete/{noticeid}")]
        public ResponseModel DeleteNotice(int noticeid)
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

                    var notice = _noticeService.GetNotice(noticeid);

                    if (notice != null)
                    {
                        _noticeService.DeleteNotice(notice);
                        return response.CreateSuccessRespone(null, "Notice deleted successfully");
                    }
                    else
                    {
                        response.Message = "Notice not found with the given id";
                    }

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
    }
}
