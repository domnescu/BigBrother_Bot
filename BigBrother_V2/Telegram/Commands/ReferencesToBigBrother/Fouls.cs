using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace BigBrother_V2.Telegram.Commands.ReferencesToBigBrother
{
    internal class FoulsTelegram : CommandTelegram
    {
        public override string Name => "Ответ на оскорбление";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database db = new();
            _ = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: db.RandomResponse("AnswerOnFoul"),
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            return text.Contains("id") == false && db.CheckText(text, "fouls") && ((text.Contains("ты") && db.CheckText(text, "WarningList") == false)
                || db.CheckText(text, "BotNames"));
        }
    }
}
