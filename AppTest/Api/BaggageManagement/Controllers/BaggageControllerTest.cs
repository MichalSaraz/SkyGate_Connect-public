using System.Linq.Expressions;
using AutoMapper;
using Core.BaggageContext;
using Core.BaggageContext.Enums;
using Core.Dtos;
using Core.FlightContext;
using Core.FlightContext.FlightInfo;
using Core.FlightContext.FlightInfo.Enums;
using Core.HistoryTracking;
using Core.Interfaces;
using Core.PassengerContext;
using Core.PassengerContext.Booking.Enums;
using Core.PassengerContext.JoinClasses;
using Core.SeatingContext.Enums;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Web.Api.BaggageManagement.Controllers;
using Web.Api.BaggageManagement.Models;
using Web.Errors;

namespace TestProject.Api.BaggageManagement.Controllers;

[TestSubject(typeof(BaggageController))]
public class BaggageControllerTest
{
    private readonly BaggageController _baggageController;
    private readonly Mock<IPassengerRepository> _passengerRepository;
    private readonly Mock<IFlightRepository> _flightRepository;
    private readonly Mock<IDestinationRepository> _destinationRepository;
    private readonly Mock<IBaggageRepository> _baggageRepository;
    private readonly Mock<IActionHistoryRepository> _actionHistoryRepository;

    public BaggageControllerTest()
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Baggage, BaggageOverviewDto>();
        });

        var mapper = mapperConfig.CreateMapper();
        _passengerRepository = new Mock<IPassengerRepository>();
        _flightRepository = new Mock<IFlightRepository>();
        _destinationRepository = new Mock<IDestinationRepository>();
        _baggageRepository = new Mock<IBaggageRepository>();
        _actionHistoryRepository = new Mock<IActionHistoryRepository>();

        _baggageController = new BaggageController(
            mapper,
            _baggageRepository.Object,
            _passengerRepository.Object,
            _flightRepository.Object,
            _destinationRepository.Object,
            _actionHistoryRepository.Object);
    }

    [Fact]
    private async Task AddBaggage_ReturnsBadRequest_WhenNoBaggageDataProvided()
    {
        // Arrange
        var passengerId = Guid.NewGuid();
        var flightId = Guid.NewGuid();
        var addBaggageModels = new List<AddBaggageModel>();

        // Act
        var result = await _baggageController.AddBaggage(passengerId, flightId, addBaggageModels);

        // Assert
        var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequestResult.Value.Should().BeOfType<ApiResponse>().Subject;
        response.StatusCode.Should().Be(400);
    }

    [Fact]
    private async Task AddBaggage_ReturnsBadRequest_WhenManualTagTypeWithoutTagNumber()
    {
        // Arrange
        var passengerId = Guid.NewGuid();
        var flightId = Guid.NewGuid();
        
        var addBaggageModels = 
            new List<AddBaggageModel> { new() { TagType = TagTypeEnum.Manual, FinalDestination = "KRS", TagNumber = "" } };

        // Act
        var result = await _baggageController.AddBaggage(passengerId, flightId, addBaggageModels);

        // Assert
        var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequestResult.Value.Should().BeOfType<ApiResponse>().Subject;
        response.StatusCode.Should().Be(400);
    }

    [Fact]
    private async Task AddBaggage_ReturnsNotFound_WhenPassengerNotFound()
    {
        // Arrange
        var passengerId = Guid.NewGuid();
        var flightId = Guid.NewGuid();
        
        var addBaggageModels = 
            new List<AddBaggageModel> { new() { TagType = TagTypeEnum.System, FinalDestination = "KRS" } };

        _passengerRepository.Setup(repo => repo.GetPassengerDetailsByIdAsync(passengerId, false))
            .ReturnsAsync(null as Passenger);

        // Act
        var result = await _baggageController.AddBaggage(passengerId, flightId, addBaggageModels);

        // Assert
        var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
        var response = notFoundResult.Value.Should().BeOfType<ApiResponse>().Subject;
        response.StatusCode.Should().Be(404);
    }

    [Fact]
    private async Task AddBaggage_ReturnsNotFound_WhenFlightNotFound()
    {
        // Arrange
        var passengerId = Guid.NewGuid();
        var flightId = Guid.NewGuid();
        var passenger = new Passenger(1, false, "John", "Doe", PaxGenderEnum.M, null, 88);
        
        var addBaggageModels =
            new List<AddBaggageModel> { new() { TagType = TagTypeEnum.System, FinalDestination = "KRS" } };

        _passengerRepository.Setup(repo => repo.GetPassengerDetailsByIdAsync(passengerId, false))
            .ReturnsAsync(passenger);
        _flightRepository.Setup(repo => repo.GetFlightByIdAsync(flightId, true)).ReturnsAsync(null as Flight);

        // Act
        var result = await _baggageController.AddBaggage(passengerId, flightId, addBaggageModels);

        // Assert
        var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
        var responseObject = notFoundResult.Value.Should().BeOfType<ApiResponse>().Subject;
        responseObject.StatusCode.Should().Be(404);
    }

    [Fact]
    private async Task AddBaggage_ReturnsBadRequest_WhenFlightIsClosed()
    {
        // Arrange
        var passengerId = Guid.NewGuid();
        var flightId = Guid.NewGuid();
        var passenger = new Passenger(1, false, "John", "Doe", PaxGenderEnum.M, null, 88);
        var flight = new Flight("DY1503", new DateTime(2023, 1, 1, 12, 0, 0), new DateTime(2023, 1, 1, 14, 0, 0), "OSL",
            "KRS", "DY") { FlightStatus = FlightStatusEnum.Closed };
        
        var addBaggageModels = 
            new List<AddBaggageModel> { new() { TagType = TagTypeEnum.System, FinalDestination = "KRS" } };

        _passengerRepository.Setup(repo => repo.GetPassengerDetailsByIdAsync(passengerId, false))
            .ReturnsAsync(passenger);
        _flightRepository.Setup(repo => repo.GetFlightByIdAsync(flightId, true)).ReturnsAsync(flight);

        // Act
        var result = await _baggageController.AddBaggage(passengerId, flightId, addBaggageModels);

        // Assert
        var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseObject = badRequestResult.Value.Should().BeOfType<ApiResponse>().Subject;
        responseObject.StatusCode.Should().Be(400);
    }

    [Fact]
    private async Task AddBaggage_ReturnsBadRequest_WhenPassengerNotCheckedIn()
    {
        // Arrange
        var passengerId = Guid.NewGuid();
        var flightId = Guid.NewGuid();
        var flight = new Flight("DY1503", new DateTime(2023, 1, 1, 12, 0, 0), new DateTime(2023, 1, 1, 14, 0, 0), "OSL",
            "KRS", "DY");
        
        var addBaggageModels = 
            new List<AddBaggageModel> { new() { TagType = TagTypeEnum.System, FinalDestination = "KRS" } };
        
        var passenger = new Passenger(1, false, "John", "Doe", PaxGenderEnum.M, null, 88)
        {
            Flights = new List<PassengerFlight>
            {
                new(passengerId, flightId, FlightClassEnum.Y) { AcceptanceStatus = AcceptanceStatusEnum.NotAccepted }
            }
        };

        _passengerRepository.Setup(repo => repo.GetPassengerDetailsByIdAsync(passengerId, false))
            .ReturnsAsync(passenger);
        _flightRepository.Setup(repo => repo.GetFlightByIdAsync(flightId, true)).ReturnsAsync(flight);

        // Act
        var result = await _baggageController.AddBaggage(passengerId, flightId, addBaggageModels);

        // Assert
        var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseObject = badRequestResult.Value.Should().BeOfType<ApiResponse>().Subject;
        responseObject.StatusCode.Should().Be(400);
    }

    [Fact]
    private async Task AddBaggage_ReturnsNotFound_WhenDestinationNotFound()
    {
        // Arrange
        var passengerId = Guid.NewGuid();
        var flightId = Guid.NewGuid();
        var flight = new Flight("DY1503", new DateTime(2023, 1, 1, 12, 0, 0), new DateTime(2023, 1, 1, 14, 0, 0), "OSL",
            "KRS", "DY") { FlightStatus = FlightStatusEnum.Open };

        var addBaggageModels =
            new List<AddBaggageModel> { new() { TagType = TagTypeEnum.System, FinalDestination = "KRS" } };
        
        var passengerFlight = new PassengerFlight(passengerId, flightId, FlightClassEnum.Y)
        {
            AcceptanceStatus = AcceptanceStatusEnum.Accepted
        };
        
        var passenger = new Passenger(1, false, "John", "Doe", PaxGenderEnum.M, null, 88)
        {
            Flights = new List<PassengerFlight> { passengerFlight }
        };

        _passengerRepository.Setup(repo => repo.GetPassengerDetailsByIdAsync(passengerId, false))
            .ReturnsAsync(passenger);
        _flightRepository.Setup(repo => repo.GetFlightByIdAsync(flightId, true)).ReturnsAsync(flight);
        _destinationRepository
            .Setup(repo => repo.GetDestinationByCriteriaAsync(It.IsAny<Expression<Func<Destination, bool>>>()))
            .ReturnsAsync(null as Destination);

        // Act
        var result = await _baggageController.AddBaggage(passengerId, flightId, addBaggageModels);

        // Assert
        var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
        var response = notFoundResult.Value.Should().BeOfType<ApiResponse>().Subject;
        response.StatusCode.Should().Be(404);
    }

    [Fact]
    private async Task AddBaggage_ReturnsBadRequest_WhenBaggageHasDifferentFinalDestinations()
    {
        // Arrange
        var passengerId = Guid.NewGuid();
        var flightId = Guid.NewGuid();
        var flight = new Flight("DY1503", new DateTime(2023, 1, 1, 12, 0, 0), new DateTime(2023, 1, 1, 14, 0, 0), "OSL",
            "KRS", "DY") { FlightStatus = FlightStatusEnum.Open };
        
        var addBaggageModels = new List<AddBaggageModel>
        {
            new() { TagType = TagTypeEnum.System, FinalDestination = "OSL" },
            new() { TagType = TagTypeEnum.System, FinalDestination = "KRS" }
        };
        
        var passengerFlight = new PassengerFlight(passengerId, flightId, FlightClassEnum.Y)
        {
            AcceptanceStatus = AcceptanceStatusEnum.Accepted
        };
        
        var passenger = new Passenger(1, false, "John", "Doe", PaxGenderEnum.M, null, 88)
        {
            PassengerCheckedBags = new List<Baggage> { new(passengerId, "KRS", 12) },
            Flights = new List<PassengerFlight> { passengerFlight }
        };

        _passengerRepository.Setup(repo => repo.GetPassengerDetailsByIdAsync(passengerId, false))
            .ReturnsAsync(passenger);
        _flightRepository.Setup(repo => repo.GetFlightByIdAsync(flightId, true)).ReturnsAsync(flight);
        _destinationRepository
            .Setup(repo => repo.GetDestinationByCriteriaAsync(It.IsAny<Expression<Func<Destination, bool>>>()))
            .ReturnsAsync(new Destination());

        // Act
        var result = await _baggageController.AddBaggage(passengerId, flightId, addBaggageModels);

        // Assert
        var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var responseObject = badRequestResult.Value.Should().BeOfType<ApiResponse>().Subject;
        responseObject.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task AddBaggage_ReturnsOk_WithAddedBaggageList()
    {
        // Arrange
        var passengerId = Guid.NewGuid();
        var flightId = Guid.NewGuid();
        var destinationCode = "KRS";

        var baggage = new Baggage(passengerId, destinationCode, 20);
        var destination = new Destination { IATAAirportCode = destinationCode };
        var flight = new Flight("DY1503", new DateTime(2023, 1, 1, 12, 0, 0), new DateTime(2023, 1, 1, 14, 0, 0), "OSL",
            "KRS", "DY");

        var baggageModels = new List<AddBaggageModel>
        {
            new()
            {
                Weight = 20,
                TagType = TagTypeEnum.System,
                FinalDestination = destinationCode,
                BaggageType = BaggageTypeEnum.Local
            }
        };

        var passenger = new Passenger(1, false, "John", "Doe", PaxGenderEnum.M, null, 88)
        {
            Flights = new List<PassengerFlight>
            {
                new(passengerId, flightId, FlightClassEnum.Y) { AcceptanceStatus = AcceptanceStatusEnum.Accepted }
            }
        };

        _passengerRepository.Setup(repo => repo.GetPassengerDetailsByIdAsync(passengerId, false))
            .ReturnsAsync(passenger);
        _flightRepository.Setup(repo => repo.GetFlightByIdAsync(flightId, true)).ReturnsAsync(flight);
        _destinationRepository
            .Setup(repo => repo.GetDestinationByCriteriaAsync(It.IsAny<Expression<Func<Destination, bool>>>()))
            .ReturnsAsync(destination);
        _baggageRepository.Setup(repo => repo.AddAsync(It.IsAny<Baggage>())).Returns(Task.FromResult(baggage));

        // Act
        var result = await _baggageController.AddBaggage(passengerId, flightId, baggageModels);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Which;
        var addedBaggageListDto = okResult.Value.Should().BeAssignableTo<List<BaggageOverviewDto>>().Which;

        addedBaggageListDto.Count.Should().Be(1);

        var firstBaggage = addedBaggageListDto.First();
        firstBaggage.PassengerFirstName.Should().Be("John");
        firstBaggage.PassengerLastName.Should().Be("Doe");

        _baggageRepository.Verify(repo => repo.AddAsync(It.IsAny<Baggage>()), Times.Once);
        _actionHistoryRepository.Verify(repo => repo.AddAsync(It.IsAny<ActionHistory>()), Times.Once);
    }
}