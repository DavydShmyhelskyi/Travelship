using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Feedbacks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class FeedbackRepository(ApplicationDbContext context) : IFeedbackRepository, IFeedbackQueries
{
    public async Task<Feedback> AddAsync(Feedback entity, CancellationToken cancellationToken)
    {
        await context.Feedbacks.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<IReadOnlyList<Feedback>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Feedbacks
            .Include(x => x.User)
            .Include(x => x.Place)
            .ToListAsync(cancellationToken);
    }
}
