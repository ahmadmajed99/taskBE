using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using task5.Models;
using task5.DatabaseConnections;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.CodeAnalysis.Scripting;
using Humanizer;
using Microsoft.Extensions.Hosting;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.X86;
using System.Security.AccessControl;
using NuGet.Common;

namespace task5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly DatabaseContext _context;

        IConfiguration _configuration;

        public UserController(IConfiguration configuration, DatabaseContext context)
        {
            _context = context;
            _configuration = configuration;
        }

        public class JwtSettings
        {
            public string Audience { get; set; }
            public string Issuer { get; set; }
            public string Key { get; set; }
            public string Subject { get; set; }
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] User model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _context.Users.FirstOrDefault(u => u.Username == model.Username);

            if (user == null)
            {
                return Ok(new {status = "fail", message = "Invalid username or password." });
            }

            // Check if the user exists and verify the password
            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
               return Ok(new {status = "fail", message = "Invalid username or password." });
            }

            // Create a JWT token
            var token = GenerateJwtToken(user);


            return Ok(new { token = token, message = "Login successful.", status = "success" });
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
               new Claim(ClaimTypes.Name, user.Username),
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],              
                claims,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }


}
