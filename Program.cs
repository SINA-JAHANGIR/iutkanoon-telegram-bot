using IutKanoon.TelegramBot.Abstractions;
using IutKanoon.TelegramBot.Handlers;
using IutKanoon.TelegramBot.Options;
using IutKanoon.TelegramBot.Services;
using IutKanoon.TelegramBot.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Telegram.Bot;


var builder = WebApplication.CreateBuilder(args);

// Bind configuration section to BotConfiguration model
builder.Services.Configure<BotConfiguration>(builder.Configuration.GetSection(BotConfiguration.Configuration));

// Register BotDbContext with SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BotDbContext>(options => options.UseSqlite(connectionString));

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

// Register Core Services
builder.Services.AddSingleton<IUpdateRouter,UpdateRouter>();
builder.Services.AddSingleton<ICommandRegistry,CommandRegistry>();

// Register Command Handlers (As Transient to allow BotDbContext injection)
builder.Services.AddTransient<ITelegramCommandHandler, StartCommandHandler>();

// Register PollingBackgroundService as a Hosted Service
builder.Services.AddHostedService<PollingBackgroundService>();

var app = builder.Build();

app.MapGet("/", () => "IutKanoon Telegram Bot is running!");

app.Run();