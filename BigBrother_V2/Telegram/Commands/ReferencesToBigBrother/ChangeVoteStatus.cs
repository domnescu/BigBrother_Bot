using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace BigBrother_V2.Telegram.Commands.ReferencesToBigBrother
{
    internal class ChangeVoteStatusTelegram : CommandTelegram
    {
        public override string Name => "Принудительное изменение опера";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            UserTelegram user = new(message);
            string Text = message.Text.ToLower();
            string Response = string.Empty;
            Database db = new();
            if (user.IsAdmin && message.ForwardFrom == null)
            {
                if (Text.Contains("открой"))
                {
                    db.SetWorkingVariable("VoteAcces", "open");
                    Response = "Голосование открыто.";
                }
                else if (Text.Contains("закрой"))
                {
                    db.SetWorkingVariable("VoteAcces", "closed");
                    db.CleanTable("votes");
                    Response = "Голосование закрыто.";
                }

            }
            else
            {
                Response = user.IsAdmin && message.ForwardFrom != null
                    ? "Пересланное сообщение Админа сообщества ? Серьёзно ? Это так не работает)"
                    : "Только Администраторы сообщества имеют право менять статус голосования";
            }

            _ = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: Response,
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            return text.Contains("голосование") && (message.Chat.Id > 0 || db.CheckText(text, "BotNames"));
        }
    }
}
