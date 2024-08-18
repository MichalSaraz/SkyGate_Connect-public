using AutoMapper;
using Core.Dtos;
using Core.FlightContext;
using Core.HistoryTracking;
using Core.HistoryTracking.Enums;
using Core.Interfaces;
using Core.PassengerContext;
using Core.SeatingContext;

namespace Infrastructure.Services;

public class ActionHistoryService : IActionHistoryService
{
    private readonly IBasePassengerOrItemRepository _basePassengerOrItemRepository;
    private readonly IPassengerOrItemDtoMappingService _passengerOrItemDtoMappingService;
    private readonly IActionHistoryRepository _actionHistoryRepository;
    private readonly IMapper _mapper;

    public ActionHistoryService(
        IBasePassengerOrItemRepository basePassengerOrItemRepository,
        IPassengerOrItemDtoMappingService passengerOrItemDtoMappingService,
        IActionHistoryRepository actionHistoryRepository,
        IMapper mapper)
    {
        _basePassengerOrItemRepository = basePassengerOrItemRepository;
        _passengerOrItemDtoMappingService = passengerOrItemDtoMappingService;
        _actionHistoryRepository = actionHistoryRepository;
        _mapper = mapper;
    }
    
    public async Task<List<BasePassengerOrItemDto>> SavePassengerOrItemActionsToPassengerHistoryAsync(
        IReadOnlyList<BasePassengerOrItem> oldValuesForActionHistory, 
        IReadOnlyList<BasePassengerOrItem> passengerOrItems, BaseFlight selectedFlight, bool displayDetails = true)
    {
        oldValuesForActionHistory = oldValuesForActionHistory.OrderBy(x => x.Id).ToList();
        passengerOrItems = passengerOrItems.OrderBy(x => x.Id).ToList();

        var oldValuesForActionHistoryDto = 
            _passengerOrItemDtoMappingService.MapPassengerOrItemsToDto(oldValuesForActionHistory, selectedFlight,
                displayDetails);
        var passengerOrItemsDto = 
            _passengerOrItemDtoMappingService.MapPassengerOrItemsToDto(passengerOrItems, selectedFlight,
                displayDetails);

        for (int i = 0; i < passengerOrItems.Count; i++)
        {
            var record = new ActionHistory(ActionTypeEnum.Updated, passengerOrItems[i].Id, nameof(Passenger),
                passengerOrItemsDto[i], oldValuesForActionHistoryDto[i]);

            await _actionHistoryRepository.AddAsync(record);
        }

        return passengerOrItemsDto;
    }

    /// <summary>
    /// Saves seat actions to passenger history.
    /// </summary>
    /// <param name="flightId">The ID of the flight.</param>
    /// <param name="oldValuesForActionHistory">The list of old values for action history.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation. The task result
    /// contains a list of <see cref="SeatDto"/>.</returns>
    public async Task<List<SeatDto>> SaveSeatActionsToPassengerHistoryAsync(Guid flightId,
        IReadOnlyList<BasePassengerOrItem> oldValuesForActionHistory)
    {
        var passengerOrItems = await _basePassengerOrItemRepository.GetBasePassengerOrItemsByCriteriaAsync(p => 
            oldValuesForActionHistory.Select(x => x.Id).Contains(p.Id), false);
        
        oldValuesForActionHistory = oldValuesForActionHistory.OrderBy(x => x.Id).ToList();
        passengerOrItems = passengerOrItems.OrderBy(x => x.Id).ToList();

        var oldValuesForActionHistoryDto = oldValuesForActionHistory.Select(p =>
                _mapper.Map<SeatDto>(p.AssignedSeats.FirstOrDefault(s => s.FlightId == flightId)))
            .ToList();
        var seatDto = passengerOrItems.Select(p =>
                _mapper.Map<SeatDto>(p.AssignedSeats.FirstOrDefault(s => s.FlightId == flightId)))
            .ToList();
        
        var tasks = new List<Task>();
        
        for (int i = 0; i < passengerOrItems.Count; i++)
        {
            var record = new ActionHistory(ActionTypeEnum.Updated, passengerOrItems[i].Id, nameof(Seat),
                seatDto[i], oldValuesForActionHistoryDto[i]);

            tasks.Add(_actionHistoryRepository.AddAsync(record));
        }

        await Task.WhenAll(tasks);

        return seatDto;
    }
}