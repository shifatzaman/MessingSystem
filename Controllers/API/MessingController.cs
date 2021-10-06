using AutoMapper;
using MessingSystem.Domain;
using MessingSystem.Enums;
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
        public ResponseModel AddMessMember([FromBody] MessMemberViewModel model)
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

                    if (messMember.Id > 0)
                    {
                        _messingService.UpdateMessMember(messMember);
                    }
                    else
                    {
                        messMember.DateOfEntry = DateTime.Now;
                        _messingService.AddMessMember(messMember);
                    }

                    //Add/Update user info
                    if (!string.IsNullOrWhiteSpace(model.Email))
                    {
                        if (messMember.UserId == 0)
                        {
                            var userId = _userService.AddUser(new UserViewModel
                            {
                                FirstName = model.Name,
                                Email = model.Email,
                                Password = model.Password,
                                Role = (int)UserRoles.Member
                            });

                            messMember.UserId = userId;

                            _messingService.UpdateMessMember(messMember);
                        }
                        else if (messMember.UserId > 0)
                        {
                            var user = _userService.GetUserById(messMember.UserId);
                            user.Email = model.Email;

                            if (!string.IsNullOrWhiteSpace(model.Password))
                            {
                                byte[] passwordHash, passwordSalt;
                                CommonUtilities.CreatePasswordHash(model.Password,out passwordHash,out passwordSalt);
                                user.PasswordHash = passwordHash;
                                user.PasswordSalt = passwordSalt;
                            }

                            _userService.UpdateUser(user);
                        }
                    }

                    return response.CreateSuccessRespone(null, "Member info saved successfully");

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
                    return response.CreateSuccessRespone(members, "Member list generated");
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
        [Route("member/{memberId}")]
        public ResponseModel GetMessMember(int memberId)
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
                    var member = _messingService.GetMessMemberById(memberId);
                    return response.CreateSuccessRespone(member, "Member generated");
                }
                else
                {
                    response.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error Occured";
                _logger.LogDebug(string.Format("Error in messing/member/{0} - {0}",memberId, ex.Message));
            }

            return response;
        }


        [HttpDelete]
        [Authorize]
        [Route("member/{memberId}")]
        public ResponseModel DeleteMessMember(int memberId)
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
                    var member = _messingService.GetMessMember(memberId);

                    if (member != null)
                    {
                        if (member.UserId > 0)
                        {
                            var user = _userService.GetUserById(member.UserId);

                            if (user != null)
                            {
                                user.IsDeleted = true;
                                _userService.UpdateUser(user);
                            }
                        }

                        _messingService.DeleteMessMember(member);

                        return response.CreateSuccessRespone(null, "Member Deleted");
                    }
                    else
                    {
                        response.Message = "Member not found";
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
                _logger.LogDebug(string.Format("Error in messing/member/{0} - {0}", memberId, ex.Message));
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
                    var userId = Convert.ToInt32(User.Identity.GetId());
                    int memberId = _userService.GetMemberId(userId);

                    var meals = _messingService.GetMemberMealsByDate(date, memberId);
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


        [HttpGet]
        [Authorize]
        [Route("extra/{memberId}")]
        public ResponseModel GetExtraMessings(int memberId)
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
                    var meals = _messingService.GetExtraMessings(memberId);
                    return response.CreateSuccessRespone(meals, "Extra messing list generated");
                }
                else
                {
                    response.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error Occured";
                _logger.LogDebug(string.Format("Error in messing/extra/{0} - {1}", memberId, ex.Message));
            }

            return response;
        }

        [HttpPost]
        [Authorize]
        [Route("extra/add")]
        public ResponseModel AddExtraMessing([FromBody] AddExtraMessingViewModel model)
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
                    var extraMessingData = _mapper.Map<AddExtraMessingViewModel, ExtraMessing>(model);
                    _messingService.AddExtraMessing(extraMessingData);
                    return response.CreateSuccessRespone(null, "Extra messing added successfully for this member");
                }
                else
                {
                    response.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error Occured";
                _logger.LogDebug(string.Format("Error in messing/extra/add - {0}", ex.Message));
            }

            return response;
        }

        [HttpDelete]
        [Authorize]
        [Route("cafe/{id}")]
        public ResponseModel DeleteCafeBill(int id)
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
                    _messingService.DeleteCafeteriaBill(id);
                    return response.CreateSuccessRespone(null, "Cafe bill deleted successfully for this member");
                }
                else
                {
                    response.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error Occured";
                _logger.LogDebug(string.Format("Error in messing/extra/add - {0}", ex.Message));
            }

            return response;
        }

        [HttpDelete]
        [Authorize]
        [Route("extra/{id}")]
        public ResponseModel DeleteExtraMessing(int id)
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
                    _messingService.DeleteExtraMessing(id);
                    return response.CreateSuccessRespone(null, "Extra messing deleted successfully for this member");
                }
                else
                {
                    response.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error Occured";
                _logger.LogDebug(string.Format("Error in messing/extra/add - {0}", ex.Message));
            }

            return response;
        }

        [HttpDelete]
        [Authorize]
        [Route("utility/{id}")]
        public ResponseModel DeleteUtilityBill(int id)
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
                    _messingService.DeleteUtilityBill(id);
                    return response.CreateSuccessRespone(null, "Utility Bill Deleted Successfully");
                }
                else
                {
                    response.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error Occured";
                _logger.LogDebug(string.Format("Error in messing/extra/add - {0}", ex.Message));
            }

            return response;
        }

        [HttpGet]
        [Authorize]
        [Route("cafe/{memberId}")]
        public ResponseModel GetCafeteriaBills(int memberId)
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
                    var meals = _messingService.GetCafeterialBills(memberId);
                    return response.CreateSuccessRespone(meals, "Cafeteria bill generated");
                }
                else
                {
                    response.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error Occured";
                _logger.LogDebug(string.Format("Error in messing/cafe/{0} - {1}", memberId, ex.Message));
            }

            return response;
        }

        [HttpPost]
        [Authorize]
        [Route("cafe/add")]
        public ResponseModel AddCafeteriaBill([FromBody] AddCafeteriaBillViewModel model)
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
                    var cafBillData = _mapper.Map<AddCafeteriaBillViewModel, CafeterialBill>(model);
                    _messingService.AddCafeterialBill(cafBillData);
                    return response.CreateSuccessRespone(null, "Cafeteria bill added successfully for this member");
                }
                else
                {
                    response.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error Occured";
                _logger.LogDebug(string.Format("Error in messing/cafe/add - {0}", ex.Message));
            }

            return response;
        }


        [HttpGet]
        [Authorize]
        [Route("utility/list")]
        public ResponseModel GetUtilityBills()
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
                    var utilityBills = _messingService.GetUtilityBills();
                    return response.CreateSuccessRespone(utilityBills, "Utility bill generated");
                }
                else
                {
                    response.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error Occured";
                _logger.LogDebug(string.Format("Error in messing/utility/list - {0}", ex.Message));
            }

            return response;
        }

        [HttpPost]
        [Authorize]
        [Route("utility/add")]
        public ResponseModel AddUtilityBill([FromBody] AddUtilityBillViewModel model)
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
                    var cafBillData = _mapper.Map<AddUtilityBillViewModel, UtilityBill>(model);
                    _messingService.AddUtilityBill(cafBillData);
                    return response.CreateSuccessRespone(null, "Utility bill added successfully for this member");
                }
                else
                {
                    response.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error Occured";
                _logger.LogDebug(string.Format("Error in messing/utility/add - {0}", ex.Message));
            }

            return response;
        }


        [HttpGet]
        [Authorize]
        [Route("bill/{memberId}/{startDate}/{endDate}")]
        public ResponseModel GetMessBillByDateRange(int memberId, DateTime startDate, DateTime endDate)
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
                    var messBill = _messingService.GetMessBill(memberId, startDate, endDate);
                    return response.CreateSuccessRespone(messBill, "Mess bill generated");
                }
                else
                {
                    response.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error Occured";
                _logger.LogDebug(string.Format("Error in messing/bill/{0}/{1}/{2} - {3}",memberId, startDate, endDate, ex.Message));
            }

            return response;
        }


        [HttpGet]
        [Authorize]
        [Route("rooms")]
        public ResponseModel GetRooms(bool includeVacantRoomsOnly = false)
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
                    var rooms = _messingService.GetRooms(includeVacantRoomsOnly);
                    var roomVMs = _mapper.Map<IList<Room>, IList<RoomViewModel>>(rooms);
                    return response.CreateSuccessRespone(roomVMs, "Room list generated");
                }
                else
                {
                    response.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error Occured";
                _logger.LogDebug(string.Format("Error in rooms - {0}", ex.Message));
            }

            return response;
        }

        [HttpGet]
        [Authorize]
        [Route("rooms/{roomId}")]
        public ResponseModel GetRoom(int roomId)
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
                    var room = _messingService.GetRoom(roomId);
                    var roomVM = _mapper.Map<Room, RoomViewModel>(room);
                    return response.CreateSuccessRespone(roomVM, "Room generated");
                }
                else
                {
                    response.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error Occured";
                _logger.LogDebug(string.Format("Error in rooms - {0}", ex.Message));
            }

            return response;
        }

        [HttpPost]
        [Authorize]
        [Route("rooms")]
        public ResponseModel AddRoom([FromBody] RoomViewModel model)
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
                    var room = _mapper.Map<RoomViewModel, Room>(model);

                    if (model.Id > 0)
                    {
                        if (!room.IsAllocated)
                        {
                            room.AllocatedTo = string.Empty;
                        }

                        _messingService.UpdateRoom(room);
                    }
                    else
                    {
                        if (!room.IsAllocated)
                        {
                            room.AllocatedTo = string.Empty;
                        }

                        _messingService.SaveRoom(room);
                    }
                    
                    return response.CreateSuccessRespone(null, "Room info saved");
                }
                else
                {
                    response.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error Occured";
                _logger.LogDebug(string.Format("Error in messing/utility/add - {0}", ex.Message));
            }

            return response;
        }

        [HttpDelete]
        [Authorize]
        [Route("rooms/{roomId}")]
        public ResponseModel RemoveRoom(int roomId)
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
                    _messingService.DeleteRoom(roomId);

                    return response.CreateSuccessRespone(null, "Room Deleted");
                }
                else
                {
                    response.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error Occured";
                _logger.LogDebug(string.Format("Error in messing/utility/add - {0}", ex.Message));
            }

            return response;
        }
    }
}
