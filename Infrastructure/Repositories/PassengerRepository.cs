﻿using Core.Interfaces;
using Core.PassengerContext;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class PassengerRepository : BasePassengerOrItemRepository, IPassengerRepository
    {
        private readonly IMemoryCache _cache;

        public PassengerRepository(AppDbContext context, IMemoryCache cache) : base(context)
        {
            _cache = cache;
        }

        public override async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Set<Passenger>().AnyAsync(f => f.Id == id);
        }
                
        public async Task<Passenger> GetPassengerDetailsByIdAsync(Guid id, bool tracked = true)
        {
            var CACHE_KEY = $"Passenger_{id}_{tracked}";
            
            if (!tracked && _cache.TryGetValue(CACHE_KEY, out Passenger cachedPassenger))
            {
                return cachedPassenger;
            }

            var passengerQuery = _context.Set<Passenger>().AsQueryable()
                .Include(_ => _.BookingDetails)
                    .ThenInclude(_ => _.PNR)
                .Include(_ => _.Flights)
                    .ThenInclude(_ => _.Flight)
                .Include(_ => _.AssignedSeats)
                    .ThenInclude(_ => _.Flight)
                .Include(_ => _.Infant.BookingDetails)
                .Include(_ => _.Infant)
                    .ThenInclude(_ => _.Flights)
                        .ThenInclude(_ => _.Flight)
                .Include(_ => _.PassengerCheckedBags)
                    .ThenInclude(_ => _.BaggageTag)
                .Include(_ => _.PassengerCheckedBags)
                    .ThenInclude(_ => _.SpecialBag)
                .Include(_ => _.PassengerCheckedBags)
                    .ThenInclude(_ => _.FinalDestination)
                .Include(_ => _.PassengerCheckedBags)
                    .ThenInclude(_ => _.Flights)
                        .ThenInclude(_ => _.Flight)
                .Include(_ => _.SpecialServiceRequests)
                    .ThenInclude(_ => _.SSRCode)
                .Include(_ => _.SpecialServiceRequests)
                    .ThenInclude(_ => _.Flight)
                .Include(_ => _.TravelDocuments)
                .Include(_ => _.Comments)
                    .ThenInclude(_ => _.LinkedToFlights)
                        .ThenInclude(_ => _.Flight)
                .Where(_ => _.Id == id);

            if (!tracked)
            {
                passengerQuery = passengerQuery.AsNoTracking();
            }

            var passenger = await passengerQuery.SingleOrDefaultAsync();

            _cache.Set(CACHE_KEY, passenger,
                new MemoryCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(5) });

            return passenger;
        }

        // Note: The full implementation of this class is located in a private repository.
    }
}