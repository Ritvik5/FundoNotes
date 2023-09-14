using BusinessLayer.Interface;
using CommonLayer.Model;
using RepoLayer.Entities;
using RepoLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IUserRepo userRepo;
        public UserBusiness(IUserRepo userRepo)
        {
            this.userRepo = userRepo;
        }

        public async Task<UserEntity> UserRegister(UserRegistrationModel model)
        {
            try
            {
                return await userRepo.UserRegister(model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string GenerateJWTToken(string email, long userId)
        {
            try
            {
                return userRepo.GenerateJWTToken(email,userId);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<LoginResultModel> LogIn(UserLoginModel userLoginModel)
        {
            try
            {
                return await userRepo.LogIn(userLoginModel);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<string> ForgotPassword(ForgotPasswordModel model)
        {
            try
            {
               return await userRepo.ForgotPassword(model);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> ResetPassword(ResetPasswordModel model, string email) 
        {
            try
            {
                return await userRepo.ResetPassword(model, email);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeleteUser(DeleteUserModel model)
        {
            try
            {
                return await userRepo.DeleteUser(model);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

    }
}

