using Core.FlightContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IFlightRepository : IBaseFlightRepository
    {
        Task<bool> ExistsAsync(Guid flightId);
        /// <summary>
        /// Retrieves a list of flights based on the given criteria.
        /// </summary>
        /// <param name="criteria">The criteria expression used to filter the flights.</param>
        /// <param name="tracked">Optional. Indicates whether the returned flights should be tracked by the context.
        /// Default is false.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the list of flights that
        /// match the criteria.</returns>
        Task<IReadOnlyList<Flight>> GetFlightsByCriteriaAsync(
            Expression<Func<Flight, bool>> criteria, bool tracked = false);

        // Note: The full implementation of this interface is located in a private repository.
    }
}