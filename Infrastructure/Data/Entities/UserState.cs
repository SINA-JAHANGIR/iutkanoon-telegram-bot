namespace IutKanoon.TelegramBot.Infrastructure.Data.Entities;

public class UserState
{
    public long TelegramId { get; set; }
    public string CurrentState { get; set; } = string.Empty;
    public string? TempData { get; set; }
    public DateTime UpdatedAt { get; set; }
}
