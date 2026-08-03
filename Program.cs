using IutKanoon.TelegramBot.Abstractions;
using IutKanoon.TelegramBot.Options;
using IutKanoon.TelegramBot.Services;
using Microsoft.Extensions.Options;
using Telegram.Bot;


var builder = WebApplication.CreateBuilder(args);

// Bind configuration section to BotConfiguration model
builder.Services.Configure<BotConfiguration>(builder.Configuration.GetSection(BotConfiguration.Configuration));

// Register ITelegramBotClient as a Singleton in DI container
builder.Services.AddSingleton<ITelegramBotClient>(sp =>
{
    var botConfig = sp.GetRequiredService<IOptions<BotConfiguration>>().Value;

    if (string.IsNullOrEmpty(botConfig.BotToken))
    {
        throw new InvalidOperationException("Telegram Bot Token is missing in configuration.");
    }

    return new TelegramBotClient(botConfig.BotToken);
});

// Register UpdateRouter
builder.Services.AddSingleton<IUpdateRouter,UpdateRouter>();

// Register PollingBackgroundService as a Hosted Service
builder.Services.AddHostedService<PollingBackgroundService>();

var app = builder.Build();

app.MapGet("/", () => "IutKanoon Telegram Bot is running!");

app.Run();
