﻿using Core.SeatingContext;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface ISeatRepository : IGenericRepository<Seat>
    {
        // Note: The full implementation of this interface is located in a private repository.        
        // For an example of how a similar class is implemented, please refer to other interfaces.
    }
}
