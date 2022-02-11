using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.TelegramBigBro.Commands.Numbers
{
    class DekanNumberTelegram : CommandTelegram
    {
        public override string Name => "Номер Кольцова";

        public string Number = "Декан общеинженерного факультета (он же Директор МЦОО) Кольцов Олег Вениаминович 8(812)421-38-97";

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
            if ((text.StartsWith("номер") || text.Contains("у кого")) && text.Contains("номер") && (text.Contains("декан ") || text.EndsWith("декан") ||
                text.Contains("директор") || text.EndsWith("декана") || text.Contains("кольцов")))
                return true;
            return false;
        }
    }
}
