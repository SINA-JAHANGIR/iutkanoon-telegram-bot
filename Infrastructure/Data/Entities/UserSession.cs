namespace IutKanoon.TelegramBot.Infrastructure.Data.Entities;

public class UserSession
{
    public long TelegramId { get; set; }
    public int WpUserId { get; set; }
    public string UserEmail { get; set; } = string.Empty;
    public string UserRole { get; set; } = string.Empty;

    public bool IsAuthenticated { get; set; }
    public DateTime LinkedAt { get; set; }
}
