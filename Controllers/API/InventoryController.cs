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
    public class InventoryController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;
        private readonly IInventoryService _inventoryService;
        private readonly IMapper _mapper;

        public InventoryController(ILogger<AccountController> logger,
                                 IUserService userService,
                                 IInventoryService inventoryService,
                                 IMapper mapper)
        {
            _logger = logger;
            _userService = userService;
            _inventoryService = inventoryService;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        [Route("add")]
        public ResponseModel AddInventory([FromBody] AddInventoryViewModel model)
        {
            var response = new ResponseModel();

            if (!ModelState.IsValid)
            {
                response.Message = "Inalid Input Parameter";
            }

            try
            {
                if (User != null &&  User.Identity != null)
                {
                    _inventoryService.AddOrUpdateInventoryItem(model);
                    return response.CreateSuccessRespone(null, "Bazar added successfully.");
                }
                else
                {
                    response.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error Occured";
                _logger.LogDebug(string.Format("Error in inventory/add - {0}", ex.Message));
            }

            return response;
        }

        [HttpGet]
        [Authorize]
        [Route("history")]
        public ResponseModel GetInventoryItemHistory()
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
                    var items = _inventoryService.GetInventoryItems();
                    return response.CreateSuccessRespone(items, "Bazar history generated.");
                }
                else
                {
                    response.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error Occured";
                _logger.LogDebug(string.Format("Error in inventory/history - {0}", ex.Message));
            }

            return response;
        }

        [HttpDelete]
        [Authorize]
        [Route("delete/{inventoryItemId}")]
        public ResponseModel DeleteInventoryItem(int inventoryItemId)
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
                    _inventoryService.DeleteInventoryItem(inventoryItemId);
                    return response.CreateSuccessRespone(null, "Bazar item deleted");
                }
                else
                {
                    response.Message = "Access Denied";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error Occured";
                _logger.LogDebug(string.Format("Error in inventory/delete - {0}", ex.Message));
            }

            return response;
        }


        [HttpPost]
        [Authorize]
        [Route("type/add")]
        public ResponseModel AddInventoryItemType([FromBody] AddInventoryItemTypeViewModel model)
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
                    if (model.ItemTypeId > 0)
                    {
                        _inventoryService.UpdateInventoryItemType(model);
                        return response.CreateSuccessRespone(null, "Store item updated successfully.");
                    }
                    else
                    {
                        _inventoryService.AddInventoryItemType(model);
                        return response.CreateSuccessRespone(null, "Store item added successfully.");
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
                _logger.LogDebug(string.Format("Error in inventory/type/add - {0}", ex.Message));
            }

            return response;
        }

        [HttpGet]
        [Authorize]
        [Route("type/{itemTypeId}")]
        public ResponseModel GetInventoryItemType(int itemTypeId)
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

                    bool hasAccess = false;
                    string accessMsg = string.Empty;

                    (hasAccess, accessMsg) = _userService.HasPermission(userId, Constants.IventoryAdd);

                    if (hasAccess)
                    {
                        var item = _inventoryService.GetInventoryItemType(itemTypeId);
                        var itemVM = _mapper.Map<InventoryItemType, FetchInventoryItemTypeViewModel>(item);
                        return response.CreateSuccessRespone(item, "Inventory item generated");
                    }
                    else
                    {
                        response.Message = accessMsg;
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
                _logger.LogDebug(string.Format("Error in inventory/type/{0} - {1}", itemTypeId, ex.Message));
            }

            return response;
        }

        [HttpGet]
        [Authorize]
        [Route("type/list")]
        public ResponseModel GetInventoryItemTypes()
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

                    bool hasAccess = false;
                    string accessMsg = string.Empty;

                    (hasAccess, accessMsg) = _userService.HasPermission(userId, Constants.IventoryAdd);

                    if (hasAccess)
                    {
                        var items = _inventoryService.GetInventoryItemTypes();
                        var itemVMs = _mapper.Map<IList<InventoryItemType>, IList<FetchInventoryItemTypeViewModel>>(items);
                        return response.CreateSuccessRespone(items, "Store items generated");
                    }
                    else
                    {
                        response.Message = accessMsg;
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
                _logger.LogDebug(string.Format("Error in inventory/type/list - {0}", ex.Message));
            }

            return response;
        }

        [HttpDelete]
        [Authorize]
        [Route("type/{itemTypeId}")]
        public ResponseModel DeleteInventoryItemType(int itemTypeId)
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

                    bool hasAccess = false;
                    string accessMsg = string.Empty;

                    (hasAccess, accessMsg) = _userService.HasPermission(userId, Constants.IventoryAdd);

                    if (hasAccess)
                    {
                        _inventoryService.DeleteInventoryItemType(itemTypeId);
                        return response.CreateSuccessRespone(string.Empty, "Store item deleted");
                    }
                    else
                    {
                        response.Message = accessMsg;
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
                _logger.LogDebug(string.Format("Error in inventory/type/{0} - {1}", itemTypeId, ex.Message));
            }

            return response;
        }

    }
}
