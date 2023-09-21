using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;



namespace BigBrother_V2.Telegram.Commands.Oper
{
    internal class InitialsStepanovTelegram : CommandTelegram
    {
        public override string Name => "Инициалы Степанова";


        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            _ = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Степанов Е.И.",
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.Contains("инициалы") && text.Contains("степанов");
        }
    }
}
