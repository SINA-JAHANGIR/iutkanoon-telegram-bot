using IutKanoon.TelegramBot.Abstractions;
using IutKanoon.TelegramBot.Infrastructure.Data.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace IutKanoon.TelegramBot.Services;

// Routes incoming Telegram updates to appropriate handlers based on update type and user state.
public class UpdateRouter : IUpdateRouter
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ICommandRegistry _commandRegistry;
    private readonly ILogger<UpdateRouter> _logger;

    public UpdateRouter(IServiceProvider serviceProvider, ICommandRegistry commandRegistry, ILogger<UpdateRouter> logger)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _commandRegistry = commandRegistry ?? throw new ArgumentNullException(nameof(commandRegistry));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task RouteAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(botClient);
        ArgumentNullException.ThrowIfNull(update);

        try
        {
            // Creates a scoped service container for processing the current update safely.
            using var scope = _serviceProvider.CreateScope();
            var stateRepository = scope.ServiceProvider.GetRequiredService<IUserStateRepository>();

            switch (update.Type)
            {
                case UpdateType.Message when update.Message is not null:
                    await HandleMessageAsync(botClient, update.Message, stateRepository, cancellationToken);
                    break;

                case UpdateType.CallbackQuery when update.CallbackQuery is not null:
                    _logger.LogInformation("Received callback query from user ID: {UserId}", update.CallbackQuery.From?.Id);
                    break;

                default:
                    _logger.LogDebug("Received unsupported update type: {UpdateType}", update.Type);
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred while processing update ID: {UpdateId}", update.Id);
        }
    }

    private async Task HandleMessageAsync(ITelegramBotClient botClient, Message message,
        IUserStateRepository stateRepository, CancellationToken cancellationToken)
    {
        var telegramId = message.From?.Id;
        if (telegramId is null)
        {
            return;
        }

        var messageText = message.Text?.Trim();

        // Checks if the message is a registered command (starting with '/').
        if (!string.IsNullOrEmpty(messageText) && messageText.StartsWith('/'))
        {
            var commandName = messageText.Split(' ')[0];
            var handler = _commandRegistry.GetHandler(commandName);

            if (handler is not null)
            {
                await handler.HandleAsync(botClient, message, cancellationToken);
                return;
            }

            // Fallback for unknown commands.
            _logger.LogWarning("No handler found for command: {Command}", commandName);

            await botClient.SendMessage(
                chatId: message.Chat.Id,
                text: "Invalid command. Please check the bot menu.",
                cancellationToken: cancellationToken);
            return;
        }

        // Retrieves user state from database for multi-step conversations.
        var userState = await stateRepository.GetByTelegramIdAsync(telegramId.Value, cancellationToken);

        _logger.LogInformation("Processing message for user {TelegramId} with current state: {State}",
            telegramId, userState?.CurrentState ?? "None");

        // Fallback for regular text messages when no command or active state matches.
        await botClient.SendMessage(
            chatId: message.Chat.Id,
            text: "Your message has been received.",
            cancellationToken: cancellationToken);
    }
}