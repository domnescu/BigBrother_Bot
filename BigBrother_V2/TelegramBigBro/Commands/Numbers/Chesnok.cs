using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.TelegramBigBro.Commands.Numbers
{
    internal class ChesnokNumberTelegram : CommandTelegram
    {
        public override string Name => "Номер Чеснока";

        public string Number = "Старший начальник курса Чесноков Владимир Валентинович - 8(911)115-54-41";

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
            return (text.StartsWith("номер") || text.Contains("у кого")) && text.Contains("номер") && text.Contains("чеснок");
        }
    }
}
