using IutKanoon.TelegramBot.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace IutKanoon.TelegramBot.Services;

// Central update processor independent of delivery mechanism (Polling or Webhook)
public class UpdateRouter : IUpdateRouter
{
    private readonly ILogger<UpdateRouter> _logger;

    public UpdateRouter(ILogger<UpdateRouter> logger)
    {
        _logger = logger;
    }

    public async Task RouteAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        // Ignore update if it does not contain a text message
        if (update.Message is not { } message || message.Text is not { } messageText)
            return;

        var chatId = message.Chat.Id;
        var username = message.From?.Username ?? "Unknown";

        _logger.LogInformation("Received message '{MessageText}' in chat {ChatId} from @{Username}.", messageText, chatId, username);

        // Handle /start command
        if (messageText.Equals("/start", StringComparison.OrdinalIgnoreCase))
        {
            await botClient.SendMessage(
                chatId: chatId,
                text: "Welcome to IUT Kanoon Bot! 🚀",
                cancellationToken: cancellationToken);
        }
    }
}