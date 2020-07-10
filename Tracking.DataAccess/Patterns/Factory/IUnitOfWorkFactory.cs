using Tracking.DataAccess.Patterns.Uow;

namespace Tracking.DataAccess.Patterns.Factory
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create(bool trackChanges = true, bool enableLogging = false);
    }
}