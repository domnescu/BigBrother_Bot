using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.TelegramBigBro.Commands.Numbers
{
    class AccountingTelegram : CommandTelegram
    {
        public override string Name => "Номер Бухгалтерии";

        public string Number = "Номер Бухгалтерии - 8 (812) 748-96-99";

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
            if ((text.StartsWith("номер") || text.Contains("у кого")) && text.Contains("номер") && text.Contains("бухгал"))
            {
                return true;
            }

            return false;
        }
    }
}
