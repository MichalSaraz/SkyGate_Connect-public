using Core.PassengerContext.APIS;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Core.Interfaces;

namespace Infrastructure.Repositories
{
    public class APISDataRepository : GenericRepository<APISData>, IAPISDataRepository
    {
        public APISDataRepository(AppDbContext context) : base(context)
        {
        }

        // Note: The full implementation of this class is located in a private repository.        
        // For an example of how a similar class is implemented, please refer to other repository classes.
    }
}