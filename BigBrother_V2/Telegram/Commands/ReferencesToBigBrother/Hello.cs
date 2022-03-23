using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace BigBrother_V2.TelegramBigBro.Commands.ReferencesToBigBrother
{
    class HelloTelegram : CommandTelegram
    {
        public override string Name => "Приветствие";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Привет, прости, в Телеграме я только учусь работать и могу где-то накосячить",
                cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            Database db = new();
            if (text.StartsWith("привет") && (message.Chat.Id > 0 || db.CheckText(text, "BotNames")))
            {
                return true;
            }

            return false;
        }
    }
}
