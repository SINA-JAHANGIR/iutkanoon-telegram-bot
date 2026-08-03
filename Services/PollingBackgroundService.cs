using IutKanoon.TelegramBot.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace IutKanoon.TelegramBot.Services;

// Background service responsible for receiving updates via Long Polling in Development mode
public class PollingBackgroundService : BackgroundService
{
    private readonly ITelegramBotClient _botClient;
    private readonly IUpdateRouter _updateRouter;
    private readonly ILogger<PollingBackgroundService> _logger;

    public PollingBackgroundService(
        ITelegramBotClient botClient,
        IUpdateRouter updateRouter,
        ILogger<PollingBackgroundService> logger)
    {
        _botClient = botClient;
        _updateRouter = updateRouter;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting Telegram Bot Polling service...");

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>(), // Receive all update types
            DropPendingUpdates = true // Ignore old messages sent while the bot was offline
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await _botClient.ReceiveAsync(
                    updateHandler: async (client, update, ct) => await _updateRouter.RouteAsync(client, update, ct),
                    errorHandler: (client, exception, ct) =>
                    {
                        _logger.LogError(exception, "Error occurred during Telegram polling.");
                        return Task.CompletedTask;
                    },
                    receiverOptions: receiverOptions,
                    cancellationToken: stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Polling loop encountered an exception. Retrying in 5 seconds...");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}