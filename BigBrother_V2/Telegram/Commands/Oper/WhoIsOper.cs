using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;



namespace BigBrother_V2.Telegram.Commands.Oper
{
    internal class WhoIsOperTelegram : CommandTelegram
    {
        public override string Name => "Кто опер ?";

        public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            Database db = new();
            UserTelegram user = new(message);
            string oper = db.GetWorkingVariable("CurrentOper");
            _ = await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: user.FirstName + ", сейчас " + oper + " опер.",
            cancellationToken: cancellationToken
            );
        }

        public override bool Contatins(Message message)
        {
            string text = message.Text.ToLower();
            return text.Contains("кто") && text.Contains("опер") && text.Contains("заступ") == false && text.Contains("будет") == false && text.Contains("завтра") == false;
        }
    }
}
