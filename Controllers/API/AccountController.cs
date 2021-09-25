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
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;


        public AccountController(ILogger<AccountController> logger,
                                 IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public ResponseModel Login([FromBody] LoginViewModel model)
        {
            var response = new ResponseModel();

            if (!ModelState.IsValid)
            {
                response.Message = "Inalid Input Parameter";
            }

            try
            {
                var existingUser = _userService.GetUserByEmail(model.Email);

                if (existingUser != null)
                {
                    if (CommonUtilities.VerifyPassword(model.Password, existingUser.PasswordHash, existingUser.PasswordSalt))
                    {
                        var token = CommonUtilities.GenereateJsonWebToken(existingUser);

                        if (!string.IsNullOrEmpty(token))
                        {
                            var data = new
                            {
                                token = token,
                                userdata = new
                                {
                                    firstName = existingUser.FirstName,
                                    lastName = existingUser.LastName,
                                    userId = existingUser.UserId,
                                    email = existingUser.Email
                                }
                            };

                            return response.CreateSuccessRespone(data, "Login Successful");
                        }
                        else
                        {
                            response.Message = "Failed to generate token";
                        }
                    }
                    else
                    {
                        response.Message = "Wrong Credentials";
                    }
                }
                else
                {
                    response.Message = "No user found with this email";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error Occured";
                _logger.LogDebug(string.Format("Error in account/login - {0}", ex.Message));
            }

            return response;
        }

    }
}
