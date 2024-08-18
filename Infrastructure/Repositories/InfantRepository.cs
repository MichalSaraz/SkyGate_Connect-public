using Core.Interfaces;
using Core.PassengerContext;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Core.HistoryTracking;

namespace Infrastructure.Repositories
{
    public class InfantRepository : BasePassengerOrItemRepository, IInfantRepository
    {
        public InfantRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Set<Infant>().AnyAsync(f => f.Id == id);
        }

        // Note: The full implementation of this class is located in a private repository.
    }
}
