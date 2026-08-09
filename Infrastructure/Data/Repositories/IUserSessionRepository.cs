using IutKanoon.TelegramBot.Infrastructure.Data.Entities;

namespace IutKanoon.TelegramBot.Infrastructure.Data.Repositories;

// Defines data access operations for Telegram user session entities
public interface IUserSessionRepository
{
    Task<UserSession?> GetByTelegramIdAsync(long telegramId, CancellationToken cancellationToken = default);
    Task AddOrUpdateAsync(UserSession session, CancellationToken cancellationToken = default);
    Task RemoveAsync(long telegramId, CancellationToken cancellationToken= default);
}
