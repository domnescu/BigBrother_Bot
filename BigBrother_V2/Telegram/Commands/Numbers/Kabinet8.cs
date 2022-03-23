using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.TelegramBigBro.Commands.Numbers
{
    class Kabinet8NumberTelegram : CommandTelegram
    {
        public override string Name => "Номер 8 кабинета";

        public string Number = "8 кабинет - 8(911)721-19-04";

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
            if ((text.StartsWith("номер") || text.Contains("у кого")) && text.Contains("номер") && ((text.Contains("8") || text.Contains("восьм")) && text.Contains("кабинет")))
            {
                return true;
            }

            return false;
        }
    }
}
