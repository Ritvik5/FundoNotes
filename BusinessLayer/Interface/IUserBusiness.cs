using CommonLayer.Model;
using RepoLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IUserBusiness
    {
        Task<UserEntity> UserRegister(UserRegistrationModel model);
        Task<LoginResultModel> LogIn(UserLoginModel userLoginModel);
        public string GenerateJWTToken(string email,long userId);
        Task<string> ForgotPassword(ForgotPasswordModel model);
        Task<bool> ResetPassword(ResetPasswordModel model, string email);
        Task<bool> DeleteUser(DeleteUserModel model);
    }
}
