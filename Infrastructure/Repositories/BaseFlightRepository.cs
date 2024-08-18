using Core.FlightContext;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class BaseFlightRepository : GenericRepository<BaseFlight>, IBaseFlightRepository
    {
        public BaseFlightRepository(AppDbContext context) : base(context)
        {
        }

        public virtual async Task<BaseFlight> GetFlightByIdAsync(Guid id, bool tracked = true)
        {
            var flightQuery = _context.Flights.AsQueryable()
                .Include(_ => _.Airline)
                .Include(_ => _.ListOfBookedPassengers)
                .Where(_ => _.Id == id);            

            if (!tracked)
            {
                flightQuery = flightQuery.AsNoTracking();
            }

            var flight = await flightQuery.FirstOrDefaultAsync();

            return flight;
        }

        // Note: The full implementation of this class is located in a private repository.
    }
}
