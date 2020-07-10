using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tracking.Common.ViewModels;
using Tracking.Services.Interfaces;

namespace Tracking.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ILogger<EmployeesController> _logger;
        private readonly IEmployeeService _employeeService;

        public EmployeesController(ILogger<EmployeesController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var data = await _employeeService.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var data = await _employeeService.GetAsync(id);
            return Ok(data);
        }

        [HttpPost]
        public async Task Post([FromBody] EmployeeViewModel employee) => await _employeeService.AddUpdateAsync(null, employee);

        [HttpPut("{id}")]
        public async Task Post(int id, [FromBody] EmployeeViewModel employee) => await _employeeService.AddUpdateAsync(id, employee);

        [HttpDelete("{id}")]
        public async Task DeleteAsync(int id) => await _employeeService.DeleteAsync(id);
    }
}