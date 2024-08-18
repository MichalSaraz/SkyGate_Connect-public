using System.Linq.Expressions;
using AutoMapper;
using Core.Dtos;
using Core.FlightContext;
using Core.FlightContext.FlightInfo.Enums;
using Core.HistoryTracking;
using Core.HistoryTracking.Enums;
using Core.Interfaces;
using Core.PassengerContext;
using Core.PassengerContext.Booking.Enums;
using Core.PassengerContext.JoinClasses;
using Core.SeatingContext.Enums;
using Microsoft.AspNetCore.Mvc;
using Web.Api.FlightManagement.Models;
using Web.Errors;
using Web.Extensions;

namespace Web.Api.FlightManagement.Controllers
{
    [ApiController]
    [Route("flight-management")]
    public class FlightController : ControllerBase
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IBaseFlightRepository _baseFlightRepository;
        private readonly IOtherFlightRepository _otherFlightRepository;
        private readonly IPassengerRepository _passengerRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ISpecialServiceRequestRepository _specialServiceRequestRepository;
        private readonly IBasePassengerOrItemRepository _basePassengerOrItemRepository;
        private readonly IActionHistoryRepository _actionHistoryRepository;
        private readonly IInfantRepository _infantRepository;
        private readonly ITimeProvider _timeProvider;
        private readonly IMapper _mapper;

        public FlightController(
            IFlightRepository flightRepository,
            IBaseFlightRepository baseFlightRepository,
            IOtherFlightRepository otherFlightRepository,
            IPassengerRepository passengerRepository,
            ICommentRepository commentRepository,
            ISpecialServiceRequestRepository specialServiceRequestRepository,
            IBasePassengerOrItemRepository basePassengerOrItemRepository,
            IActionHistoryRepository actionHistoryRepository,
            IInfantRepository infantRepository,
            ITimeProvider timeProvider,
            IMapper mapper)
        {
            _flightRepository = flightRepository;
            _baseFlightRepository = baseFlightRepository;
            _otherFlightRepository = otherFlightRepository;
            _passengerRepository = passengerRepository;
            _commentRepository = commentRepository;
            _specialServiceRequestRepository = specialServiceRequestRepository;
            _basePassengerOrItemRepository = basePassengerOrItemRepository;
            _actionHistoryRepository = actionHistoryRepository;
            _infantRepository = infantRepository;
            _timeProvider = timeProvider;
            _mapper = mapper;
        }

        // Note: The full implementation of this controller is located in a private repository.        
        // For an example of how a similar controller is implemented, please refer to the BaggageController.
    }
}
