using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepoLayer.Interface;
using RepoLayer.Service;

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

        [HttpGet]
        [Route("Get")]
        public IActionResult GetUser(GetUserModel model)
        {
            try
            {
                var result = userBusiness.GetUser(model);
                if (result != null)
                {
                    return Ok(new { success = true, message = "User is in Database", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "User is Not in Databse" });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAllUser()
        {
            try
            {
                var result = userBusiness.GetAllUsers();
                if (result != null)
                {
                    return Ok(new { success = true, message = "Sucessful", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Databse is empty" });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
