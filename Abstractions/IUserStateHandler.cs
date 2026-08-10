using Telegram.Bot;
using Telegram.Bot.Types;

namespace IutKanoon.TelegramBot.Abstractions;

// Defines a contract for handling user interaction during a specific conversation state.
public interface IUserStateHandler
{
    // Gets the unique user state identifier handled by this instance.
    string SupportedState { get; }

    // Processes the incoming message for the current user state.
    Task HandleAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken = default);
}