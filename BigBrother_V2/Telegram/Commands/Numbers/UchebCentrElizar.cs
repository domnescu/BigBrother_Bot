using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.TelegramBigBro.Commands.Numbers
{
    class UchebCentrElizarNumberTelegram : CommandTelegram
    {
        public override string Name => "Номер учебного центра на Елизаре";

        public string Number = "Учебный центр на Елизаровской - 8(812)459-47-29";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: Number,
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if ((text.StartsWith("номер") || text.Contains("у кого")) && text.Contains("номер") && (text.Contains("учебн") && (text.Contains("центр") || text.Contains("елизар"))))
            {
                return true;
            }

            return false;
        }
    }
}
