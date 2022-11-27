using FinalSurvey.DTOs.AuthUser;
using FinalSurvey.Models;
using System.Threading.Tasks;

namespace FinalSurvey.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string username, string password);
        Task<ServiceResponse<GetUserDto>> UpdateUser(User user, string password, int id);
        Task<bool> UserIdExist(int id);
        Task<bool> UserExist(string username);

    }
}
