using Core.HistoryTracking;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ActionHistoryRepository : GenericRepository<ActionHistory>, IActionHistoryRepository
{
    public  ActionHistoryRepository(AppDbContext context) : base(context)
    {
    }

    // Note: The full implementation of this class is located in a private repository.        
    // For an example of how a similar class is implemented, please refer to other repository classes.
}