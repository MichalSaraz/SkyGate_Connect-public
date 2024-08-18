using AutoMapper;
using Core.Dtos;
using Core.FlightContext;
using Core.HistoryTracking;
using Core.HistoryTracking.Enums;
using Core.Interfaces;
using Core.PassengerContext.JoinClasses;
using Microsoft.AspNetCore.Mvc;
using Web.Api.PassengerManagement.Models;
using Web.Errors;

namespace Web.Api.PassengerManagement.Controllers
{
    [ApiController]
    [Route("passenger-management/special-service-request")]
    public class SpecialServiceRequestController : ControllerBase
    {
        private readonly IPassengerRepository _passengerRepository;
        private readonly ISSRCodeRepository _sSRCodeRepository;
        private readonly ISpecialServiceRequestRepository _specialServiceRequestRepository;
        private readonly IActionHistoryRepository _actionHistoryRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly IMapper _mapper;

        public SpecialServiceRequestController(
            IPassengerRepository passengerRepository,
            ISSRCodeRepository sSRCodeRepository,
            ISpecialServiceRequestRepository specialServiceRequestRepository,
            IActionHistoryRepository actionHistoryRepository,
            IFlightRepository flightRepository,
            IMapper mapper)
        {
            _passengerRepository = passengerRepository;
            _sSRCodeRepository = sSRCodeRepository;
            _specialServiceRequestRepository = specialServiceRequestRepository;
            _actionHistoryRepository = actionHistoryRepository;
            _flightRepository = flightRepository;
            _mapper = mapper;
        }

        // Note: The full implementation of this controller is located in a private repository.        
        // For an example of how a similar controller is implemented, please refer to the BaggageController.
    }
}