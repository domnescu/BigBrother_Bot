using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;



namespace BigBrother_V2.TelegramBigBro.Commands.Oper
{
    class InitialsSavukTelegram : CommandTelegram
    {
        public override string Name => "Инициалы Савука";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Савук Ю. А.",
            cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if (text.Contains("инициалы") && (text.Contains("савук") || text.Contains("павук")))
            {
                return true;
            }

            return false;
        }
    }
}
