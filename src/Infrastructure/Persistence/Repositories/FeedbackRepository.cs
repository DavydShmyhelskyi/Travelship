using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Feedbacks;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class FeedbackRepository(ApplicationDbContext context)
    : IFeedbackRepository, IFeedbackQueries
{
    public async Task<Feedback> AddAsync(Feedback entity, CancellationToken cancellationToken)
    {
        await context.Feedbacks.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<Feedback> UpdateAsync(Feedback entity, CancellationToken cancellationToken)
    {
        context.Feedbacks.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<Feedback> DeleteAsync(Feedback entity, CancellationToken cancellationToken)
    {
        context.Feedbacks.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<Option<Feedback>> GetByRatingAsync(int rating, CancellationToken cancellationToken)
    {
        var feedback = await context.Feedbacks
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Rating == rating, cancellationToken);

        return feedback is null ? Option<Feedback>.None : feedback;
    }

    public async Task<Option<Feedback>> GetByIdAsync(FeedbackId id, CancellationToken cancellationToken)
    {
        var feedback = await context.Feedbacks
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return feedback is null ? Option<Feedback>.None : feedback;
    }

    public async Task<IReadOnlyList<Feedback>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Feedbacks
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
