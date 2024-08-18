using AutoMapper;
using Core.Dtos;
using Core.FlightContext;
using Core.Interfaces;
using Core.PassengerContext;

namespace Infrastructure.Services;

public class PassengerOrItemDtoMappingService : IPassengerOrItemDtoMappingService
{
    private readonly IMapper _mapper;

    public PassengerOrItemDtoMappingService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public BasePassengerOrItemDto MapSinglePassengerOrItemToDto(BasePassengerOrItem item, BaseFlight flight,
        bool displayDetails = true)
    {
        BasePassengerOrItemDto dto = item switch
        {
            Passenger passenger => (displayDetails)
                ? _mapper.Map<PassengerDetailsDto>(passenger, opt =>
                {
                    opt.Items["DepartureDateTime"] = flight.DepartureDateTime;
                    opt.Items["FlightId"] = flight.Id;
                })
                : _mapper.Map<PassengerOrItemOverviewDto>(passenger, opt =>
                {
                    opt.Items["DepartureDateTime"] = flight.DepartureDateTime;
                    opt.Items["FlightId"] = flight.Id;
                }),
            Infant infant => (displayDetails)
                ? _mapper.Map<InfantDetailsDto>(infant, opt =>
                {
                    opt.Items["DepartureDateTime"] = flight.DepartureDateTime;
                    opt.Items["FlightId"] = flight.Id;
                })
                : _mapper.Map<InfantOverviewDto>(infant, opt =>
                {
                    opt.Items["DepartureDateTime"] = flight.DepartureDateTime;
                    opt.Items["FlightId"] = flight.Id;
                }),
            CabinBaggageRequiringSeat or ExtraSeat when displayDetails => 
                _mapper.Map<ItemDetailsDto>(item, opt =>
                {
                    opt.Items["DepartureDateTime"] = flight.DepartureDateTime;
                    opt.Items["FlightId"] = flight.Id;
                }),
            CabinBaggageRequiringSeat or ExtraSeat => 
                _mapper.Map<PassengerOrItemOverviewDto>(item, opt =>
                {
                    opt.Items["DepartureDateTime"] = flight.DepartureDateTime;
                    opt.Items["FlightId"] = flight.Id;
                }),              
            _ => null
        };
        
        return dto;
    }

    public List<BasePassengerOrItemDto> MapPassengerOrItemsToDto(IEnumerable<BasePassengerOrItem> passengerOrItems,
        BaseFlight flight, bool displayDetails = true)
    {
        return passengerOrItems
            .Select(passengerOrItem => MapSinglePassengerOrItemToDto(passengerOrItem, flight, displayDetails))
            .ToList();
    }
}