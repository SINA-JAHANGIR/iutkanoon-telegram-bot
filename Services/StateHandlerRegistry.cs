using IutKanoon.TelegramBot.Abstractions;

namespace IutKanoon.TelegramBot.Services;

// Registers and resolves state handlers based on user state string identifiers.
public class StateHandlerRegistry : IStateHandlerRegistry
{
    private readonly Dictionary<string, IUserStateHandler> _handlers;
    private readonly ILogger<StateHandlerRegistry> _logger;

    public StateHandlerRegistry(IEnumerable<IUserStateHandler> handlers, ILogger<StateHandlerRegistry> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _handlers = new Dictionary<string, IUserStateHandler>(StringComparer.OrdinalIgnoreCase);

        foreach (var handler in handlers)
        {
            if (_handlers.ContainsKey(handler.SupportedState))
            {
                _logger.LogWarning("Duplicate state handler detected for state: {State}", handler.SupportedState);
                continue;
            }

            _handlers.Add(handler.SupportedState, handler);
            _logger.LogInformation("Registered state handler for: {State}", handler.SupportedState);
        }
    }

    public IUserStateHandler? GetHandler(string stateName)
    {
        if (string.IsNullOrWhiteSpace(stateName))
            return null;

        _handlers.TryGetValue(stateName, out var handler);
        return handler;
    }
}
