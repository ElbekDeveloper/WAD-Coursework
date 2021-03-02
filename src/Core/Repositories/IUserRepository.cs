using Core.Auth.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IUserRepository 
    {
        Task<AuthResult> CreateAsync(string email, string password);
        Task<AuthResult> LoginAsync(string email, string password);
        Task<bool> CheckUserOwnsArticle(int articleId, string userId);
        
    }
}
