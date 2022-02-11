using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.TelegramBigBro.Commands.Numbers
{
    class ZamDekNumberTelegram : CommandTelegram
    {
        public override string Name => "номер заместителя декана (директора)";

        public string Number = "Заместитель декана Капустин Сергей Викторович - 8(911)930-49-11";

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
            if ((text.StartsWith("номер") || text.Contains("у кого")) && text.Contains("номер") && (text.Contains("замдек") || text.Contains("капустин")))
                return true;
            return false;
        }
    }
}
