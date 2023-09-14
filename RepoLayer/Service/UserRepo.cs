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
        /// <summary>
        /// User Registeration
        /// </summary>
        /// <param name="model">Registration Model</param>
        /// <returns>User Info</returns>
        public async Task<UserEntity> UserRegister(UserRegistrationModel model)
        {
            try
            {
                UserEntity userEntity = new UserEntity();
                userEntity.FirstName = model.FirstName;
                userEntity.LastName = model.LastName;
                userEntity.Email = model.Email;
                userEntity.Password = EncryptedPassword(model.Password);

                fundoContext.Users.Add(userEntity);
                await fundoContext.SaveChangesAsync();

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
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Generating JWT(JSON Web Token)
        /// </summary>
        /// <param name="email"> Email Id of User</param>
        /// <param name="userId"> User Id </param>
        /// <returns></returns>
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

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// User Login 
        /// </summary>
        /// <param name="userLoginModel"> Login Info </param>
        /// <returns> User Info with JWT Token </returns>
        public async Task<LoginResultModel> LogIn(UserLoginModel userLoginModel)
        {
            try
            {
                UserEntity userEntity = new UserEntity();
                userEntity = await fundoContext.Users.FirstOrDefaultAsync(u => u.Email  == userLoginModel.Email);

                if(userEntity == null)
                {
                    return null;
                }

                string decryptedPassword = DecryptedPassword(userEntity.Password);
                if(decryptedPassword == userLoginModel.Password)
                {
                    var token = GenerateJWTToken(userLoginModel.Email,userEntity.UserId);
                    LoginResultModel loginResult = new LoginResultModel
                    {
                        Token = token,
                        User = userEntity,
                    };
                    return loginResult;
                }
                else
                {
                    return null;
                }
                
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Encrypting User Password 
        /// </summary>
        /// <param name="password"> user password </param>
        /// <returns> Encrypted Password </returns>
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
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Decrypting user's encrypted password
        /// </summary>
        /// <param name="password"> Encrypted Password </param>
        /// <returns> Decrypted Password </returns>
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
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Forgot Password
        /// </summary>
        /// <param name="forgotPassword"> Email Id </param>
        /// <returns> Send JWT Token </returns>
        public async Task<string> ForgotPassword(ForgotPasswordModel forgotPassword)
        {
            try
            {
                UserEntity user = new UserEntity();
                user = await fundoContext.Users.SingleOrDefaultAsync(x => x.Email == forgotPassword.Email);
                if(user == null)
                {
                    return null;
                }
                var token = GenerateJWTToken(forgotPassword.Email,user.UserId);
                MsmqModel msmqModel = new MsmqModel();
                msmqModel.sendData2Queue(token);
                return token;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="resetPassword"> ResetPassword Info </param>
        /// <param name="email"> Email Id Of User</param>
        /// <returns> Boolena Value </returns>
        public async Task<bool> ResetPassword(ResetPasswordModel resetPassword,string email)
        {
            try
            {
                var user = await fundoContext.Users.SingleOrDefaultAsync(x => x.Email == email);
                if(user != null && resetPassword.NewPassword == resetPassword.ConfirmPassword)
                {
                    user.Password = EncryptedPassword(resetPassword.NewPassword);
                    await fundoContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Deleting User
        /// </summary>
        /// <param name="model"> Email Id Of User </param>
        /// <returns> Boolean Value </returns>
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
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

    }
}
