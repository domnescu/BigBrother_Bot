using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BigBrother_V2.TelegramBigBro.Commands.Numbers
{
    class Voen_CafedraNomberTelegram : CommandTelegram
    {
        public override string Name => "Номер начальника военной кафедры";

        public string Number = "Начальник военной кафедры Алексеев Алексей Сергеевич 8(812)321-15-93";

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
            if ((text.StartsWith("номер") || text.Contains("у кого")) && text.Contains("номер") && (text.Contains("начальник") && text.Contains("воен")))
                return true;
            return false;
        }
    }
}