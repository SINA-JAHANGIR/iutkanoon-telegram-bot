using IutKanoon.TelegramBot.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace IutKanoon.TelegramBot.Services;

// Routes incoming Telegram updates to appropriate command handlers
public class UpdateRouter : IUpdateRouter
{
    private readonly ILogger<UpdateRouter> _logger;
    private readonly ICommandRegistry _commandRegistry;

    public UpdateRouter(ICommandRegistry commandRegistry, ILogger<UpdateRouter> logger)
    {
        _commandRegistry = commandRegistry;
        _logger = logger;
    }

    public async Task RouteAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        // Currently only processing text messages
        if (update.Type != UpdateType.Message || update.Message is not { Text: { } messageText })
        {
            return;
        }

        var message = update.Message;
        _logger.LogInformation("Received message '{Text}' from ChatId: {ChatId}", messageText, message.Chat.Id);

        // Extract command name (handling potential space-separated arguments)
        var commandName = messageText.Split(' ')[0];

        // Lookup handler in registry
        var handler = _commandRegistry.GetHandler(commandName);

        if (handler is not null)
        {
            await handler.HandleAsync(botClient, message, cancellationToken);
            return;
        }

        // Fallback for unknown commands
        _logger.LogWarning("No handler found for command: {Command}", commandName);

        await botClient.SendMessage(
            chatId: message.Chat.Id,
            text: "Command not found!",
            cancellationToken: cancellationToken);
    }
}