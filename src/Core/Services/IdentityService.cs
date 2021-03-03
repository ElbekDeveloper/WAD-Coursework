using Core.Auth.Models;
using Core.Interfaces;
using Core.Repositories;
using System.Threading.Tasks;

namespace Core.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IUserRepository _repository;


        public IdentityService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<AuthResult> RegisterAsync(string email, string password)
        {
            return await _repository.CreateAsync(email, password);
        }

        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            return await _repository.LoginAsync(email, password);
        }
        public Task<bool> CheckUserOwns(int articleId, string userId)
        {
            return _repository.CheckUserOwnsArticle(articleId, userId);
        }

        public Task<AuthResult> RefreshTokenAsync(string token, string refreshToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
