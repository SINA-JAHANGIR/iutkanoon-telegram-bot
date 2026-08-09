using IutKanoon.TelegramBot.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IutKanoon.TelegramBot.Infrastructure.Data.Repositories;


// Implements data access operations for Telegram user state entities using Entity Framework Core
public class UserStateRepository : IUserStateRepository
{
    private readonly BotDbContext _dbContext;

    public UserStateRepository(BotDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<UserState?> GetByTelegramIdAsync(long telegramId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserStates
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.TelegramId == telegramId, cancellationToken);
    }

    public async Task AddOrUpdateAsync(UserState state, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(state);

        var existingState = await _dbContext.UserStates
            .FirstOrDefaultAsync(s => s.TelegramId == state.TelegramId, cancellationToken);

        if (existingState is null)
        {
            await _dbContext.UserStates.AddAsync(state, cancellationToken);
        }
        else
        {
            _dbContext.Entry(existingState).CurrentValues.SetValues(state);
            existingState.UpdatedAt = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(long telegramId, CancellationToken cancellationToken = default)
    {
        var state = await _dbContext.UserStates
            .FirstOrDefaultAsync(s => s.TelegramId == telegramId, cancellationToken);

        if (state is not null)
        {
            _dbContext.UserStates.Remove(state);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}