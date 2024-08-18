using Core.PassengerContext.JoinClasses;
using Infrastructure.Data;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class SpecialServiceRequestRepository : GenericRepository<SpecialServiceRequest>,
        ISpecialServiceRequestRepository
    {
        public SpecialServiceRequestRepository(AppDbContext context) : base(context)
        {
        }

        // Note: The full implementation of this class is located in a private repository.        
        // For an example of how a similar class is implemented, please refer to other repository classes.
    }
}