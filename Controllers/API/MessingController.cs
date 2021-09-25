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
    public class MessingController : ControllerBase
    {
        private readonly ILogger<MessingController> _logger;
        private readonly IUserService _userService;
        private readonly IInventoryService _inventoryService;
        private readonly IMapper _mapper;
        private readonly IMessingService _messingService;

        public MessingController(ILogger<MessingController> logger,
                                 IUserService userService,
                                 IInventoryService inventoryService,
                                 IMapper mapper,
                                 IMessingService messingService)
        {
            _logger = logger;
            _userService = userService;
            _inventoryService = inventoryService;
            _mapper = mapper;
            _messingService = messingService;
        }

        [HttpPost]
        [Authorize]
        [Route("member/add")]
        public ResponseModel AddInventory([FromBody] MessMemberViewModel model)
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
                    var messMember = _mapper.Map<MessMemberViewModel, MessMember>(model);
                    messMember.DateOfEntry = DateTime.Now;
                    _messingService.AddMessMember(messMember);
                    response.Message = "Mess member added successfully";
                }
                else
                {
                    response.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error Occured";
                _logger.LogDebug(string.Format("Error in messing/member/add - {0}", ex.Message));
            }

            return response;
        }

        [HttpGet]
        [Authorize]
        [Route("member/list")]
        public ResponseModel GetMessingMembers(string searchString = null)
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
                    var members = _messingService.GetMessMembers(searchString);
                    var memberVMs = _mapper.Map<IList<MessMember>, IList<MessMemberViewModel>>(members);
                    return response.CreateSuccessRespone(memberVMs, "Member list generated");
                }
                else
                {
                    response.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error Occured";
                _logger.LogDebug(string.Format("Error in messing/member/list - {0}", ex.Message));
            }

            return response;
        }

        [HttpGet]
        [Authorize]
        [Route("member/meals/{date}")]
        public ResponseModel GetMeals(DateTime date)
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
                    var meals = _messingService.GetMemberMealsByDate(date);
                    return response.CreateSuccessRespone(meals, "Meals list generated");
                }
                else
                {
                    response.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error Occured";
                _logger.LogDebug(string.Format("Error in messing/meals/{0} - {1}", date, ex.Message));
            }

            return response;
        }

        [HttpPost]
        [Authorize]
        [Route("member/meals")]
        public ResponseModel AddMemberMeal([FromBody] AddMemberMealViewModel model)
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
                    _messingService.DeleteAllMeals(model.Date);
                    _messingService.AddMemberMeals(model.MemberMeals);
                    response.Message = "Mess member meal added successfully";
                }
                else
                {
                    response.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error Occured";
                _logger.LogDebug(string.Format("Error in messing/member/add - {0}", ex.Message));
            }

            return response;
        }

        [HttpPost]
        [Authorize]
        [Route("daily/add")]
        public ResponseModel AddDailyMessing([FromBody] AddDailyMessingViewModel model)
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
                    _messingService.AddDailyMessing(model);
                    response.Message = "Daily messing added successfully";
                }
                else
                {
                    response.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error Occured";
                _logger.LogDebug(string.Format("Error in messing/daily/add - {0}", ex.Message));
            }

            return response;
        }

        [HttpGet]
        [Authorize]
        [Route("daily/{date}")]
        public ResponseModel GetDailyMessings(DateTime date)
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
                    var meals = _messingService.GetDailyMessings(date);
                    return response.CreateSuccessRespone(meals, "Daily messing list generated");
                }
                else
                {
                    response.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error Occured";
                _logger.LogDebug(string.Format("Error in messing/daily/{0} - {1}", date, ex.Message));
            }

            return response;
        }
    }
}
