using System.Collections.Generic;
using System.Threading.Tasks;
using Tracking.Common.Models;
using Tracking.Common.ViewModels;

namespace Tracking.Services.Interfaces
{
    public interface IUserService
    {
        Task Seed();
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string appSecret);
        Task<IEnumerable<UserViewModel>> GetAllAsync();
    }
}