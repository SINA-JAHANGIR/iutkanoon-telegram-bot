using IutKanoon.TelegramBot.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IutKanoon.TelegramBot.Infrastructure.Data.Repositories;

// Implements data access operations for Telegram user session entities using Entity Framework Core
public class UserSessionRepository : IUserSessionRepository
{
    private readonly BotDbContext _dbContext;

    public UserSessionRepository(BotDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<UserSession?> GetByTelegramIdAsync(long telegramId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserSessions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.TelegramId == telegramId, cancellationToken);
    }

    public async Task AddOrUpdateAsync(UserSession session, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(session);

        var existingSession = await _dbContext.UserSessions
            .FirstOrDefaultAsync(s => s.TelegramId == session.TelegramId, cancellationToken);

        if (existingSession is null)
        {
            await _dbContext.UserSessions.AddAsync(session, cancellationToken);
        }
        else
        {
            _dbContext.Entry(existingSession).CurrentValues.SetValues(session);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(long telegramId, CancellationToken cancellationToken = default)
    {
        var session = await _dbContext.UserSessions
            .FirstOrDefaultAsync(s => s.TelegramId == telegramId, cancellationToken);

        if (session is not null)
        {
            _dbContext.UserSessions.Remove(session);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}