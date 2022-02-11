using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace BigBrother_V2.TelegramBigBro.Commands.ReferencesToBigBrother
{
    class MessageDistributionTelegram : CommandTelegram
    {
        public override string Name => "Рассылка сообщений";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database db = new Database();
            string Text = message.Text.Remove(0, 19);
            await MessageDistributionWithTelegram("@"+ message.From.Username +" из Телеграма просил разослать следующее сообщение:\n"+ Text);
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Готово, я разослал твоё сообщение.",
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            if (text.StartsWith("бб сделай рассылку"))
                return true;
            return false;
        }
    }
}
