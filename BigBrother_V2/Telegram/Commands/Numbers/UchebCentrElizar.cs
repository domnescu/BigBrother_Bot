using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.Telegram.Commands.Numbers
{
    internal class UchebCentrElizarNumberTelegram : CommandTelegram
    {
        public override string Name => "Номер учебного центра на Елизаре";

        public string Number = "Учебный центр на Елизаровской - 8(812)459-47-29";

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
            return (text.StartsWith("номер") || text.Contains("у кого")) && text.Contains("номер") && text.Contains("учебн") && (text.Contains("центр") || text.Contains("елизар"));
        }
    }
}
