using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using WanderLust.Data;
using WanderLust.Models.CommonModels;
using WanderLust.Models.DataModels;
using WanderLust.Models.ViewModels;
using WanderLust.Service;

namespace WanderLust.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IOptions<AppSettings> appSettings;
        private readonly UserService userService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly WanderlustDbx wanderLustDbx;



        public UserController(IOptions<AppSettings> _appSettings,
           
            UserManager<ApplicationUser> _userManager,
            SignInManager<ApplicationUser> _signInManager,
            RoleManager<IdentityRole> _roleManager)
        {
            appSettings = _appSettings;
            userService = new UserService(_appSettings);
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            wanderLustDbx = new WanderlustDbx(_appSettings);

        }

        #region Specific ASPNETIDENTITY Methods
        [HttpPost]
        [Route("CreateRole")]
        public async Task<ApiResponse> CreateRole(RoleViewModel model)
        {
            try
            {
                #region if role is not found or duplicate user found then return error message
                var asp_role = await roleManager.FindByNameAsync(model.Name);

                if (asp_role != null)
                    return new ApiResponse(CustomResponseMessage.DuplicateRole, StatusCodes.Status406NotAcceptable);

                var res = await roleManager.CreateAsync(new IdentityRole(model.Name));
                if (res.Succeeded)
                {
                    return new ApiResponse(CustomResponseMessage.UserCreated, StatusCodes.Status200OK);
                }
                #endregion

            }
            catch (Exception)
            {
                return new ApiResponse(CustomResponseMessage.InternalServerError, StatusCodes.Status500InternalServerError);
            }
            return new ApiResponse(CustomResponseMessage.UserCouldNotBeCreated, StatusCodes.Status406NotAcceptable);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ApiResponse> Register(UserViewModel model)
        {
            try
            {
                string defaultRoleName = string.IsNullOrEmpty(model.RoleName) ? "Admin" : model.RoleName;
                #region if role is not found or duplicate user found then return error message
                var asp_role = await roleManager.FindByNameAsync(defaultRoleName);
                var asp_user = await userManager.FindByEmailAsync(model.Email);
                if (asp_role == null)
                {
                    return new ApiResponse(CustomResponseMessage.RoleNotFound, StatusCodes.Status204NoContent);
                }
                if (asp_user != null)
                    return new ApiResponse(CustomResponseMessage.DuplicateUser, StatusCodes.Status406NotAcceptable);
                #endregion

                #region construct ApplicationUser
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PasswordHash = model.Password,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    //PhoneNumber = model.PhoneNumber

                };
                #endregion

                #region Create User
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    #region Twilio Functions
                    //const string accountSid = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
                    //const string authToken = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
                    //TwilioClient.Init(accountSid, authToken);

                    //var to = new PhoneNumber("+977" + model.PhoneNumber);

                    //var message = MessageResource.Create(
                    //    to,
                    //    from: new PhoneNumber("xxxxxxxxxxxxxxxxxxx"), //  From number, must be an SMS-enabled Twilio number ( This will send sms from ur "To" numbers ).  
                    //    body: $"Hello ! You have been successfully registered");



                    //Console.WriteLine(message.Sid);
                    #endregion

                    var res5 = await userManager.AddToRoleAsync(user, asp_role.Name);
                    return new ApiResponse(CustomResponseMessage.UserCreated, StatusCodes.Status200OK);
                }
                else
                {
                    if (result.Errors.Count() > 0)
                    {
                        string exceptionMessage = "{";
                        foreach (var error in result.Errors)
                        {
                            exceptionMessage += error.Description + ",";
                        }
                        exceptionMessage += "}";
                        var responseMessage = JsonConvert.SerializeObject(exceptionMessage);
                        return new ApiResponse(CustomResponseMessage.PasswordValidationFailed, responseMessage, StatusCodes.Status500InternalServerError);
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                return new ApiResponse(CustomResponseMessage.InternalServerError, StatusCodes.Status500InternalServerError);
            }
            return new ApiResponse(CustomResponseMessage.UserCouldNotBeCreated, StatusCodes.Status406NotAcceptable);
        }

        [HttpPost]
        [Route("AttemptAuth")]
        public async Task<ApiResponse> AttemptAuth(LoginViewModel model)
        {
            try
            {
                string commaSeperatedRoles = string.Empty;

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                if (result.Succeeded)
                {
                    var appUser = userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                    var roles = await userManager.GetRolesAsync(appUser);
                    var token = string.Format("{0}", GenerateJwtToken(model.Email, appUser));
                    var resp = new { Token = token, UserInfo = appUser, UserRoles = roles };
                    return new ApiResponse(resp, 200);

                }
                return new ApiResponse(CustomResponseMessage.InvalidLoginAttempt, StatusCodes.Status404NotFound);

            }
            catch (Exception ex)
            {
                return new ApiResponse(CustomResponseMessage.InternalServerError, StatusCodes.Status500InternalServerError);
            }
        }


        private object GenerateJwtToken(string email, ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name , user.UserName ),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Value.JwtKey));
            var key_phrase = appSettings.Value.JwtKey;
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(appSettings.Value.JwtExpireDays));
            var token = new JwtSecurityToken(
                appSettings.Value.JwtIssuer,
                appSettings.Value.JwtAudience,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion

        #region Other General Functions
        //[HttpGet]
        //[Route("GetAllUsers")]
        //public async Task<List<AspNetUser>> GetAllUsers()
        //{
        //    return await userService.GetAllUsers();
        //}

        //[HttpGet]
        //[Route("FindAllRoles")]
        //public async Task<ApiResponse> FindAllRoles(string id)
        //{
        //    try
        //    {
        //        var userDetail = await userService.FindUserById(id);
        //        if (userDetail != null)
        //        {
        //            return new ApiResponse(userDetail, 200);
        //        }
        //        return new ApiResponse(CustomResponseMessage.UnableToFindInformation, StatusCodes.Status404NotFound);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ApiResponse(CustomResponseMessage.InternalServerError, StatusCodes.Status500InternalServerError);
        //    }

        //}

        #endregion


    }
}