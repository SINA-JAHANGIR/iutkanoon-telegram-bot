using Telegram.Bot;
using Telegram.Bot.Types;

namespace IutKanoon.TelegramBot.Abstractions
{
    // Defines the contract for routing incoming Telegram updates
    public interface IUpdateRouter
    {
        // Routes an incoming update to its designated handler
        Task RouteAsync(ITelegramBotClient botClient,Update update,CancellationToken cancellationToken);
    }
}
