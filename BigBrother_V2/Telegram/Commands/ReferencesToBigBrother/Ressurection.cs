using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace BigBrother_V2.Telegram.Commands.ReferencesToBigBrother
{
    internal class RessurectionTelegram : CommandTelegram
    {
        public override string Name => "С возвращением";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            string text;
            UserTelegram user = new(message);
            text = user.IsAdmin && message.ForwardFrom == null
                ? "Да иди ты! Не дал мне нормально отдохнуть!! "
                : "Да я уже давненько не уходил))";
            _ = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: text,
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            return text.Contains("возвращени") && (message.Chat.Id > 0 || db.CheckText(text, "BotNames"));
        }
    }
}
