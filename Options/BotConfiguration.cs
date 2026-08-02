namespace IutKanoon.TelegramBot.Options
{
    // Represents configuration optoins for the Telegram bot
    public class BotConfiguration
    {
        // Configuration section name in appsettings.json
        public const string Configuration = "BotConfiguration";

        // Telegram Bot API Token
        public string BotToken { get; set; } = string.Empty;
    }
}
