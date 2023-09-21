using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.Telegram.Commands.Numbers
{
    internal class MedChastiTelegram : CommandTelegram
    {
        public override string Name => "Номер медсанчасти";

        public string Number = "Медсанчасть - 8(921)903-04-95";

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
            return (text.StartsWith("номер") || text.Contains("у кого")) && text.Contains("номер") && (text.Contains("медсанчаст") || text.Contains("медчаст") || text.Contains("медпункт"));
        }
    }
}
