using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.TelegramBigBro.Commands.Numbers
{
    class SorokinaNumberTelegram : CommandTelegram
    {
        public override string Name => "Номер Сорокиной";

        public string Number = "Заместитель декана по учебной работе Сорокина Елена Юрьевна 8(812)421-35-86";

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
            if ((text.StartsWith("номер") || text.Contains("у кого")) && text.Contains("номер") && (text.Contains("сороки") || text.Contains("елены юрьевны")))
            {
                return true;
            }

            return false;
        }
    }
}
