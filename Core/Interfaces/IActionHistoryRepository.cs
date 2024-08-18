using Core.HistoryTracking;

namespace Core.Interfaces;

public interface IActionHistoryRepository : IGenericRepository<ActionHistory>
{
    // Note: The full implementation of this interface is located in a private repository.        
    // For an example of how a similar class is implemented, please refer to other interfaces.
}