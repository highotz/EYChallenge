using EYChallenge.Utilities.SystemObjects.Enum;

namespace EYChallenge.Utilities.SystemObjects.Interfaces
{
    public interface IAuditingServiceFactory
    {
        IAuditingService Create(ServiceStrategy strategy);
    }
}
