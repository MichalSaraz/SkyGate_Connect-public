using Core.Interfaces;
using Core.PassengerContext;
using Core.PassengerContext.Booking.Enums;
using Core.SeatingContext;
using Core.SeatingContext.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using AutoMapper;
using Core.Dtos;
using Core.HistoryTracking;
using Core.HistoryTracking.Enums;
using Core.PassengerContext.Booking;
using Web.Errors;

namespace Web.Api.SeatManagement.Controllers
{
    [ApiController]
    [Route("seat-management/flight/{flightId:guid}")]
    public class SeatController : ControllerBase
    {
        private readonly ISeatRepository _seatRepository;
        private readonly ICommentService _commentService;
        private readonly IBasePassengerOrItemRepository _basePassengerOrItemRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IActionHistoryService _actionHistoryService;
        private readonly IActionHistoryRepository _actionHistoryRepository;
        private readonly IMapper _mapper;

        public SeatController(
            ISeatRepository seatRepository,
            ICommentService commentService,
            IBasePassengerOrItemRepository basePassengerOrItemRepository,
            IFlightRepository flightRepository,
            ICommentRepository commentRepository,
            IActionHistoryService actionHistoryService,
            IActionHistoryRepository actionHistoryRepository,
            IMapper mapper)
        {
            _seatRepository = seatRepository;
            _commentService = commentService;
            _basePassengerOrItemRepository = basePassengerOrItemRepository;
            _flightRepository = flightRepository;
            _commentRepository = commentRepository;
            _actionHistoryService = actionHistoryService;
            _actionHistoryRepository = actionHistoryRepository;
            _mapper = mapper;
        }

        // Note: The full implementation of this controller is located in a private repository.        
        // For an example of how a similar controller is implemented, please refer to the BaggageController.
    }
}