using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PassengerFlightRepository : IPassengerFlightRepository
    {
        private readonly AppDbContext _context;

        public PassengerFlightRepository(AppDbContext context)
        {
            _context = context;
        }

        // Note: The full implementation of this class is located in a private repository.        
        // For an example of how a similar class is implemented, please refer to other repository classes.
    }
}
