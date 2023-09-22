using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.TelegramBigBro.Commands.Numbers
{
    internal class Electric_SantehnicNumberTelegram : CommandTelegram
    {
        public override string Name => "Номер вызова сантехника или электрика";

        public string Number = "Вызов электрика, сантехника УГ-4 - 8(812)421-49-02";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            _ = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: Number,
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return (text.StartsWith("номер") || text.Contains("у кого")) && text.Contains("номер") && (text.Contains("сантехник") || text.Contains("электрик"));
        }
    }
}
