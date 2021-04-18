using Core.Auth.Models;
using Core.Auth.Roles;
using Core.Auth.Settings;
using Core.Repositories;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
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
        private TokenValidationParameters _tokenValidationParameters;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserRepository(UserManager<IdentityUser> userManager, JwtSettings jwtSettings, ApplicationDbContext dbContext, TokenValidationParameters tokenValidationParameters, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _dbContext = dbContext;
            _tokenValidationParameters = tokenValidationParameters;
            _roleManager = roleManager;
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
            bool writerRoleExists = await _roleManager.RoleExistsAsync(Roles.CanWriteArticle);

            if (writerRoleExists == false)
            {
                var role = new IdentityRole();
                role.Name = Roles.CanWriteArticle;
                await _roleManager.CreateAsync(role);
            }
            //At this point we successfully created users and roles
            var result = await _userManager.AddToRoleAsync(newUser,Roles.CanWriteArticle);
            return await GenerateAuthResultAsync(newUser); ;
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

            return await GenerateAuthResultAsync(user);
        }

        public async Task<AuthResult> RefreshTokenAsync(string token, string refreshToken)
        {
            var validatedToken = GetClaimsPrincipal(token);
            if (validatedToken == null)
            {
                return new AuthResult { Errors = new[] { "Invalid Token" } };
            }
            var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            var expiryDateUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expiryDateUnix);

            if (expiryDateUtc>DateTime.UtcNow)
            {
                return new AuthResult { Errors = new[] { "Token hasn't expired yet" } };
            }
            var jti = validatedToken.Claims.Single(t => t.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _dbContext.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);
            if (storedRefreshToken == null)
            {
                return new AuthResult { Errors = new[] { "Token doesn't exist" } };
            }
            if (DateTime.UtcNow> storedRefreshToken.ExpiryDate)
            {
                return new AuthResult { Errors = new[] { "Token has already expired" } };
            }
            if (storedRefreshToken.IsInvalid)
            {
                return new AuthResult { Errors = new[] { "Token has already expired" } };
            }
            if (storedRefreshToken.IsUsed)
            {
                return new AuthResult { Errors = new[] { "Token has already been used" } };
            }
            if (storedRefreshToken.JwtId != jti)
            {
                return new AuthResult { Errors = new[] { "Token doesn't match this JWT" } };
            }

            storedRefreshToken.IsUsed = true;
            _dbContext.RefreshTokens.Update(storedRefreshToken);
            await _dbContext.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(t => t.Type == "id").Value);
            return await GenerateAuthResultAsync(user);
        }
        private ClaimsPrincipal GetClaimsPrincipal(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);

                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }
                return principal;
            }
            catch (Exception)
            {

                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken)
                && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        }
        private async Task<AuthResult> GenerateAuthResultAsync(IdentityUser user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, "CanWriteArticle"));
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
                    new Claim("id", user.Id), 
                    new Claim(ClaimTypes.Role, Roles.CanWriteArticle)
                }),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                Token = Guid.NewGuid().ToString(),
                UserId = user.Id,
                CreatedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
            };
            await _dbContext.RefreshTokens.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();

            return new AuthResult
            {
                IsSuccessful = true,
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token
            };
        }

    
    }
}
