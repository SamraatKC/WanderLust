using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using WanderLust.Common;
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
        private readonly AppSettings appSettings;
        private readonly UserService userService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly WanderlustDbx wanderLustDbx;
        private readonly EmailHelper emailHelper;



        public UserController(IOptions<AppSettings> _appSettings,
           EmailHelper _emailHelper,
            UserManager<ApplicationUser> _userManager,
            SignInManager<ApplicationUser> _signInManager,
            RoleManager<IdentityRole> _roleManager)
        {
            emailHelper = _emailHelper;
            appSettings = _appSettings.Value;
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


                string customerRole = Enum.GetName(typeof(Enums.RoleNames), 1);
                string defaultRoleName = string.IsNullOrEmpty(model.RoleName) ? customerRole : model.RoleName;
                int adminCodeIndex = model.FirstName.ToUpper().IndexOf(appSettings.AdminCode.ToUpper());
                if (adminCodeIndex > 0)
                {
                    string adminRole = Enum.GetName(typeof(Enums.RoleNames), 0);
                    defaultRoleName = string.IsNullOrEmpty(model.RoleName) ? adminRole : model.RoleName;
                    model.FirstName = model.FirstName.Substring(0, adminCodeIndex);
                }
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
                string password = Helper.GenerateRandomPassword();
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PasswordHash = password,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    IsFirstLogin = true,
                    IsPasswordReset = false


                    //PhoneNumber = model.PhoneNumber

                };
                #endregion

                #region Create User
                var result = await userManager.CreateAsync(user, user.PasswordHash);
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

                    #region Send Email Along With Password
                    string code = userManager.GenerateEmailConfirmationTokenAsync(user).Result;
                    code = System.Web.HttpUtility.UrlEncode(code);
                    string url = Url.Action(nameof(ConfirmEmail), "User", new { userid = user.Id, code = code }, Request.Scheme, Request.Host.ToString());

                    string verificationLink = string.Format("{0}{1}{2}{3}{4}", appSettings.JwtIssuer, appSettings.VerificationLink, user.Id, "&code=", code);
                    string htmlEmailBody = emailHelper.GetEmailBody(appSettings.EmailTemplate_AccountVerification);
                    htmlEmailBody = htmlEmailBody.Replace("{FirstName}", model.FirstName);
                    htmlEmailBody = htmlEmailBody.Replace("{Email}", model.Email);
                    htmlEmailBody = htmlEmailBody.Replace("{Password}", password);
                    htmlEmailBody = htmlEmailBody.Replace("{ActivationLink}", url);
                    emailHelper.SendEmail("Account Activation - Wanderlust Holidays", model.Email, htmlEmailBody);
                    #endregion

                    var res5 = await userManager.AddToRoleAsync(user, asp_role.Name);
                    return new ApiResponse(CustomResponseMessage.AccountVerificationLinkSent, StatusCodes.Status200OK);
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
                return new ApiResponse(ex.Message, StatusCodes.Status500InternalServerError);
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

                var res = await userManager.FindByEmailAsync(model.Email);
                if (res != null)
                {
                    var hasher = new PasswordHasher<ApplicationUser>();
                    hasher.VerifyHashedPassword(res,res.PasswordHash,model.Password);
                    if (!res.IsPasswordReset && res.PasswordHash != model.Password)
                    {
                        return new ApiResponse(new { code = 600, message = "User has not reset  default password", userid = res.Id }, StatusCodes.Status200OK);
                    }
                    else
                    {
                        var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                        if (result.Succeeded)
                        {
                            var appUser = userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                            var roles = await userManager.GetRolesAsync(appUser);
                            var token = string.Format("{0}", GenerateJwtToken(model.Email, appUser, roles));
                            var resp = new { Token = token };
                            return new ApiResponse(resp, 200);

                        }
                        else
                        {
                            return new ApiResponse(StatusCodes.Status500InternalServerError);
                        }
                    }
                }


                else
                {


                    return new ApiResponse(CustomResponseMessage.InvalidLoginAttempt, StatusCodes.Status404NotFound);
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }


        private object GenerateJwtToken(string email, ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("FirstName", user.FirstName),
                new Claim("FullName" , string.Format("{0} {1}", user.FirstName, user.LastName) ),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber?? ""),
                new Claim("Roles", string.Join(",",roles))
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.JwtKey));
            var key_phrase = appSettings.JwtKey;
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(appSettings.JwtExpireDays));
            var token = new JwtSecurityToken(
                appSettings.JwtIssuer,
                appSettings.JwtAudience,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail")]
        public async Task<ActionResult> ConfirmEmail(string userid, string code)
        {
            var errorResponse = new ApiResponse("Oops! some error occured while activating your account. Please contact system provider.", StatusCodes.Status500InternalServerError);
            if (userid == null || code == null)
            {
                return Redirect(appSettings.JwtAudience);
            }
            code = System.Web.HttpUtility.UrlDecode(code);
            IdentityResult result;
            try
            {
                var user = await userManager.FindByIdAsync(userid);
                if (user == null) return Redirect(appSettings.JwtAudience);
                result = await userManager.ConfirmEmailAsync(user, code);
            }
            catch (InvalidOperationException ioe)
            {
                // ConfirmEmailAsync throws when the userId is not found.
                return Redirect(appSettings.JwtAudience);
            }
            if (result.Succeeded)
            {
                return Redirect(appSettings.ActivatedPostUrl);

            }
            if (!result.Succeeded)
            {
                return Redirect(appSettings.JwtAudience);
            }
            return Redirect(appSettings.JwtAudience);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ResetPassword/{userid}/{newpassword}/{oldpassword}")]
        public async Task<ApiResponse> ResetPassword(string userid, string newpassword, string oldpassword = null)
        {
            try
            {
                var checkUser = await userManager.FindByIdAsync(userid);
                if (checkUser != null)
                {

                    var hasher = new PasswordHasher<ApplicationUser>();
                    if (!oldpassword.Equals("d967b23b"))//if not initial password reset
                    {
                        var res = hasher.VerifyHashedPassword(checkUser, checkUser.PasswordHash, oldpassword);
                        if (res > 0)
                        {
                            var changedPassword = await userManager.ChangePasswordAsync(checkUser, oldpassword, newpassword);
                            var isPasswordReset = await userService.IsPasswordReset(userid);
                            if (isPasswordReset.IsPasswordReset == true)
                            {
                                return new ApiResponse(new { code = 602, message = "Password Succefully reset" }, StatusCodes.Status200OK);
                            }
                            else
                            {
                                return new ApiResponse(CustomResponseMessage.InternalServerError, StatusCodes.Status500InternalServerError);
                            }

                        }
                        else
                        {
                            return new ApiResponse(CustomResponseMessage.InternalServerError, StatusCodes.Status500InternalServerError);
                        }
                    }
                    else
                    {
                        string token = await userManager.GeneratePasswordResetTokenAsync(checkUser);
                        var changePassword = await userManager.ResetPasswordAsync(checkUser, token, newpassword);
                        await wanderLustDbx.SaveChangesAsync();
                        if (changePassword != null)
                        {
                            return new ApiResponse(new { code = 602, message = "Password Succefully reset" }, StatusCodes.Status200OK);
                        }
                        else
                        {
                            return new ApiResponse(CustomResponseMessage.InternalServerError, StatusCodes.Status500InternalServerError);
                        }
                    }
                }

                else
                {
                    return new ApiResponse(new { code = 603, message = "Oops! some error occured" }, StatusCodes.Status200OK);

                }
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message, StatusCodes.Status500InternalServerError);
            }

            //return new ApiResponse(new { code = 605, message = "Password has been successfully changed" });

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ForgotPassword/{email}")]
        public async Task<ApiResponse> ForgotPassword(string email)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(email);

                string url = string.Format("{0}{1}/{2}/102", appSettings.AllowedOrigin, appSettings.PasswordReset, user.Id);
                if (url != null)
                {
                    #region Send Forgot Password Email
                    string htmlEmailBody = emailHelper.GetEmailBody(appSettings.EmailTemplate_ForgotPassword);
                    htmlEmailBody = htmlEmailBody.Replace("{FirstName}", user.FirstName);
                    htmlEmailBody = htmlEmailBody.Replace("{ResetPasswordLink}", url);
                    emailHelper.SendEmail("Password Reset - Wanderlust Holidays", user.Email, htmlEmailBody);
                    return new ApiResponse(CustomResponseMessage.PasswordResetLinkSent, StatusCodes.Status200OK);
                    #endregion
                }
                //if (user != null)
                //{

                //    var hasher = new PasswordHasher<ApplicationUser>();
                //    hasher.HashPassword(user, newpassword);
                //    string token = await userManager.GeneratePasswordResetTokenAsync(user);
                //    var changePassword = await userManager.ResetPasswordAsync(user, token, newpassword);
                //    await wanderLustDbx.SaveChangesAsync();
                //    if (changePassword != null)
                //    {
                //        return new ApiResponse(new { code = 604, message = "Password Succefully restored" }, StatusCodes.Status200OK);
                //    }
                //    else
                //    {
                //        return new ApiResponse(CustomResponseMessage.InternalServerError, StatusCodes.Status500InternalServerError);
                //    }
                //}
                //else
                //{
                //    return new ApiResponse(CustomResponseMessage.UserNotFound, StatusCodes.Status406NotAcceptable);
                //}
                //var changePassword = await userManager.ResetPasswordAsync(user, token, newpassword);

                //}
                else
                {
                    return new ApiResponse(CustomResponseMessage.UserNotFound, StatusCodes.Status406NotAcceptable);
                }


            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message, StatusCodes.Status500InternalServerError);
            }

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