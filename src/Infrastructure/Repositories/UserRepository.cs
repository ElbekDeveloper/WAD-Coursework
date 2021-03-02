using Core.Auth.Models;
using Core.Auth.Settings;
using Core.Repositories;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly ApplicationDbContext _dbContext;
        
        public UserRepository(UserManager<IdentityUser> userManager, JwtSettings jwtSettings, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _dbContext = dbContext;
        }
        public async Task<bool> CheckUserOwnsArticle(int articleId, string userId)
        {
            var article = await _dbContext.Articles.AsNoTracking().SingleOrDefaultAsync(a => a.Id == articleId);

            if (article is null)
            {
                return false;
            }

            if (article.AuthorId != userId)
            {
                return false;
            }
            return true;
        }

        public async Task<AuthResult> CreateAsync(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return new AuthResult
                {
                    Errors = new[] { "User with this email address exists" }
                };
            }
            var newUser = new IdentityUser
            {
                Email = email,
                UserName = email
            };

            var createdUser = await _userManager.CreateAsync(newUser, password);


            if (!createdUser.Succeeded)
            {
                return new AuthResult
                {
                    Errors = createdUser.Errors.Select(x => x.Description)
                };
            }
            return GenerateAuthResult(newUser); ;
        }



        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new AuthResult
                {
                    Errors = new[] { "User does not exist" }
                };
            }

            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);

            if (userHasValidPassword == false)
            {
                return new AuthResult
                {
                    Errors = new[] { "Login/Password combination does not match" }
                };
            }

            return GenerateAuthResult(user);
        }



        private AuthResult GenerateAuthResult(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Aud, _jwtSettings.Audience),
                        new Claim(JwtRegisteredClaimNames.Iss, _jwtSettings.Issuer),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email),
                        new Claim("id", user.Id)
                    }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthResult
            {
                IsSuccessful = true,
                Token = tokenHandler.WriteToken(token)
            };
        }


    }
}
