namespace Core.Dtos;

public class PassengerFlightConnectionsDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Gender { get; set; }
    public int NumberOfCheckedBags { get; set; }
    public string FlightClass { get; set; }
    public string SeatNumber { get; set; }
    public List<PassengerFlightDto> FlightConnections { get; set; }
}