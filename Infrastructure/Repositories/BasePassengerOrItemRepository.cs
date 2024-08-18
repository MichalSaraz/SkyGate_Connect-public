﻿using Core.Interfaces;
using Core.PassengerContext;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class BasePassengerOrItemRepository : GenericRepository<BasePassengerOrItem>, IBasePassengerOrItemRepository
    {
        public BasePassengerOrItemRepository(AppDbContext context) : base(context)
        {            
        }

        public virtual async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Passengers.AnyAsync(f => f.Id == id);
        }

        public async Task<IReadOnlyList<BasePassengerOrItem>> GetBasePassengerOrItemsByCriteriaAsync(
            Expression<Func<BasePassengerOrItem, bool>> criteria, bool tracked = true)
        {
            var basePassengerOrItemQuery = _context.Passengers.AsQueryable()
                .Include(_ => _.Flights)
                    .ThenInclude(_ => _.Flight)
                .Include(_ => _.AssignedSeats)
                    .ThenInclude(_ => _.Flight)
                .Include(_ => _.BookingDetails)
                .Include(_ => _.Comments)
                    .ThenInclude(_ => _.LinkedToFlights)
                        .ThenInclude(_ => _.Flight)
                .Where(criteria);

            if (!tracked)
            {
                basePassengerOrItemQuery = basePassengerOrItemQuery.AsNoTracking();
            }

            var basePassengerOrItems = await basePassengerOrItemQuery.ToListAsync();
            return basePassengerOrItems;
        }
        
        public async Task<BasePassengerOrItem> GetBasePassengerOrItemByIdAsync(Guid id)
        {
            var basePassengerOrItem = await _context.Passengers.AsNoTracking()
                .Include(_ => _.Flights)
                    .ThenInclude(_ => _.Flight)
                .Include(_ => _.AssignedSeats)
                    .ThenInclude(_ => _.Flight)
                .Include(_ => _.Comments)
                    .ThenInclude(_ => _.LinkedToFlights)
                        .ThenInclude(_ => _.Flight)
                .FirstOrDefaultAsync(_ => _.Id == id);

            return basePassengerOrItem;
        }

        // Note: The full implementation of this class is located in a private repository.
    }
}
