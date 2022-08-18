namespace miniTodo.Services.UserAccount;

using miniTodo.Services.UserAccount.Models;
using System.Threading.Tasks;

public interface IUserAccount
{
    Task<string> Login(LoginUserModel model);
    Task<string> Register(RegisterUserModel model);
}
