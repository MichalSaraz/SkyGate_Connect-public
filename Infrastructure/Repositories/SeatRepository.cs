﻿using Core.Interfaces;
using Core.SeatingContext;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class SeatRepository : GenericRepository<Seat>, ISeatRepository
    {
        public SeatRepository(AppDbContext context) : base(context)
        {
        }

        // Note: The full implementation of this class is located in a private repository.        
        // For an example of how a similar class is implemented, please refer to other repository classes.
    }
}