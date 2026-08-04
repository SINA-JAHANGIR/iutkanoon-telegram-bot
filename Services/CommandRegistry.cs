using IutKanoon.TelegramBot.Abstractions;

namespace IutKanoon.TelegramBot.Services;

// Thread-safe registry that holds all registered command handlers mapped by command name
public class CommandRegistry : ICommandRegistry
{
    private readonly ILogger<CommandRegistry> _logger;
    private readonly Dictionary<string, ITelegramCommandHandler> _handlers;

    // Receives all registered ITelegramCommandHandler instances via Dependency Injection
    public CommandRegistry(IEnumerable<ITelegramCommandHandler> handlers, ILogger<CommandRegistry> logger)
    {
        _logger = logger;
        _handlers = new Dictionary<string, ITelegramCommandHandler>(StringComparer.OrdinalIgnoreCase);

        foreach (var handler in handlers)
        {
            if (_handlers.ContainsKey(handler.Command))
            {
                _logger.LogWarning("Duplicate command handler detected for command: {Command}. Skipping duplicate.", handler.Command);
                continue;
            }

            _handlers.Add(handler.Command, handler);
            _logger.LogInformation("Registered command handler: {Command} -> {HandlerType}", handler.Command, handler.GetType().Name);
        }
    }

    public ITelegramCommandHandler? GetHandler(string command)
    {
        if (string.IsNullOrWhiteSpace(command))
            return null;

        _handlers.TryGetValue(command, out var handler);
        return handler;
    }
}