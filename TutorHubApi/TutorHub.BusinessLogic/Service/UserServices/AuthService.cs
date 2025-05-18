using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TutorHub.BusinessLogic.IServices;
using TutorHub.BusinessLogic.IServices.IUserServices;
using TutorHub.BusinessLogic.Models.User;
using TutorHub.BusinessLogic.Models.User.Teacher;
using TutorHub.DataAccess.Data;
using TutorHub.DataAccess.Entities;

namespace TutorHub.BusinessLogic.Service.UserServices
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public AuthService(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            IConfiguration configuration,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.context = context;
            this.configuration = configuration;
            this.mapper = mapper;
        }

        public async Task<(int, string, string)> Registration(RegistrationModel model, string role)
        {
            if (model == null)
            {
                return (0, "User is empty", "");
            }

            
            var userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                return (0, "User with this email already exists", "");
            }

            User user = mapper.Map<User>(model);

            var createUserResult = await userManager.CreateAsync(user, model.Password);
            if (!createUserResult.Succeeded)
            {
                return (0, $"User registration failed: {string.Join(", ", createUserResult.Errors.Select(e => e.Description))}", "");
            }

            if (!await roleManager.RoleExistsAsync(role))
            {
                _ = await roleManager.CreateAsync(new IdentityRole(role));
            }

            _ = await userManager.AddToRoleAsync(user, role);
            return (1, "User created successfully!", user.Id);
        }

        public async Task<(int, string, string?)> Login(LoginModel model)
        {
            if (model == null)
            {
                return (0, "User is empty", null);
            }

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return (0, "No such email", null);
            }

            if (!await userManager.CheckPasswordAsync(user, model.Password))
            {
                return (0, "Invalid password", null);
            }

            var userRoles = await userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
            };

            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));

                // Fetch the Teacher or Student Id from the database
                if (role == "Teacher")
                {
                    var teacher = await context.Teachers.FirstOrDefaultAsync(t => t.UserId == user.Id);
                    if (teacher != null)
                        authClaims.Add(new Claim("TeacherId", teacher.Id.ToString()));
                }
                else if (role == "Student")
                {
                    var student = await context.Students.FirstOrDefaultAsync(s => s.UserId == user.Id);
                    if (student != null)
                        authClaims.Add(new Claim("StudentId", student.Id.ToString()));
                }
            }

            string token = GenerateToken(authClaims);
            return (1, token, model.Email);
        }

        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = configuration["JWT:ValidIssuer"],
                Audience = configuration["JWT:ValidAudience"],
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
