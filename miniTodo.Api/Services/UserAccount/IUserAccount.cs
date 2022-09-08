namespace miniTodo.Api.Services.UserAccount;

using miniTodo.Api.Data.Entities;
using miniTodo.Api.Services.UserAccount.Models;
using System.Threading.Tasks;

public interface IUserAccount
{
    Task<User> Login(LoginUserModel model);
    Task<User> Register(RegisterUserModel model);
}
