using CommonLayer.Model;
using RepoLayer.Context;
using RepoLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepoLayer.Interface
{
    public interface IUserRepo
    {
        public UserEntity UserRegister(UserRegistrationModel model);
        public UserEntity GetUser(GetUserModel getUserModel);

        public List<UserEntity> GetAlluser();
        public string LogIn(UserLoginModel loginModel);

        public string GenerateJWTToken(string email);

    }
}
