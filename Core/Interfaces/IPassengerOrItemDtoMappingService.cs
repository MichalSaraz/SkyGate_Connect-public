using Core.Dtos;
using Core.FlightContext;
using Core.PassengerContext;

namespace Core.Interfaces;

public interface IPassengerOrItemDtoMappingService
{
    /// <summary>
    /// Maps a single passenger or item to a DTO based on the given flight and display details flag.
    /// </summary>
    /// <param name="item">The passenger or item to map.</param>
    /// <param name="flight">The flight associated with the passenger or item.</param>
    /// <param name="displayDetails">Flag indicating whether to include detailed information in the DTO.</param>
    /// <returns>The mapped DTO object or null if the passenger or item does not match any specific type.</returns>
    BasePassengerOrItemDto MapSinglePassengerOrItemToDto(BasePassengerOrItem item, BaseFlight flight,
        bool displayDetails = true);

    /// <summary>
    /// Maps a collection of passenger or item objects to DTOs using the <see cref="BaseFlight"/> information.
    /// </summary>
    /// <param name="passengerOrItems">The collection of passenger or item objects to be mapped to DTOs.</param>
    /// <param name="flight">The <see cref="BaseFlight"/> object containing flight information.</param>
    /// <param name="displayDetails">A flag indicating whether to display additional details in the DTOs (default is
    /// true).</param>
    /// <returns>The collection of DTOs mapped from the passenger or item objects.</returns>
    List<BasePassengerOrItemDto> MapPassengerOrItemsToDto(IEnumerable<BasePassengerOrItem> passengerOrItems, 
        BaseFlight flight, bool displayDetails = true);
}