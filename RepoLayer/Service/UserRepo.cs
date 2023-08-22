using CommonLayer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepoLayer.Context;
using RepoLayer.Entities;
using RepoLayer.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace RepoLayer.Service
{
    public class UserRepo : IUserRepo
    {
        private readonly FundoContext fundoContext;
        private readonly IConfiguration configuration;

        public UserRepo(FundoContext fundoContext, IConfiguration configuration)
        {
            this.fundoContext = fundoContext;
            this.configuration = configuration;
        }

        public UserEntity UserRegister(UserRegistrationModel model)
        {
            try
            {
                UserEntity userEntity = new UserEntity();
                userEntity.FirstName = model.FirstName;
                userEntity.LastName = model.LastName;
                userEntity.Email = model.Email;
                userEntity.Password = EncryptedPassword(model.Password);

                fundoContext.Users.Add(userEntity);
                fundoContext.SaveChanges();

                if(userEntity != null)
                {
                    return userEntity;
                }
                else 
                {
                    return null; 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserEntity GetUser(GetUserModel getUserModel)
        {
            try
            {
                UserEntity userEntity = new UserEntity();
                userEntity = fundoContext.Users.FirstOrDefault(u => u.UserId == getUserModel.UserId);

                if (userEntity != null)
                {
                    return userEntity;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<UserEntity> GetAlluser()
        {
            try
            {
                List<UserEntity> users = new List<UserEntity>();

                users = fundoContext.Users.ToList();

                if (users == null)
                {
                    return null;
                }
                else
                {
                    return users;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GenerateJWTToken(string email,long userId)
        {
            try
            {
                var tokenhandler = new JwtSecurityTokenHandler();

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));

                var tokenDescriptions = new SecurityTokenDescriptor
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(
                        new Claim[]
                        {
                        new Claim(ClaimTypes.Email,email),
                        new Claim("userId",userId.ToString())
                        }),
                    Expires = DateTime.Now.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                };

                var token = tokenhandler.CreateToken(tokenDescriptions);
                return tokenhandler.WriteToken(token);
            }   
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string LogIn(UserLoginModel userLoginModel)
        {
            try
            {
                UserEntity userEntity = new UserEntity();
                userEntity = fundoContext.Users.FirstOrDefault(u => u.Email  == userLoginModel.Email);

                if(userEntity == null)
                {
                    return null;
                }

                string decryptedPassword = DecryptedPassword(userEntity.Password);
                if(decryptedPassword == userLoginModel.Password)
                {
                    var token = GenerateJWTToken(userLoginModel.Email,userEntity.UserId);
                    return token;
                }
                else
                {
                    return null;
                }
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static string EncryptedPassword(string password)
        {
            try
            {
                if (string.IsNullOrEmpty(password))
                {
                    return null;
                }
                else
                {
                    byte[] storePassword = ASCIIEncoding.ASCII.GetBytes(password);
                    string encrytedPassword = Convert.ToBase64String(storePassword);
                    return encrytedPassword;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string DecryptedPassword(string password)
        {
            try
            {
                if (string.IsNullOrEmpty(password))
                {
                    return null;
                }
                else
                {
                    byte[] encryptedPassword = Convert.FromBase64String(password);
                    string decryptedPassword = ASCIIEncoding.ASCII.GetString(encryptedPassword);
                    return decryptedPassword;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string ForgotPassword(ForgotPasswordModel model)
        {
            try
            {
                UserEntity user = new UserEntity();
                user = fundoContext.Users.SingleOrDefault(x => x.Email == model.Email);
                if(user == null)
                {
                    return null;
                }
                var token = GenerateJWTToken(model.Email,user.UserId);
                MsmqModel msmqModel = new MsmqModel();
                msmqModel.sendData2Queue(token);
                return token;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> ResetPassword(ResetPasswordModel model,string email)
        {
            try
            {
                var resetPassword = await fundoContext.Users.SingleOrDefaultAsync(x => x.Email == email);
                if(resetPassword != null && model.NewPassword == model.ConfirmPassword)
                {
                    resetPassword.Password = EncryptedPassword(model.NewPassword);
                    await fundoContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> DeleteUser(DeleteUserModel model)
        {
            try
            {
                var deleteUser = await fundoContext.Users.SingleOrDefaultAsync(x=> x.Email == model.Email);
                if(deleteUser != null)
                {
                    fundoContext.Users.Remove(deleteUser);
                    await fundoContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
