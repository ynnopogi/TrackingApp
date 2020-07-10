using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Tracking.Services.Interfaces;

namespace Tracking.Services
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddTransient<IUserService, UserService>();
            serviceCollection.TryAddTransient<IEmployeeService, EmployeeService>();

            return serviceCollection;
        }
    }
}