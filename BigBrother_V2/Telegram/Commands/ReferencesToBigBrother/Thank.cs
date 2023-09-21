using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace BigBrother_V2.Telegram.Commands.ReferencesToBigBrother
{
    internal class ThankTelegram : CommandTelegram
    {
        public override string Name => "Ответ на благодарность";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database db = new();
            _ = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: db.RandomResponse("Thank"),
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            return (text.Contains("спасибо") || text.Contains("пасиб") || text.Contains("спс") || text.Contains("благодарю")) && (message.Chat.Id > 0 || db.CheckText(text, "BotNames"));
        }
    }
}
