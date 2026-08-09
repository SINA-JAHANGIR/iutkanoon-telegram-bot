using IutKanoon.TelegramBot.Infrastructure.Data.Entities;
using IutKanoon.TelegramBot.Infrastructure.Data.Repositories;

namespace IutKanoon.TelegramBot.Infrastructure.Data.Repositories;

// Defines data access operations for Telegram user state entities
public interface IUserStateRepository
{
    Task<UserState?> GetByTelegramIdAsync(long telegramId, CancellationToken cancellationToken = default);
    Task AddOrUpdateAsync(UserState state, CancellationToken cancellationToken = default);
    Task RemoveAsync(long telegramId, CancellationToken cancellationToken= default);
}
