using IutKanoon.TelegramBot.Abstractions;
using IutKanoon.TelegramBot.Handlers;
using IutKanoon.TelegramBot.Infrastructure.Data;
using IutKanoon.TelegramBot.Infrastructure.Data.Repositories;
using IutKanoon.TelegramBot.Options;
using IutKanoon.TelegramBot.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

// Binds configuration section to BotConfiguration model.
builder.Services.Configure<BotConfiguration>(builder.Configuration.GetSection(BotConfiguration.Configuration));

// Registers BotDbContext with SQLite.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BotDbContext>(options => options.UseSqlite(connectionString));

// Registers data repositories in the DI container.
builder.Services.AddScoped<IUserSessionRepository, UserSessionRepository>();
builder.Services.AddScoped<IUserStateRepository, UserStateRepository>();

// Registers ITelegramBotClient as a singleton instance.
builder.Services.AddSingleton<ITelegramBotClient>(sp =>
{
    var botConfig = sp.GetRequiredService<IOptions<BotConfiguration>>().Value;

    if (string.IsNullOrEmpty(botConfig.BotToken))
    {
        throw new InvalidOperationException("Telegram Bot Token is missing in configuration.");
    }

    return new TelegramBotClient(botConfig.BotToken);
});

// Registers core application services.
builder.Services.AddSingleton<IUpdateRouter, UpdateRouter>();
builder.Services.AddSingleton<ICommandRegistry, CommandRegistry>();
builder.Services.AddSingleton<IStateHandlerRegistry, StateHandlerRegistry>();

// Registers command handlers with transient lifetime.
builder.Services.AddTransient<ITelegramCommandHandler, StartCommandHandler>();

// Registers PollingBackgroundService as a hosted service.
builder.Services.AddHostedService<PollingBackgroundService>();

var app = builder.Build();

// Applies pending database migrations automatically on application startup.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BotDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.MapGet("/", () => "IutKanoon Telegram Bot is running!");

app.Run();