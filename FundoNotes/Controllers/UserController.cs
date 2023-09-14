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
    /// <summary>
    /// User Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness userBusiness;
        public UserController(IUserBusiness userBusiness)
        {
            this.userBusiness = userBusiness;
        }
        /// <summary>
        /// User Registration
        /// </summary>
        /// <param name="model"> User Info for new user </param>
        /// <returns> SMD(Status,Message,Data) Format </returns>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> UserRegister(UserRegistrationModel model)
        {
            try
            {
                var result = await userBusiness.UserRegister(model);
                if (result != null)
                {
                    return Ok(new { success = true, message = "User Registeration Sucessful", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "User Registeration Unsucessful" });
                }
            }
            catch (System.Exception ex)
            {

                throw new System.Exception(ex.Message);
            }
        }
        /// <summary>
        /// User Login
        /// </summary>
        /// <param name="userLoginModel"> Login info </param>
        /// <returns> SMD(Status,Message,Data) Format </returns>
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> UserLogin(UserLoginModel userLoginModel)
        {
            try
            {
                var result = await userBusiness.LogIn(userLoginModel);
                if (result == null)
                {
                    return Unauthorized(new { success = false, message = "User is not Registered" });
                }
                else
                {
                    return Ok(new { success = true, message = "User Login Successfully", data = result });
                }
            }
            catch (System.Exception ex)
            {

                throw new System.Exception(ex.Message);
            }
        }
        /// <summary>
        /// Forget Password
        /// </summary>
        /// <param name="model"> Forget Password model </param>
        /// <returns> SMD(Status,Message,Data) Format </returns>
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
            catch (System.Exception ex)
            {

                throw new System.Exception(ex.Message);
            }
        }
        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="model"> reset password model </param>
        /// <returns> SMD(Status,Message,Data) Format </returns>
        [Authorize]
        [HttpPut("resetpassword")]
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
            catch (System.Exception ex)
            {

                throw new System.Exception(ex.Message);
            }
        }
        /// <summary>
        /// Deleting User
        /// </summary>
        /// <param name="model"> email id of user </param>
        /// <returns> SMD(Status,Message,Data) Format </returns>
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
            catch (System.Exception ex)
            {

                throw new System.Exception(ex.Message);
            }
        }
    }
}
