using Core.Dtos;
using Core.FlightContext;
using Core.PassengerContext;

namespace Core.Interfaces;

public interface IActionHistoryService
{
    /// <summary>
    /// Saves the passenger actions to the passenger history asynchronously.
    /// </summary>
    /// <param name="oldValuesForActionHistory">The old values.</param>
    /// <param name="passengerOrItems">The passenger or items.</param>
    /// <param name="selectedFlight">The selected flight.</param>
    /// <param name="displayDetails">If set to <c>true</c>, display details.</param>
    /// <returns>Returns a <see cref="Task{TResult}"/> of type <see cref="List{BasePassengerOrItemDto}"/> representing
    /// the passenger or item actions saved to the passenger history.</returns>
    Task<List<BasePassengerOrItemDto>> SavePassengerOrItemActionsToPassengerHistoryAsync(
        IReadOnlyList<BasePassengerOrItem> oldValuesForActionHistory, 
        IReadOnlyList<BasePassengerOrItem> passengerOrItems, BaseFlight selectedFlight, bool displayDetails = true);

    /// <summary>
    /// Saves the seat actions to the passenger history asynchronously.
    /// </summary>
    /// <param name="flightId">The flight ID.</param>
    /// <param name="oldValuesForActionHistory">The list of old passenger or item values.</param>
    /// <returns>Returns a <see cref="Task{TResult}"/> of type <see cref="List{SeatDto}"/> representing the seat actions
    /// saved to the passenger history.</returns>
    Task<List<SeatDto>> SaveSeatActionsToPassengerHistoryAsync(Guid flightId,
        IReadOnlyList<BasePassengerOrItem> oldValuesForActionHistory);
}