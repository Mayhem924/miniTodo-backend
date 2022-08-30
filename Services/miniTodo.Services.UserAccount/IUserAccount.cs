namespace miniTodo.Services.UserAccount;

using miniTodo.Data.Entities;
using miniTodo.Services.UserAccount.Models;
using System.Threading.Tasks;

public interface IUserAccount
{
    Task<User> Login(LoginUserModel model);
    Task<User> Register(RegisterUserModel model);
    Task<string> RefreshToken(RefreshTokenModel model);
}
