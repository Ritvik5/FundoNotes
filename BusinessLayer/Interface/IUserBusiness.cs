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
        public UserEntity UserRegister(UserRegistrationModel model);
        public UserEntity GetUser(GetUserModel getUserModel);
        public List<UserEntity> GetAllUsers();
        public string LogIn(UserLoginModel loginModel);
        public string GenerateJWTToken(string email,long userId);
        public string ForgotPassword(ForgotPasswordModel model);
        Task<bool> ResetPassword(ResetPasswordModel model, string email);
        Task<bool> DeleteUser(DeleteUserModel model);
    }
}
