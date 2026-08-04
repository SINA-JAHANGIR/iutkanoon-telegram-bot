using IutKanoon.TelegramBot.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace IutKanoon.TelegramBot.Handlers;

// Handles the "/start" command for welcoming users
public class StartCommandHandler : ITelegramCommandHandler
{
    private readonly ILogger<StartCommandHandler> _logger;

    public StartCommandHandler(ILogger<StartCommandHandler> logger)
    {
        _logger = logger;
    }

    // Explicit command string for matching 
    public string Command => "/start";

    public async Task HandleAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        var firstName = message.From?.FirstName ?? "User";
        _logger.LogInformation("Processing /start command for ChatId: {ChatId}, User: {FirstName}", message.Chat.Id, firstName);

        var welcomeMessage = $"Hello dear {firstName}, welcome to IUT Kanoon bot";

        await botClient.SendMessage(chatId: message.Chat.Id, text: welcomeMessage, cancellationToken: cancellationToken);
    }
}
