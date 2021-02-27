using Core.Auth.Models;
using System.Threading.Tasks;

namespace Core.Interfaces
{
public interface IIdentityService
{
    Task<AuthResult> RegisterAsync(string email, string password);
}
}
