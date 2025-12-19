using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IEmailSender emailSender;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AuthenticationService(UserManager<ApplicationUser> userManager , IConfiguration configuration , 
          IEmailSender emailSender ,
          SignInManager<ApplicationUser> signInManager
          
            )
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.emailSender = emailSender;
            this.signInManager = signInManager;
        }
        public async  Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(loginRequest.Email);
                if (user is null) 
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "invalid email",
                     };
                }
                if (await userManager.IsLockedOutAsync(user))
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "Account is locked , try again later"
                    };
                }
                var result = await signInManager.CheckPasswordSignInAsync(user , loginRequest.Password, true );
                if (result.IsLockedOut)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "Account locked due multiple failed attempts"
                    };
                }
                else if (result.IsNotAllowed)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "please confirm your email"
                    };
                }
                if(!result.Succeeded)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = " invalid password"
                    };
                }
                 
                return new LoginResponse()
                {
                    Success = true,
                    Message = "Login successful",
                    AccessToken = await GenerateAccessToken(user)
                };
            }
            catch(Exception ex) 
            {
                return new LoginResponse()
                {
                    Success = false,
                    Message = "An UnExpected Error..",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            try
            {
                var user = registerRequest.Adapt<ApplicationUser>();
                var result = await userManager.CreateAsync(user, registerRequest.Password);

                if (!result.Succeeded)
                {
                    return new RegisterResponse()
                    {
                        Success = false,
                        Message = "User Creation Failed",
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    };
                }
                await userManager.AddToRoleAsync(user, "User");
                //create token
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                token = Uri.EscapeDataString(token);
                var emailUrl = $"https://localhost:7077/api/auth/Account/ConfirmEmail?token={token}&userId={user.Id}";
                await emailSender.SendEmailAsync(user.Email ,"welcome" , $"<h1>welcome.. {user.UserName}</h1> " +
                    $"<a href='{emailUrl}'>confirm email</a>");
                return new RegisterResponse()
                {
                    Success = true,
                    Message = "Success"
                };
            }
            catch (Exception ex)
            {
                return new RegisterResponse()
                {
                    Success = false,
                    Message = "An UnExpected Error..",
                    Errors = new List<string> { ex.Message }
                };
            }
         }
        public async Task<bool> ConfirmEmailAsync(string token , string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is null) return false;
            var result = await userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded) 
            { 
                  return false;
            }
            return true;

              
        }

        private async Task<string> GenerateAccessToken(ApplicationUser user)
        {
            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier , user.Id),
                new Claim(ClaimTypes.Name , user.UserName),
                new Claim(ClaimTypes.Email , user.Email),

            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<ForgotPasswordResponse> RequestPasswordReset(ForgotPasswordRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null) 
            {
                return new ForgotPasswordResponse
                {
                    Success = false,
                    Message ="email not found"
                };
            }
            var random = new Random();
            var code = random.Next(1000 , 9999).ToString();
            user.CodeResetPassword = code;
            user.PasswordResetCodeExpiry = DateTime.UtcNow.AddMinutes(15);
            await userManager.UpdateAsync(user);
            await emailSender.SendEmailAsync(request.Email ,"reset password",$"<p>code is {code}</p>");

            return new ForgotPasswordResponse
            {
                Success = true,
                Message = "code sent to your email"
            };

        }




        public async Task<ResetPasswordResponse> ResetPassword(ResetPasswordRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new ResetPasswordResponse
                {
                    Success = false,
                    Message = "email not found"
                };
            }
            else if (user.CodeResetPassword != request.Code)
            {
                return new ResetPasswordResponse
                {
                    Success = false,
                    Message = "invalid code"
                };
            }

            else if (user.PasswordResetCodeExpiry < DateTime.UtcNow)
            {
                return new ResetPasswordResponse
                {
                    Success = false,
                    Message = "code expired"
                };
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user , token , request.NewPassword);
            if (! result.Succeeded)
            {
                return new ResetPasswordResponse
                {
                    Success = false,
                    Message = "password reset failed",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

             await emailSender.SendEmailAsync(request.Email, "change password", $"<p>your password is changed</p>");

            return new ResetPasswordResponse
            {
                Success = true,
                Message = "password reset Successfully"
            };

        }

    }
}   

