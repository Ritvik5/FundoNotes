using BusinessLayer.Interface;
using CommonLayer.Model;
using Experimental.System.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepoLayer.Interface;
using RepoLayer.Service;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FundoNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness userBusiness;

        public UserController(IUserBusiness userBusiness)
        {
            this.userBusiness = userBusiness;
        }


        [HttpPost]
        [Route("Register")]
        public IActionResult UserRegister(UserRegistrationModel model)
        {
            try
            {
                var result = userBusiness.UserRegister(model);
                if (result != null)
                {
                    return Ok(new { success = true, message = "User Registeration Sucessful", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "User Registeration Unsucessful" });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult UserLogin(UserLoginModel userLoginModel)
        {
            try
            {
                var result = userBusiness.LogIn(userLoginModel);
                if (result == null)
                {
                    return Unauthorized(new { success = false, message = "User is not Registered" });
                }
                else
                {
                    return Ok(new { success = true, message = "User Login Successfully", data = result });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        
        [HttpPost]
        [Route("forgotpassword/{email}")]
        public IActionResult ForgotPass(ForgotPasswordModel model)
        {
            try
            {
                var result = userBusiness.ForgotPassword(model);
                if (result != null)
                {
                    return Ok(new { success = true, message = "Please check your email for password reset instructions" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Something went wrong" });
                }
                
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email).Value.ToString();
                bool resetPassword = await userBusiness.ResetPassword(model, email);
                if (resetPassword)
                {
                    return Ok(new { success = true, message = "Password has been Updated" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Something went wrong" });
                }

            }
            catch (System.Exception)
            {

                throw;
            }
        }
        [HttpDelete("deleteuser")]
        public async Task<IActionResult> DeleteUserAsync(DeleteUserModel model)
        {
            try
            {
                bool deleteUser = await userBusiness.DeleteUser(model);
                if (deleteUser)
                {
                    return Ok(new { success = true, message = "User with " + model.Email + " deleted" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "User with " + model.Email + " can't be deleted" });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
