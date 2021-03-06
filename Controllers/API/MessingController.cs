using AutoMapper;
using MessingSystem.Domain;
using MessingSystem.Enums;
using MessingSystem.Models;
using MessingSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly INotificationService _notificationService;
        private readonly IHostingEnvironment _environment;

        public MessingController(ILogger<MessingController> logger,
                                 IUserService userService,
                                 IInventoryService inventoryService,
                                 IMapper mapper,
                                 IMessingService messingService,
                                 INotificationService notificationService,
                                 IHostingEnvironment environment)
        {
            _logger = logger;
            _userService = userService;
            _inventoryService = inventoryService;
            _mapper = mapper;
            _messingService = messingService;
            _notificationService = notificationService;
            _environment = environment;
        }

        [HttpPost]
        [Authorize]
        [Route("member/add")]
        public ResponseModel AddMessMember(IFormFile file)
        {
            var response = new ResponseModel();

            if (!ModelState.IsValid)
            {
                response.Message = "Inalid Input Parameter";
            }

            try
            {
                var model = new MessMemberViewModel();

                if (Request.Form.ContainsKey("messmember"))
                {
                    model = JsonConvert.DeserializeObject<MessMemberViewModel>(Request.Form["messmember"]);
                }

                try
                {
                    if (file != null)
                    {
                        var fileContent = file;
                        if (fileContent != null && fileContent.Length > 0)
                        {
                            string path = Path.Combine(_environment.WebRootPath, "Uploads");

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            var newFileName =  string.Format("{0}{1}",Guid.NewGuid(), Path.GetExtension(file.FileName));
                            model.FileName = newFileName;

                            using (FileStream stream = new FileStream(Path.Combine(path, newFileName), FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }

                if (model != null)
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
                        if (!string.IsNullOrWhiteSpace(model.Email) || model.UserRole == (int)UserRoles.Admin)
                        {
                            if (messMember.UserId == 0)
                            {
                                var userId = _userService.AddUser(new UserViewModel
                                {
                                    FirstName = model.Name,
                                    Email = model.Email,
                                    Password = model.Password,
                                    Role = model.UserRole
                                });

                                messMember.UserId = userId;

                                _messingService.UpdateMessMember(messMember);
                            }
                            else if (messMember.UserId > 0)
                            {
                                var user = _userService.GetUserById(messMember.UserId);
                                user.Email = model.Email;
                                user.Role = model.UserRole;

                                if (!string.IsNullOrWhiteSpace(model.Password))
                                {
                                    byte[] passwordHash, passwordSalt;
                                    CommonUtilities.CreatePasswordHash(model.Password, out passwordHash, out passwordSalt);
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
                else
                {
                    response.Message = "Invalid Input";
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
        public ResponseModel GetMessingMembers(string searchString = null, bool includeAdminsOnly = false)
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
                    var members = _messingService.GetMessMembers(searchString, includeAdminsOnly);
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

                    var userId = Convert.ToInt32(User.Identity.GetId());

                    if (userId > 0)
                    {
                        var user = _userService.GetUserById(userId);
                        bool isValid = true;

                        if (user != null && user.Role == (int)UserRoles.Member)
                        {
                            var memberId = _userService.GetMemberId(userId);

                            if (memberId > 0)
                            {
                                var memberMealOnDate = _messingService.GetMemberMealsByDate(model.Date, memberId).FirstOrDefault();
                                var memberMeal = model.MemberMeals.Where(m => m.MemberId == memberId).FirstOrDefault();

                                if (memberMeal != null)
                                {
                                    //Check Breakfast TimeStamp

                                    if (memberMealOnDate == null)
                                    {
                                        if (!memberMeal.BreakFastEnabled)
                                        {
                                            isValid = CommonUtilities.IsMealStatusChangeValid(model.Date, (int)MealTypes.BreakFast);
                                        }

                                        if (!memberMeal.LunchEnabled && isValid)
                                        {
                                            isValid = CommonUtilities.IsMealStatusChangeValid(model.Date, (int)MealTypes.Lunch);
                                        }

                                        if (!memberMeal.TeaBreakEnabled && isValid)
                                        {
                                            isValid = CommonUtilities.IsMealStatusChangeValid(model.Date, (int)MealTypes.TeaBreak);
                                        }

                                        if (!memberMeal.DinnerEnabled && isValid)
                                        {
                                            isValid = CommonUtilities.IsMealStatusChangeValid(model.Date, (int)MealTypes.Dinner);
                                        }
                                    }
                                    else
                                    {
                                        if (memberMeal.BreakFastEnabled != memberMealOnDate.BreakFastEnabled)
                                        {
                                            isValid = CommonUtilities.IsMealStatusChangeValid(model.Date, (int)MealTypes.BreakFast);
                                        }

                                        if (memberMeal.LunchEnabled != memberMealOnDate.LunchEnabled && isValid)
                                        {
                                            isValid = CommonUtilities.IsMealStatusChangeValid(model.Date, (int)MealTypes.Lunch);
                                        }

                                        if (memberMeal.TeaBreakEnabled != memberMealOnDate.TeaBreakEnabled && isValid)
                                        {
                                            isValid = CommonUtilities.IsMealStatusChangeValid(model.Date, (int)MealTypes.TeaBreak);
                                        }

                                        if (memberMeal.DinnerEnabled != memberMealOnDate.DinnerEnabled && isValid)
                                        {
                                            isValid = CommonUtilities.IsMealStatusChangeValid(model.Date, (int)MealTypes.Dinner);
                                        }

                                    }

                                }

                            }
                        }

                        if (!isValid)
                        {
                            response.Message = "Failed to update meal status. Time has already expired to update or change meal status for this date";
                        }
                        else
                        {
                            _messingService.DeleteAllMeals(model.Date);
                            _messingService.AddMemberMeals(model.MemberMeals);
                            response = response.CreateSuccessRespone(null, "Mess member meal added successfully");

                            #region Add notification for admin
                            if (user != null && user.Role == (int)UserRoles.Member)
                            {
                                string notificationMsg = string.Format("{0} has updated his meal satus for {1}", user.FirstName + " " + user.LastName, model.Date.ToString("dd/MM/yyyy"));
                                string notificationUrl = "/Manager/MessMember/Meals";

                                _notificationService.AddNotification(userId, DateTime.Now, notificationMsg, notificationUrl, sendToAllAdmins: true);
                            }
                            #endregion
                        }

                    }
                    else
                    {
                        response.Message = "Unauthorized Access";
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
                    var itemExistsInStore = true;

                    if (model.DailyMessingItems != null && model.DailyMessingItems.Count > 0)
                    {
                        var itemTypes = _inventoryService.GetInventoryItemTypes();
                        foreach (var item in model.DailyMessingItems)
                        {
                            var itemInStore = itemTypes.Where(it => it.ItemTypeId == item.ItemType).FirstOrDefault();

                            if (itemInStore.Quantity < item.Quantity)
                            {
                                itemExistsInStore = false;
                                break;
                            }
                        }
                    }

                    if (!itemExistsInStore)
                    {
                        response.Message = "Not enough quantity of selected item available in store. For details check store section.";
                    }
                    else
                    {
                        _messingService.AddDailyMessing(model);
                        return response.CreateSuccessRespone(null, "Daily messing added successfully");
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
                _logger.LogDebug(string.Format("Error in messing/daily/add - {0}", ex.Message));
            }

            return response;
        }

        [HttpPost]
        [Authorize]
        [Route("template")]
        public ResponseModel AddDailyMessingTemplate([FromBody] DailyMessingTemplateViewModel model)
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
                    _messingService.AddDailyMessingTemplate(model);
                    return response.CreateSuccessRespone(null, "Template Saved Successfully");
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
        [Route("template")]
        public ResponseModel GetDailyMessingTemplates()
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
                    var templates = _messingService.GetDailyMessingTemplates();
                    var templateVMs = _mapper.Map<IList<DailyMessingTemplate>, IList<DailyMessingTemplateViewModel>>(templates);
                    return response.CreateSuccessRespone(templateVMs, "Daily messing template list generated");
                }
                else
                {
                    response.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error Occured";
                _logger.LogDebug(string.Format("Error in messing/template/ - {0}", ex.Message));
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

                    if (model.ItemType > 0)
                    {
                        var inventoryItem = _inventoryService.GetInventoryItemType(model.ItemType);

                        if (inventoryItem != null)
                        {
                            if (inventoryItem.Quantity >= extraMessingData.Quantity)
                            {
                                _messingService.AddExtraMessing(extraMessingData);
                                return response.CreateSuccessRespone(null, "Extra messing added successfully for this member");
                            }
                            else
                            {
                                response.Message = "Not enough quantity of selected item available in store. For details check store section.";
                            }
                        }
                        else
                        {
                            response.Message = "Selected item not found in store";
                        }
                    }
                    else
                    {
                        response.Message = "Selecting an item is required";
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
        public ResponseModel GetRooms()
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
                    var rooms = _messingService.GetRooms();
                    var roomVMs = _mapper.Map<IList<Room>, IList<RoomViewModel>>(rooms);
                    var roomStatisticVM = new RoomStatisticsViewModel(rooms);
                    var data = new
                    {
                        roomStats = roomStatisticVM,
                        rooms = roomVMs
                    };

                    return response.CreateSuccessRespone(data, "Room list generated");
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
                            room.DateOfEntry = null;
                        }

                        _messingService.UpdateRoom(room);
                    }
                    else
                    {
                        if (!room.IsAllocated)
                        {
                            room.AllocatedTo = string.Empty;
                            room.DateOfEntry = null;
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
