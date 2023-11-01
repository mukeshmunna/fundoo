using Microsoft.AspNetCore.Mvc;
using FundooManager.IManager;
using FundooModel.User;
using System.Threading.Tasks;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace FundooApplication.Controllers
{
    //[Route("api/[controller]")]
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUserManager userManager;
        private readonly IConfiguration configuration;

        public UserController(IUserManager userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }



        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> UserRegister(Register register)
        {
            try
            {
                var result = await this.userManager.RegisterUser(register);
                if (result == 1)
                {
                    return this.Ok(new { Status = true, Message = "User Registration Successful", data = register });
                }
                return this.BadRequest(new { Status = false, Message = "User Registration UnSuccessful" });
            }
            catch (Exception ex)
            {
                return this.NotFound(new { Status = false, Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("Login")]
        public ActionResult UserLogin(Login login)
        {
            try
            {
                var result = this.userManager.LoginUser(login);
                if (result != null)
                {
                  
                    return this.Ok(new { Status = true, Message = "User Login Successful", data = result });
                }
                return this.BadRequest(new { Status = false, Message = "User Login UnSuccessful" });
            }
            catch (Exception ex)
            {
                return this.NotFound(new { Status = false, Message = ex.Message });
            }
        }

        [HttpPut]
        [Route("ResetPassword")]
        public ActionResult UserResetPassword(ResetPassword resetPassword)
        {
            try
            {
                var result = this.userManager.ResetPassword(resetPassword);
                if (result != null)
                {
                    return this.Ok(new { Status = true, Message = "User password reset Successful", data = resetPassword });
                }
                return this.BadRequest(new { Status = false, Message = "User  password reset UnSuccessful" });
            }
            catch (Exception ex)
            {
                return this.NotFound(new { Status = false, Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("ForgetPassword")]
        public ActionResult ForgetPassword(string email)
        {
            try
            {
                var resultLog = this.userManager.ForgetPassword(email);
                if (resultLog != null)
                {
                    return Ok(new { success = true, message = "Reset Email Send" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Reset UnSuccessful" });
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        private string GenerateJWTToken(int userId, string email)
        {
            var secretkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Key"]));
            var signinCredentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
      {
          new Claim(ClaimTypes.Role, "user"),
          new Claim("Id", userId.ToString()),
          new Claim("Email", email)
      };
            var tokenOptionOne = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: signinCredentials);
            string token = new JwtSecurityTokenHandler().WriteToken(tokenOptionOne);
            return token;
        }
    }
}
