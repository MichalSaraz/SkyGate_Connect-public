using Core.FlightContext;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class OtherFlightRepository : BaseFlightRepository, IOtherFlightRepository
    {
        public OtherFlightRepository(AppDbContext context) : base(context)
        {
        }

        // Note: The full implementation of this class is located in a private repository.        
        // For an example of how a similar class is implemented, please refer to other repository classes.
    }
}
