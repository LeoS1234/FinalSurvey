using FinalSurvey.DTOs.AuthUser;
using FinalSurvey.Models;
using System.Threading.Tasks;

namespace FinalSurvey.Services.AuthService
{
    public interface IAuthService
    {
        //Task<ServiceResponse<string>> Register(User user, string password);
        //Task<ServiceResponse<string>> Login(string username, string password);
        //Task<ServiceResponse<GetUserDto>> UpdateUser(User user, string password, Guid id);
        //Task<bool> UserIdExist(Guid id);
        //Task<bool> UserExist(string username);

        Task<ServiceResponse<string>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string username, string password);
        Task<ServiceResponse<GetUserDto>> UpdateUser(User user, string password, Guid id);
        Task<bool> UserExist(string username);
        Task<bool> UserIdExist(Guid id);

    }
}
