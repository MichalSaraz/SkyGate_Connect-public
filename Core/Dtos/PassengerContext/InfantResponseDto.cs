namespace Core.Dtos;

public class InfantResponseDto
{
    public InfantOverviewDto Infant { get; set; } 
    public List<SpecialServiceRequestDto> SpecialServiceRequests { get; set; }
}