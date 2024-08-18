using Core.Interfaces;
using Core.PassengerContext.Booking;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SSRCodeRepository : GenericRepository<SSRCode>, ISSRCodeRepository
    {
        public SSRCodeRepository(AppDbContext context) : base(context)
        {
        }

        // Note: The full implementation of this class is located in a private repository.        
        // For an example of how a similar class is implemented, please refer to other repository classes.
    }
}
