using System.Linq.Expressions;
using AutoMapper;
using Core.Dtos;
using Core.HistoryTracking;
using Core.HistoryTracking.Enums;
using Core.Interfaces;
using Core.PassengerContext.APIS;
using Microsoft.AspNetCore.Mvc;
using Web.Api.PassengerManagement.Models;
using Web.Errors;

namespace Web.Api.PassengerManagement.Controllers
{
    [ApiController]
    [Route("passenger-management/travel-document")]
    public class TravelDocumentController : ControllerBase
    {
        private readonly IAPISDataRepository _apisDataRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IActionHistoryRepository _actionHistoryRepository;
        private readonly ITimeProvider _timeProvider;
        private readonly IMapper _mapper;

        public TravelDocumentController(
            IAPISDataRepository apisDataRepository,
            ICountryRepository countryRepository,
            IActionHistoryRepository actionHistoryRepository,
            ITimeProvider timeProvider,
            IMapper mapper)
        {
            _apisDataRepository = apisDataRepository;
            _countryRepository = countryRepository;
            _actionHistoryRepository = actionHistoryRepository;
            _timeProvider = timeProvider;
            _mapper = mapper;
        }

        // Note: The full implementation of this controller is located in a private repository.        
        // For an example of how a similar controller is implemented, please refer to the BaggageController.
    }
}