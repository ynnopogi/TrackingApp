using System.Collections.Generic;
using System.Threading.Tasks;
using Tracking.Common.ViewModels;

namespace Tracking.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task Seed();
        Task<IEnumerable<EmployeeViewModel>> GetAllAsync();
        Task<EmployeeViewModel> GetAsync(int id);
        Task AddUpdateAsync(int? id, EmployeeViewModel model);
        Task DeleteAsync(int id);
    }
}