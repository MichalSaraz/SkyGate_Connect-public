using Core.FlightContext;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IBaseFlightRepository : IGenericRepository<BaseFlight>
    {
        /// <summary>
        /// Retrieves a base flight with the specified id asynchronously.
        /// </summary>
        /// <param name="id">The id of the flight.</param>
        /// <param name="tracked">Indicates whether the flight should be tracked by the context. Default is
        /// false.</param>
        /// <returns>The base flight with the specified id.</returns>
        Task<BaseFlight> GetFlightByIdAsync(Guid id, bool tracked = true);

        // Note: The full implementation of this interface is located in a private repository.
    }
}
