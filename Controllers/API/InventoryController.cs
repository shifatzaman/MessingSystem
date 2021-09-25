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
                    response.Message = "Inventory item added successfully";
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
                    return response.CreateSuccessRespone(items, "Inventory history generated");
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
                    _inventoryService.AddInventoryItemType(model);
                    response.Message = "Inventory item type added successfully";
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
                    var items = _inventoryService.GetInventoryItemTypes();
                    var itemVMs = _mapper.Map<IList<InventoryItemType>, IList<FetchInventoryItemTypeViewModel>>(items);
                    return response.CreateSuccessRespone(items, "Inventory item types generated");
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

    }
}
