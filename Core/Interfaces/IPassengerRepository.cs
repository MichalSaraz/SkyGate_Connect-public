using Core.PassengerContext;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IPassengerRepository : IBasePassengerOrItemRepository
    {
        /// <summary>
        /// Retrieves the details of a passenger by their ID.
        /// </summary>
        /// <param name="id">The ID of the passenger.</param>
        /// <param name="tracked">Optional. Indicates whether to track the passenger in the context. Default is true.
        /// </param>
        /// <returns>The passenger details.</returns>
        Task<Passenger> GetPassengerDetailsByIdAsync(Guid id, bool tracked = true);

        // Note: The full implementation of this interface is located in a private repository.
    }
}