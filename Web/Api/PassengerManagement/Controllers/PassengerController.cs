using System.Linq.Expressions;
using AutoMapper;
using Core.Dtos;
using Core.FlightContext;
using Core.FlightContext.FlightInfo.Enums;
using Core.HistoryTracking;
using Core.HistoryTracking.Enums;
using Core.Interfaces;
using Core.PassengerContext;
using Core.PassengerContext.Booking;
using Core.PassengerContext.Booking.Enums;
using Core.PassengerContext.JoinClasses;
using Core.SeatingContext;
using Core.SeatingContext.Enums;
using Microsoft.AspNetCore.Mvc;
using Web.Api.PassengerManagement.Models;
using Web.Errors;

namespace Web.Api.PassengerManagement.Controllers
{
    [ApiController]
    [Route("passenger-management")]
    public class PassengerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITimeProvider _timeProvider;
        private readonly IFlightRepository _flightRepository;
        private readonly IPassengerRepository _passengerRepository;
        private readonly IInfantRepository _infantRepository;
        private readonly IPassengerBookingDetailsRepository _passengerBookingDetailsRepository;
        private readonly IPassengerFlightRepository _passengerFlightRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IBasePassengerOrItemRepository _basePassengerOrItemRepository;
        private readonly IActionHistoryRepository _actionHistoryRepository;
        private readonly IActionHistoryService _actionHistoryService;
        private readonly IPassengerOrItemDtoMappingService _passengerOrItemDtoMappingService;

        public PassengerController(
            IMapper mapper,
            ITimeProvider timeProvider,
            IFlightRepository flightRepository,
            IPassengerRepository passengerRepository,
            IInfantRepository infantRepository,
            IPassengerBookingDetailsRepository passengerBookingDetailsRepository,
            IPassengerFlightRepository passengerFlightRepository,
            ISeatRepository seatRepository,
            IBasePassengerOrItemRepository basePassengerOrItemRepository, 
            IActionHistoryRepository actionHistoryRepository, 
            IActionHistoryService actionHistoryService,
            IPassengerOrItemDtoMappingService passengerOrItemDtoMappingService)
        {
            _mapper = mapper;
            _timeProvider = timeProvider;
            _flightRepository = flightRepository;
            _passengerRepository = passengerRepository;
            _infantRepository = infantRepository;
            _passengerBookingDetailsRepository = passengerBookingDetailsRepository;
            _passengerFlightRepository = passengerFlightRepository;
            _seatRepository = seatRepository;
            _basePassengerOrItemRepository = basePassengerOrItemRepository;
            _actionHistoryRepository = actionHistoryRepository;
            _actionHistoryService = actionHistoryService;
            _passengerOrItemDtoMappingService = passengerOrItemDtoMappingService;
        }

        // Note: The full implementation of this controller is located in a private repository.        
        // For an example of how a similar controller is implemented, please refer to the BaggageController.
    }
}