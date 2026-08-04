using IutKanoon.TelegramBot.Abstractions;

namespace IutKanoon.TelegramBot.Abstractions;

// Contract for looking up registered command handlers
public interface ICommandRegistry
{
    // Retrieves a handler for the given command string, or null if not found
    ITelegramCommandHandler? GetHandler(string command);
}
