using Telegram.Bot;
using Telegram.Bot.Types;

namespace IutKanoon.TelegramBot.Abstractions;

// Abstraction contract for handling specific Telegram commands
public interface ITelegramCommandHandler
{
    // The command string that this handler processes (e.g., "/start")
    string Command { get; }

    // Executes the core logic for the command
    Task HandleAsync(ITelegramBotClient botClient,Message message,CancellationToken cancellationToken);
}
