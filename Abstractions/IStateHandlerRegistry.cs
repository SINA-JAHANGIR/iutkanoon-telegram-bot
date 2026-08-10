namespace IutKanoon.TelegramBot.Abstractions;

// Defines a contract for registering and retrieving state handlers dynamically.
public interface IStateHandlerRegistry
{
    // Retrieves the state handler associated with the specified state name.
    IUserStateHandler? GetHandler(string stateName);
}