using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.TelegramBigBro.Commands.Numbers
{
    class OperNumberTelegram : CommandTelegram
    {
        public override string Name => "Номер оперативного дежурного";

        public string Number = "Номер оперативного дежурного - 8(812)421-33-65";

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
            if ((text.StartsWith("номер") || text.Contains("у кого")) && text.Contains("номер") && text.Contains("опер"))
            {
                return true;
            }

            return false;
        }
    }
}
