using System.Linq.Expressions;
using Core.PassengerContext.APIS;

namespace Core.Interfaces
{
    public interface IAPISDataRepository : IGenericRepository<APISData>
    {
        // Note: The full implementation of this interface is located in a private repository.        
        // For an example of how a similar class is implemented, please refer to other interfaces.
    }
}