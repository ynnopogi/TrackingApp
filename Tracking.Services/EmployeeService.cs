using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tracking.Common.ViewModels;
using Tracking.DataAccess.Patterns.Factory;
using Tracking.DataAccess.Patterns.Uow;
using Tracking.Entities.Models;
using Tracking.Services.Interfaces;

namespace Tracking.Services
{
    public class EmployeeService:IEmployeeService
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public EmployeeService(ILogger<EmployeeService> logger, IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _logger = logger;
        }

        // single seed employee
        private readonly List<Employee> _employees = new List<Employee>
        {
            new Employee { FirstName = "John", LastName = "Doe", Active = true }
        };

        public async Task Seed()
        {
            using IUnitOfWork uow = _unitOfWorkFactory.Create();
            foreach (var item in _employees)
            {
                Employee employee = await uow.GetEntityRepository<Employee>().FindByAsync(q => q.FirstName.Equals(item.FirstName) && q.LastName.Equals(item.LastName));
                if (employee == null)
                {
                    uow.GetEntityRepository<Employee>().Insert(item);
                }
            }
            await uow.CommitAsync();
        }

        public async Task<IEnumerable<EmployeeViewModel>> GetAllAsync()
        {
            try
            {
                await Seed();
                IEnumerable<Employee> employees = null;
                using IUnitOfWork uow = _unitOfWorkFactory.Create();
                employees = await uow.GetEntityRepository<Employee>().AllAsync();

                if (employees == null)
                    return null;
                EmployeeViewModel employee = new EmployeeViewModel();
                return employees.Select(e => employee.Map(e))
                    .OrderByDescending(o => o.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogTrace(e.StackTrace);
            }

            return Enumerable.Empty<EmployeeViewModel>();
        }

        public async Task<EmployeeViewModel> GetAsync(int id)
        {
            using IUnitOfWork uow = _unitOfWorkFactory.Create();
            var employee = await uow.GetEntityRepository<Employee>().FindByIdAsync(id);

            if (employee == null)
                return new EmployeeViewModel();

            return new EmployeeViewModel
            {
                Id=employee.Id,
                FirstName=employee.FirstName,
                LastName=employee.LastName,
                Active=employee.Active,
                ClockIn=employee.ClockIn,
                ClockOut=employee.ClockOut
            };
        }

        public async Task AddUpdateAsync(int? id, EmployeeViewModel model)
        {
            try
            {
                using IUnitOfWork uow = _unitOfWorkFactory.Create();
                var employee = await uow.GetEntityRepository<Employee>().FindByIdAsync(id);
                if (employee != null)
                {
                    // UPDATE
                    employee.FirstName = model.FirstName;
                    employee.LastName = model.LastName;
                    employee.Active = model.Active;
                    employee.ClockIn = model.ClockIn;
                    employee.ClockOut = model.ClockOut;

                    uow.GetEntityRepository<Employee>().Update(employee);
                }
                else
                {
                    // INSERT
                    Employee newEmployee = new Employee
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Active = true,
                        ClockIn = model.ClockIn,
                        ClockOut = model.ClockOut
                    };

                    uow.GetEntityRepository<Employee>().Insert(newEmployee);
                }
                await uow.CommitAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogTrace(e.StackTrace);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using IUnitOfWork uow = _unitOfWorkFactory.Create();
                var employee = await uow.GetEntityRepository<Employee>().FindByIdAsync(id);
                if (employee!=null)
                {
                    uow.GetEntityRepository<Employee>().Delete(employee);
                    await uow.CommitAsync();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogTrace(e.StackTrace);
            }
        }
    }
}