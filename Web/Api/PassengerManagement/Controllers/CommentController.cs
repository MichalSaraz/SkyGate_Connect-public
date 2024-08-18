using AutoMapper;
using Core.Dtos;
using Core.HistoryTracking;
using Core.HistoryTracking.Enums;
using Core.Interfaces;
using Core.PassengerContext.Booking;
using Core.PassengerContext.Booking.Enums;
using Microsoft.AspNetCore.Mvc;
using Web.Api.PassengerManagement.Models;
using Web.Errors;

namespace Web.Api.PassengerManagement.Controllers
{
    [ApiController]
    [Route("passenger-management/comment")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ICommentService _commentService;
        private readonly IActionHistoryRepository _actionHistoryRepository;
        private readonly IMapper _mapper;

        public CommentController(
            ICommentRepository commentRepository, 
            ICommentService commentService, 
            IActionHistoryRepository actionHistoryRepository,
            IMapper mapper)
        {
            _commentRepository = commentRepository;
            _commentService = commentService;
            _actionHistoryRepository = actionHistoryRepository;
            _mapper = mapper;
        }

        // Note: The full implementation of this controller is located in a private repository.        
        // For an example of how a similar controller is implemented, please refer to the BaggageController.
    }
}